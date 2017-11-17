using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Xmf2.iOS.Controls.ItemControls
{
	public class ItemsControlView<TCell, TItemData> : UIView
		where TCell : BaseItemCell<TItemData>, new()
		where TItemData : class
	{
		public class ItemSelectedEventArgs : EventArgs
		{
			public TItemData Item { get; }

			public int Position { get; }

			public ItemSelectedEventArgs(TItemData item, int position)
			{
				Item = item;
				Position = position;
			}
		}

		#region Public Properties


		private readonly int? _cellHeight;
		private readonly bool _hasSeparator;
		private bool _canLayout;

		private readonly List<TCell> _usingCells = new List<TCell>();
		private readonly Queue<TCell> _usableCells = new Queue<TCell>();

		private readonly List<UIView> _usingSeparators = new List<UIView>();
		private readonly Queue<UIView> _usableSeparators = new Queue<UIView>();

		public event EventHandler<ItemSelectedEventArgs> ItemSelected;

		private IReadOnlyList<TItemData> _items;
		public virtual IReadOnlyList<TItemData> Items
		{
			get => _items;
			set
			{
				if (_items != value)
				{
					_items = value;
					UpdateListView(value);
				}
			}
		}

		public bool CanSelectItems { get; set; } = true;
		public UIColor SeparatorColor { get; set; } = UIColor.DarkGray;
		public int SeparatorHeight { get; set; } = 1;
		public UIColor FeedbackColor { get; set; } = UIColor.LightGray;
		public Func<UIView> CreateSeparator { get; set; }

		public List<TCell> UsingCells => _usingCells;

		#endregion

		public ItemsControlView(int? fixedHeight = null, bool hasSeparator = true)
		{
			_cellHeight = fixedHeight;
			_hasSeparator = hasSeparator;
		}

		public void Layout()
		{
			_canLayout = true;
		}

		protected void UpdateListView(IReadOnlyList<TItemData> items)
		{
			if (items == null)
			{
				items = new List<TItemData>();
			}

			int itemIndex;
			//try rebinding currently used cells
			for (itemIndex = 0; itemIndex < _usingCells.Count && itemIndex < items.Count; itemIndex++)
			{
				_usingCells[itemIndex].Model = items[itemIndex];
				_usingCells[itemIndex].Position = itemIndex;
			}

			//more cells than elements ?
			if (itemIndex >= items.Count)
			{
				//send old cells & separators to reusable queue
				int removeLength = _usingCells.Count - itemIndex;
				if (removeLength > 0)
				{
					for (int i = itemIndex; i < _usingCells.Count; ++i)
					{
						TCell cell = _usingCells[i];
						cell.DataContext = null;
						cell.Position = itemIndex;
						_usableCells.Enqueue(cell);

						cell.ViewWillDisappear();
						cell.RemoveFromSuperview();
						cell.ViewDidDisappear();
					}

					_usingCells.RemoveRange(itemIndex, removeLength);

					if (_hasSeparator)
					{
						//remove unneeded separators
						int separatorIndex = Math.Max(0, itemIndex - 1);
						for (int i = separatorIndex; i < _usingSeparators.Count; ++i)
						{
							UIView sep = _usingSeparators[i];
							sep.RemoveFromSuperview();
							_usableSeparators.Enqueue(sep);
						}
						_usingSeparators.RemoveRange(separatorIndex, Math.Min(_usingSeparators.Count, removeLength));
					}

					UIView lastView = _usingCells.LastOrDefault();
					if (lastView != null)
					{
						this.AnchorBottom(lastView);
					}
				}
			}
			else
			{
				//need more cells
				UIView topView = null;
				if (_usingCells.Count > 0)
				{
					NSLayoutConstraint constraint = Constraints
						.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Bottom && x.SecondAttribute == NSLayoutAttribute.Bottom);

					if (constraint != null)
					{
						RemoveConstraint(constraint);
					}
					topView = _usingCells.Last();
				}

				for (; itemIndex < items.Count; ++itemIndex)
				{
					if (itemIndex > 0 && _hasSeparator)
					{
						//add separator
						UIView sep = GetOrCreateSeparator();

						Add(sep);
						this.VerticalSpace(topView, sep, 0)
							.CenterAndFillWidth(sep);

						topView = sep;
					}

					//Add Component Cell
					TCell cell = GetOrCreateCell(items[itemIndex]);
					cell.Position = itemIndex;

					cell.ViewDidLoad();
					Add(cell);
					cell.ViewDidAppear();

					this.CenterAndFillWidth(cell);

					if (topView == null)
					{
						this.AnchorTop(cell);
					}
					else
					{
						this.VerticalSpace(topView, cell, 0);
					}

					topView = cell;
				}

				if (topView != null)
				{
					this.AnchorBottom(topView);
				}
			}

			if (_canLayout)
			{
				LayoutIfNeeded();
			}
		}

		private UIView GetOrCreateSeparator()
		{
			if (_usableSeparators.Any())
			{
				UIView oldSep = _usableSeparators.Dequeue();
				_usingSeparators.Add(oldSep);
				return oldSep;
			}
			UIView sep = (this.CreateSeparator ?? this.CreateDefaultSeparator)();
			_usingSeparators.Add(sep);
			return sep;
		}

		protected virtual TCell GetOrCreateCell(TItemData item)
		{
			if (_usableCells.Any())
			{
				TCell oldCellContent = _usableCells.Dequeue();
				if (_cellHeight.HasValue)
				{
					oldCellContent.ConstrainHeight(_cellHeight.Value);
				}
				oldCellContent.ViewDidLoad();

				oldCellContent.Model = item;

				_usingCells.Add(oldCellContent);
				oldCellContent.ViewDidAppear();

				return oldCellContent;
			}

			TCell cellContent = new TCell();
			if (_cellHeight.HasValue)
			{
				cellContent.ConstrainHeight(_cellHeight.Value);
			}
			cellContent.ViewDidLoad();
			cellContent.Model = item;
			cellContent.AutoLayout();
			cellContent.Bind();
			_usingCells.Add(cellContent);
			cellContent.ViewDidAppear();

			if (CanSelectItems)
			{
				UITapGestureRecognizer gestureRecognizer = new UITapGestureRecognizer(ItemSelectedAction);
				cellContent.AddGestureRecognizer(gestureRecognizer);
			}
			return cellContent;
		}

		public TCell FindCell(TItemData item)
		{
			return _usingCells.FirstOrDefault<TCell>(i => i.DataContext == item);
		}

		private void ItemSelectedAction(UITapGestureRecognizer recognizer)
		{
			try
			{
				TCell cell = recognizer.View as TCell;
				if (cell == null)
				{
					return;
				}
				var itemSelectEventArg = new ItemSelectedEventArgs(cell.Model, cell.Position);
				var _oldBackground = cell.BackgroundColor;
				Animate(0.1,
					animation: () => cell.BackgroundColor = FeedbackColor,
					completion: () =>
					{
						ItemSelected?.Invoke(this, itemSelectEventArg);
						Animate(0.1, () =>
						{
							cell.BackgroundColor = cell.SelectedColor ?? _oldBackground;
						}, ActionHelper.NoOp);
					}
				);
			}
			catch (Exception ex)
			{
#if DEBUG
				Console.WriteLine($"ItemSelected: Exception: {ex}");
#endif
			}
		}

		private UIView CreateDefaultSeparator()
		{
			return new UIView() { BackgroundColor = SeparatorColor }.ConstrainHeight(SeparatorHeight);
		}
	}
}
