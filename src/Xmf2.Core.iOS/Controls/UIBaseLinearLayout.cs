using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UIBaseLinearLayout : UIView
	{
		public interface IConstraintCreator
		{
			NSLayoutConstraint AnchorStart(UIView container, UIView cell);

			NSLayoutConstraint[] Space(UIView previousCell, UIView nextCell);
			NSLayoutConstraint[] Space(UIView previousCell, UIView separator, UIView nextCell);

			NSLayoutConstraint AnchorEnd(UIView cell, UIView container);

			NSLayoutConstraint[] FillSize(UIView container, UIView cell);
			NSLayoutConstraint[] FillSizeSeparator(UIView container, UIView separator);
		}

		private readonly List<UIView> _views = new List<UIView>();

		private NSLayoutConstraint _startConstraint;
		private readonly List<NSLayoutConstraint[]> _fillSizeConstraints = new List<NSLayoutConstraint[]>();
		private readonly List<NSLayoutConstraint[]> _spacingConstraints = new List<NSLayoutConstraint[]>();
		private NSLayoutConstraint _endConstraint;

		private readonly List<UIView> _separatorViews = new List<UIView>();
		private readonly bool _withSeparators;
		private Func<UIView> _separatorCreator;


		private IConstraintCreator _constraintCreator;
		public IConstraintCreator ConstraintCreator
		{
			get => _constraintCreator;
			set
			{
				_constraintCreator = value;
				this.ResetConstraints();
			}
		}

		public UIBaseLinearLayout(IConstraintCreator constraintCreator, Func<UIView> separatorCreator = null)
		{
			_constraintCreator = constraintCreator;
			_separatorCreator = separatorCreator;
			_withSeparators = separatorCreator != null;
		}

		public override void AddSubview(UIView view)
		{
			var previousViews = this._views.ToArray();

			this.AddSubviewInternal(view);

			if (ConstraintCreator == null)
			{
				return;
			}
			if (previousViews.None())
			{
				this.AnchorStartConstraint(view);
				this.FillSizeConstraint(view);
				this.AnchorEndConstraint(view);
			}
			else
			{
				if (_withSeparators)
				{
					var separator = _separatorCreator();
					_separatorViews.Add(separator);
					base.AddSubview(separator);

					this.SpaceConstraint(previousViews.Last(), separator, view);
					this.FillSizeSeparatorConstraint(separator);
				}
				else
				{
					this.SpaceConstraint(previousViews.Last(), view);
				}
				this.RemoveEndConstraint();
				this.FillSizeConstraint(view);
				this.AnchorEndConstraint(view);
			}
		}

		public void AddSubviews(IEnumerable<UIView> views) => AddSubviews(views.ToArray());

		/// <remarks>
		/// On ne peut pas overrider <see cref="UIView.AddSubviews(UIView[])"/>,
		/// mais il reste préférable de le redéfinir car cette méthode provoque plusieurs appels de 
		/// <see cref="AddSubview(UIView)"/> ce qui est contreperformant.
		/// </remarks>
		public new void AddSubviews(params UIView[] viewsToAdd)
		{
			if (viewsToAdd.None())
			{
				return;
			}

			var previousViews = _views.ToArray();
			this.AddSubviewsInternal(viewsToAdd);
			if (ConstraintCreator != null)
			{
				if (previousViews.None())
				{
					this.SetConstraints(viewsToAdd);
				}
				else
				{
					this.RemoveEndConstraint();
					var previousView = previousViews.Last();
					for (int i = 0; i < viewsToAdd.Length; i++)
					{
						var view = viewsToAdd[i];
						if (_withSeparators)
						{
							UIView separator;
							if (previousViews.Length + i >= _separatorViews.Count)
							{
								separator = _separatorCreator();
								separator.TranslatesAutoresizingMaskIntoConstraints = false;
								_separatorViews.Add(separator);
								base.AddSubview(separator);
							}
							else
							{
								separator = _separatorViews[previousViews.Length + i];
							}
							this.SpaceConstraint(previousView, separator, view);
							this.FillSizeConstraint(view);
							this.FillSizeSeparatorConstraint(separator);
							previousView = view;
						}
						else
						{
							this.SpaceConstraint(previousView, view);
							this.FillSizeConstraint(view);
							previousView = view;
						}
					}

					this.AnchorEndConstraint(viewsToAdd.Last());
				}
			}
		}

		/// <remarks>
		/// Permet de passer de préférence par cette méthode plutôt que par les méthodes d'extension
		/// <see cref="CreatorExtensions.WithSubviews"/>, qui appeleraient le contreperformant <see cref="UIView.AddSubviews(UIView[])"/>
		/// </remarks>
		public UIBaseLinearLayout WithSubviews(params UIView[] viewsToAdd)
		{
			this.AddSubviews(viewsToAdd);
			return this;
		}

		public ISetResult Set(params UIView[] views)
		{
			//TODO: Handle view re-ordering.
			var toRemove = _views.Except(views).ToList();
			var toAdd = views.Except(_views).ToList();

			if (toRemove.Any() || toAdd.Any())
			{
				ClearConstraints();
				foreach (var v in toRemove)
				{
					_views.Remove(v);
					v.RemoveFromSuperview();
				}
				foreach (var v in toAdd)
				{
					this.AddSubviewInternal(v);
				}
				SetConstraints(_views);
			}
			return new SetResult
			{
				RemovedViews = toRemove,
				AddedViews = toAdd
			};
		}

		public void ResetConstraints()
		{
			this.ClearConstraints();
			this.SetConstraints(_views);
		}

		private void SetConstraints(IEnumerable<UIView> views)
		{
			if (_views.None() || ConstraintCreator == null)
			{
				return;
			}

			UIView firstView = views.First();
			this.AnchorStartConstraint(firstView);
			this.FillSizeConstraint(firstView);

			UIView previousView = firstView;
			if (_withSeparators)
			{
				var usableArray = views.ToArray();
				for (int i = 1; i < usableArray.Length; i++)
				{
					var view = usableArray[i];
					UIView separator;
					if (i >= _separatorViews.Count)
					{
						separator = _separatorCreator();
						separator.TranslatesAutoresizingMaskIntoConstraints = false;
						_separatorViews.Add(separator);
						base.AddSubview(separator);
					}
					else
					{
						separator = _separatorViews[i - 1];
					}

					this.SpaceConstraint(previousView, separator, view);
					this.FillSizeConstraint(view);
					this.FillSizeSeparatorConstraint(separator);
					previousView = view;
				}
			}
			else
			{
				foreach (var view in views.Skip(1))
				{
					this.SpaceConstraint(previousView, view);
					this.FillSizeConstraint(view);
					previousView = view;
				}
			}
			this.AnchorEndConstraint(lastView: previousView);
		}

		public void Clear()
		{
			ClearConstraints();

			for (var i = 0; i < _views.Count; i++)
			{
				_views[i].RemoveFromSuperview();
			}
			_views.Clear();

			foreach (var separator in _separatorViews)
			{
				separator.RemoveFromSuperview();
			}
			_separatorViews.Clear();
		}

		private void ClearConstraints()
		{
			this.RemoveStartConstraint();
			this.RemoveEndConstraint();
			this.RemoveFillingSizeConstraint();
			this.RemoveSpacingConstraints();
		}

		private void AddSubviewInternal(UIView view)
		{
			view.TranslatesAutoresizingMaskIntoConstraints = false;
			base.AddSubview(view);
			_views.Add(view);
		}

		private void AddSubviewsInternal(params UIView[] viewsToAdd)
		{
			for (int i = 0; i < viewsToAdd.Length; i++)
			{
				var view = viewsToAdd[i];
				view.TranslatesAutoresizingMaskIntoConstraints = false;
				base.AddSubview(view);

				bool notLast = i != viewsToAdd.Length - 1;
				if (_withSeparators && notLast)
				{
					var separator = _separatorCreator();
					separator.TranslatesAutoresizingMaskIntoConstraints = false;
					_separatorViews.Add(separator);
					base.AddSubview(separator);
				}
			}

			_views.AddRange(viewsToAdd);
		}

		private void SpaceConstraint(UIView previousCell, UIView nextCell)
		{
			var constraint = ConstraintCreator.Space(previousCell, nextCell);
			_spacingConstraints.Add(constraint);
			this.AddConstraints(constraint);
		}

		private void SpaceConstraint(UIView previousCell, UIView separator, UIView nextCell)
		{
			var constraints = ConstraintCreator.Space(previousCell, separator, nextCell);
			_spacingConstraints.Add(constraints);
			this.AddConstraints(constraints);
		}

		private void FillSizeConstraint(UIView view)
		{
			NSLayoutConstraint[] cellConstraints = ConstraintCreator.FillSize(this, view);
			_fillSizeConstraints.Add(cellConstraints);
			AddConstraints(cellConstraints);
		}

		private void FillSizeSeparatorConstraint(UIView separator)
		{
			var constraints = ConstraintCreator.FillSizeSeparator(this, separator);
			_fillSizeConstraints.Add(constraints);
			AddConstraints(constraints);
		}

		private void AnchorStartConstraint(UIView firstView)
		{
			if (_startConstraint != null)
			{
				throw new Exception($"{nameof(_startConstraint)} was not empty");
			}
			firstView.TranslatesAutoresizingMaskIntoConstraints = false;
			_startConstraint = ConstraintCreator.AnchorStart(container: this, cell: firstView);
			this.AddConstraint(_startConstraint);
		}

		private void AnchorEndConstraint(UIView lastView)
		{
			if (_endConstraint != null)
			{
				throw new Exception($"{nameof(_endConstraint)} was not empty");
			}
			lastView.TranslatesAutoresizingMaskIntoConstraints = false;
			_endConstraint = ConstraintCreator.AnchorEnd(cell: lastView, container: this);
			this.AddConstraint(_endConstraint);
		}

		private void RemoveSpacingConstraints()
		{
			foreach (var constraints in _spacingConstraints)
			{
				this.RemoveConstraints(constraints);
				foreach (var constraint in constraints)
				{
					constraint.Dispose();
				}
			}
			_spacingConstraints.Clear();
		}

		private void RemoveStartConstraint()
		{
			if (_startConstraint != null)
			{
				this.RemoveConstraint(_startConstraint);
				_startConstraint.Dispose();
				_startConstraint = null;
			}
		}

		private void RemoveEndConstraint()
		{
			if (_endConstraint != null)
			{
				this.RemoveConstraint(_endConstraint);
				_endConstraint.Dispose();
				_endConstraint = null;
			}
		}

		private void RemoveFillingSizeConstraint()
		{
			foreach (var constraints in _fillSizeConstraints)
			{
				this.RemoveConstraints(constraints);
				foreach (var constraint in constraints)
				{
					constraint.Dispose();
				}
			}
			_fillSizeConstraints.Clear();
		}

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					Clear();
					_separatorCreator = null;
				}
			}
			finally
			{
				base.Dispose(disposing);

			}
		}

		#region Nested class
		public interface ISetResult
		{
			IReadOnlyList<UIView> RemovedViews { get; }
			IReadOnlyList<UIView> AddedViews { get; }

			void DisposeRemoved();
		}

		private class SetResult : ISetResult
		{
			public IReadOnlyList<UIView> RemovedViews { get; set; }
			public IReadOnlyList<UIView> AddedViews { get; set; }

			public void DisposeRemoved()
			{
				foreach (var v in RemovedViews)
				{
					v.Dispose();
				}

			}
		}
		#endregion Nested class
	}
}