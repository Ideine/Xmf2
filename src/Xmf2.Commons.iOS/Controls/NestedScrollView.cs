using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public class VerticalConstraintCreator : UIBaseLinearLayout.IConstraintCreator
	{
		public static NSLayoutConstraint AnchorStart(UIView container, UIView cell)
		{
			return NSLayoutConstraint.Create(cell, NSLayoutAttribute.Top, NSLayoutRelation.Equal, container, NSLayoutAttribute.Top, 1f, 0);
		}
		public static NSLayoutConstraint Space(UIView previousCell, UIView nextCell)
		{
			return NSLayoutConstraint.Create(nextCell, NSLayoutAttribute.Top, NSLayoutRelation.Equal, previousCell, NSLayoutAttribute.Bottom, 1f, 0);
		}
		public static NSLayoutConstraint AnchorEnd(UIView cell, UIView container)
		{
			return NSLayoutConstraint.Create(container, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, cell, NSLayoutAttribute.Bottom, 1f, 0);
		}
		public static NSLayoutConstraint[] FillSize(UIView container, UIView cell)
		{
			return new[]
			{
				NSLayoutConstraint.Create(container, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, cell, NSLayoutAttribute.CenterX, 1f, 0),
				NSLayoutConstraint.Create(container, NSLayoutAttribute.Width, NSLayoutRelation.Equal, cell, NSLayoutAttribute.Width, 1f, 0),
			};
		}

		NSLayoutConstraint UIBaseLinearLayout.IConstraintCreator.AnchorEnd(UIView cell, UIView container) => AnchorEnd(cell, container);
		NSLayoutConstraint UIBaseLinearLayout.IConstraintCreator.AnchorStart(UIView container, UIView cell) => AnchorStart(container, cell);
		NSLayoutConstraint[] UIBaseLinearLayout.IConstraintCreator.FillSize(UIView container, UIView cell) => FillSize(container, cell);
		NSLayoutConstraint UIBaseLinearLayout.IConstraintCreator.Space(UIView previousCell, UIView nextCell) => Space(previousCell, nextCell);
	}
	
	public class HorizontalConstraintCreator : UIBaseLinearLayout.IConstraintCreator
	{
		public static NSLayoutConstraint AnchorStart(UIView container, UIView cell)
		{
			return NSLayoutConstraint.Create(cell, NSLayoutAttribute.Left, NSLayoutRelation.Equal, container, NSLayoutAttribute.Left, 1f, 0);
		}
		public static NSLayoutConstraint Space(UIView previousCell, UIView nextCell)
		{
			return NSLayoutConstraint.Create(nextCell, NSLayoutAttribute.Left, NSLayoutRelation.Equal, previousCell, NSLayoutAttribute.Right, 1f, 0);
		}
		public static NSLayoutConstraint AnchorEnd(UIView cell, UIView container)
		{
			return NSLayoutConstraint.Create(container, NSLayoutAttribute.Right, NSLayoutRelation.Equal, cell, NSLayoutAttribute.Right, 1f, 0);
		}
		public static NSLayoutConstraint[] FillSize(UIView container, UIView cell)
		{
			return new[]
			{
				NSLayoutConstraint.Create(container, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, cell, NSLayoutAttribute.CenterY, 1f, 0),
				NSLayoutConstraint.Create(container, NSLayoutAttribute.Height, NSLayoutRelation.Equal, cell, NSLayoutAttribute.Height, 1f, 0),
			};
		}

		NSLayoutConstraint UIBaseLinearLayout.IConstraintCreator.AnchorEnd(UIView cell, UIView container)
			=> AnchorEnd(cell, container);

		NSLayoutConstraint UIBaseLinearLayout.IConstraintCreator.AnchorStart(UIView container, UIView cell)
			=> AnchorStart(container, cell);

		NSLayoutConstraint[] UIBaseLinearLayout.IConstraintCreator.FillSize(UIView container, UIView cell)
			=> FillSize(container, cell);

		NSLayoutConstraint UIBaseLinearLayout.IConstraintCreator.Space(UIView previousCell, UIView nextCell)
			=> Space(previousCell, nextCell);
	}
	
	public class UIBaseLinearLayout : UIView
	{
		public interface IConstraintCreator
		{
			NSLayoutConstraint AnchorStart(UIView container, UIView cell);

			NSLayoutConstraint Space(UIView previousCell, UIView nextCell);

			NSLayoutConstraint AnchorEnd(UIView cell, UIView container);

			NSLayoutConstraint[] FillSize(UIView container, UIView cell);
		}

		private readonly List<UIView> _views = new List<UIView>();

		private NSLayoutConstraint _startConstraint;
		private readonly List<NSLayoutConstraint[]> _fillSizeConstraints = new List<NSLayoutConstraint[]>();
		private readonly List<NSLayoutConstraint> _spacingConstraints = new List<NSLayoutConstraint>();
		private NSLayoutConstraint _endConstraint;

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

		public UIBaseLinearLayout(IConstraintCreator constraintCreator)
		{
			_constraintCreator = constraintCreator;
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
				this.RemoveEndConstraint();
				this.SpaceConstraint(previousViews.Last(), view);
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
					foreach (var view in viewsToAdd)
					{
						this.SpaceConstraint(previousView, view);
						this.FillSizeConstraint(view);
						previousView = view;
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
			return new SetResult()
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
			foreach (var view in views.Skip(1))
			{
				this.SpaceConstraint(previousView, view);
				this.FillSizeConstraint(view);
				previousView = view;
			}
			this.AnchorEndConstraint(views.Last());
		}

		public void Clear()
		{
			ClearConstraints();
			for (var i = 0; i < _views.Count; i++)
			{
				_views[i].RemoveFromSuperview();
			}
			_views.Clear();
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
			foreach (var view in viewsToAdd)
			{
				view.TranslatesAutoresizingMaskIntoConstraints = false;
				base.AddSubview(view);
			}
			_views.AddRange(viewsToAdd);
		}
		private void SpaceConstraint(UIView previousCell, UIView nextCell)
		{
			var constraint = ConstraintCreator.Space(previousCell, nextCell);
			_spacingConstraints.Add(constraint);
			this.AddConstraint(constraint);
		}
		private void FillSizeConstraint(UIView view)
		{
			view.TranslatesAutoresizingMaskIntoConstraints = false;
			NSLayoutConstraint[] cellConstraints = ConstraintCreator.FillSize(this, view);
			_fillSizeConstraints.Add(cellConstraints);
			AddConstraints(cellConstraints);
		}
		private void AnchorStartConstraint(UIView view)
		{
			if (_startConstraint != null)
			{
				throw new Exception($"{nameof(_startConstraint)} was not empty");
			}
			view.TranslatesAutoresizingMaskIntoConstraints = false;
			_startConstraint = ConstraintCreator.AnchorStart(container: this, cell: view);
			this.AddConstraint(_startConstraint);
		}
		private void AnchorEndConstraint(UIView view)
		{
			if (_endConstraint != null)
			{
				throw new Exception($"{nameof(_endConstraint)} was not empty");
			}
			view.TranslatesAutoresizingMaskIntoConstraints = false;
			_endConstraint = ConstraintCreator.AnchorEnd(cell: this, container: view);
			this.AddConstraint(_endConstraint);
		}

		private void RemoveSpacingConstraints()
		{
			foreach (var spacingConstraint in _spacingConstraints)
			{
				this.RemoveConstraint(spacingConstraint);
				spacingConstraint.Dispose();
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
			foreach (var constraint in _fillSizeConstraints.SelectMany(x => x))
			{
				this.RemoveConstraint(constraint);
				constraint.Dispose();
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
				}
			}
			finally { base.Dispose(disposing); }
		}

		#region Nested Types
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
		#endregion Nested Types
	}
	
	public class UILinearLayout : UIBaseLinearLayout
	{
		public enum LayoutOrientation
		{
			Vertical,
			Horizontal
		}

		private LayoutOrientation _orientation;

		public LayoutOrientation Orientation
		{
			get => _orientation;
			set
			{
				if (_orientation != value)
				{
					_orientation = value;
					UpdateConstraintCreator(value);
				}
			}
		}

		public UILinearLayout(LayoutOrientation orientation = LayoutOrientation.Vertical)
			: base(GetConstraintCreator(orientation))
		{
			_orientation = orientation;
		}

		private static IConstraintCreator GetConstraintCreator(LayoutOrientation orientation)
		{
			switch (orientation)
			{
				case LayoutOrientation.Vertical:	return new VerticalConstraintCreator();
				case LayoutOrientation.Horizontal:	return new HorizontalConstraintCreator();
				default:
					throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null);
			}
		}

		private void UpdateConstraintCreator(LayoutOrientation orientation)
		{
			this.ConstraintCreator = GetConstraintCreator(orientation);
		}
	}
	
	public class NestedScrollView : UIScrollView
	{
		private const string SELECTOR_BOUNDS_SIZE = "bounds";
		private const string SELECTOR_CONTENT_SIZE = "contentSize";
		private readonly List<UIView> _subviewsInLayoutOrder = new List<UIView>(4);
		private readonly List<StickyViewData> _stickyViewList = new List<StickyViewData>();

		private readonly UILinearLayout _contentView;

		public NestedScrollView()
		{
			AlwaysBounceHorizontal = false;
			AlwaysBounceVertical = true;
			Bounces = true;
			BouncesZoom = false;
			ShowsVerticalScrollIndicator = true;
			ShowsHorizontalScrollIndicator = false;
			ContentInset = UIEdgeInsets.Zero;

			_contentView = new UILinearLayout();
			base.AddSubview(_contentView);

			AutoresizingMask = UIViewAutoresizing.None;
			AutosizesSubviews = false;
			TranslatesAutoresizingMaskIntoConstraints = false;
			_contentView.AutoresizingMask = UIViewAutoresizing.None;
			_contentView.AutosizesSubviews = false;
			_contentView.TranslatesAutoresizingMaskIntoConstraints = false;

			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
			{
				ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
			}

			AddConstraints(new[]
			{
				NSLayoutConstraint.Create(this, NSLayoutAttribute.Left,    NSLayoutRelation.Equal, _contentView, NSLayoutAttribute.Left, 1f, 0),
				NSLayoutConstraint.Create(this, NSLayoutAttribute.Right,   NSLayoutRelation.Equal, _contentView, NSLayoutAttribute.Right, 1f, 0),
				NSLayoutConstraint.Create(this, NSLayoutAttribute.Top,     NSLayoutRelation.Equal, _contentView, NSLayoutAttribute.Top, 1f, 0),
				NSLayoutConstraint.Create(this, NSLayoutAttribute.Bottom,  NSLayoutRelation.Equal, _contentView, NSLayoutAttribute.Bottom, 1f, 0),
				NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, _contentView, NSLayoutAttribute.CenterX, 1f, 0),
			});
		}

		public override void AddSubview(UIView view)
		{
			if (view is UIScrollView scrollView)
			{
				scrollView.AddObserver(this, SELECTOR_CONTENT_SIZE, NSKeyValueObservingOptions.Old, Handle);
				view = new ScrollItemWrapper(scrollView, this);
			}
			else if (IsAddedByOsScrollbar(view))
			{
				base.AddSubview(view);
				return;
			}
			else if(view is UIRefreshControl)
			{
				base.AddSubview(view);
				return;
			}
			else
			{
				UIScrollView scrollViewChild = view.Subviews.OfType<UIScrollView>().FirstOrDefault();
				if(scrollViewChild != null)
				{
					scrollViewChild.AddObserver(this, SELECTOR_CONTENT_SIZE, NSKeyValueObservingOptions.Old, Handle);
					view = new ScrollItemWrapper(view, scrollViewChild, this);
				}
			}

			_subviewsInLayoutOrder.Add(view);
			_contentView.AddSubview(view);
			
			view.AddObserver(this, SELECTOR_BOUNDS_SIZE, NSKeyValueObservingOptions.Old, Handle);
		}

		private bool IsAddedByOsScrollbar(UIView view) //TODO: review scrollbar detection
		{
			if (view is UIImageView)
			{
				return view.Frame.Location == CGPoint.Empty
				&& (view.Frame.Width <= 2.5f)
				&& (view.Frame.Height <= 2.5f);
			}
			else
			{
				return false;
			}
		}

		public void AddStickableView(UIView stickableView)
		{
			UIView placeholderView = new UIView() { BackgroundColor = UIColor.Clear };
			
			base.AddSubview(stickableView);
			AddSubview(placeholderView);

			stickableView.TranslatesAutoresizingMaskIntoConstraints = false;
			var stickedViewYPosition = NSLayoutConstraint.Create(stickableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 0f);
			var unstickedViewYPosition = NSLayoutConstraint.Create(stickableView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, placeholderView, NSLayoutAttribute.CenterY, 1f, 0f);
			AddConstraints(new[]
			{
				NSLayoutConstraint.Create(stickableView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, placeholderView, NSLayoutAttribute.Height, 1f, 0),
				NSLayoutConstraint.Create(this, NSLayoutAttribute.Width,  NSLayoutRelation.Equal, stickableView, NSLayoutAttribute.Width, 1f, 0),
				NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX,NSLayoutRelation.Equal, stickableView, NSLayoutAttribute.CenterX, 1f, 0)
			});
			var stickyViewData = new StickyViewData()
			{
				StickableView = stickableView,
				LinearLayoutPlaceholder = placeholderView,
				StickedViewYPosition = stickedViewYPosition,
				UnstickedViewYPosition = unstickedViewYPosition
			};
			stickyViewData.SetSticked(true);
			_stickyViewList.Add(stickyViewData);
		}

		public void SetSticky(UIView view, bool sticky)
		{
			_stickyViewList.Single(v => v.StickableView == view).SetSticked(sticky);
		}

		public virtual NestedScrollView WithSubviews(params (UIView view, bool stickable)[] views)
		{
			foreach (var v in views)
			{
				if (v.stickable)
				{
					this.AddStickableView(v.view);
				}
				else
				{
					this.AddSubview(v.view);
				}
			}
			return this;
		}

		public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
		{
			if (context != Handle)
			{
				base.ObserveValue(keyPath, ofObject, change, context);
				return;
			}

			if (keyPath == SELECTOR_BOUNDS_SIZE || keyPath == SELECTOR_CONTENT_SIZE)
			{
				SetNeedsLayout();
				LayoutIfNeeded();
			}
			else
			{
				throw new ArgumentOutOfRangeException(nameof(keyPath), keyPath, "Unsupported KVO notification keypath");
			}
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			nfloat yMin = ContentOffset.Y;
			nfloat visibleHeight = Frame.Height;
			bool hasChanged = false;

			for (int i = 0; i < _subviewsInLayoutOrder.Count; i++)
			{
				if (_subviewsInLayoutOrder[i] is ScrollItemWrapper subview)
				{
					nfloat top = subview.Frame.Top;

					nfloat yTop = yMin - top;
					nfloat yBot = visibleHeight + yTop;

					hasChanged |= subview.SetVisibleArea(yTop, yBot);
				}
			}

			if (_stickyViewList.Count > 0)
			{
				var scrollViewYOffset = ContentOffset.Y;
				foreach (var wrapper in _stickyViewList)
				{
					var stickyViewYOffset = Max(wrapper.LinearLayoutPlaceholder.Frame.Y, scrollViewYOffset);
					if (hasChanged |= wrapper.StickedViewYPosition.Constant != stickyViewYOffset)
					{
						wrapper.StickedViewYPosition.Constant = stickyViewYOffset;
					}
				}
			}

			if (hasChanged)
			{
				_contentView.SetNeedsLayout();
				_contentView.LayoutIfNeeded();
			}
		}

		private static nfloat Max(nfloat val1, nfloat val2) => val1 > val2 ? val1 : val2;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (UIView subview in _subviewsInLayoutOrder.ToArray())
				{
					subview.RemoveObserver(this, SELECTOR_BOUNDS_SIZE, Handle);
					subview.RemoveFromSuperview();
					subview.Dispose();
				}

				_contentView.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Nested Views

		private class ScrollItemWrapper : UIView
		{
			private const float MINIMAL_RENDER_HEIGHT = 20;

			private UIView _child;
			private UIScrollView _innerView;
			private List<UIView> _boundsRegisteredViews;
			private NestedScrollView _observer;
			private NSLayoutConstraint _containerHeightConstraint;
			private NSLayoutConstraint _innerTopConstraint;
			private NSLayoutConstraint _innerHeightConstraint;
			private NSLayoutConstraint _innerChildTopConstraint;
			private NSLayoutConstraint _innerChildHeightConstraint;

			public ScrollItemWrapper(UIView child, UIScrollView innerView, NestedScrollView observer)
			{
				_child = child;
				_innerView = innerView;
				_observer = observer;
				AddSubview(_child);
				_boundsRegisteredViews = child.Subviews.Where(x => !(x is UIScrollView)).ToList();
				_boundsRegisteredViews.Add(child);

				_innerView.ScrollEnabled = false;
				_innerView.TranslatesAutoresizingMaskIntoConstraints = false;
				_child.TranslatesAutoresizingMaskIntoConstraints = false;

				_containerHeightConstraint = NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1f, GetContentHeight());
				AddConstraint(_containerHeightConstraint);

				_innerView.AddObserver(this, SELECTOR_CONTENT_SIZE, NSKeyValueObservingOptions.Old, Handle);

				foreach(UIView v in _boundsRegisteredViews)
				{
					v.AddObserver(this, SELECTOR_BOUNDS_SIZE, NSKeyValueObservingOptions.Old, Handle);
				}

				NSLayoutConstraint[] innerConstraints =
				{
					NSLayoutConstraint.Create(this, NSLayoutAttribute.Width,   NSLayoutRelation.Equal, innerView, NSLayoutAttribute.Width, 1f, 0),
					NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, innerView, NSLayoutAttribute.CenterX, 1f, 0),
					_innerTopConstraint = NSLayoutConstraint.Create(innerView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 0),
					_innerHeightConstraint = NSLayoutConstraint.Create(innerView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1f, 10),
					NSLayoutConstraint.Create(this, NSLayoutAttribute.Width,   NSLayoutRelation.Equal, child, NSLayoutAttribute.Width, 1f, 0),
					NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, child, NSLayoutAttribute.CenterX, 1f, 0),
					_innerChildTopConstraint = NSLayoutConstraint.Create(child, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 0),
					_innerChildHeightConstraint = NSLayoutConstraint.Create(child, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1f, 10)
				};

				NSLayoutConstraint[] constraintsToRemove = child.Constraints.Where(x => x.FirstItem == innerView || x.SecondItem == innerView).ToArray();
				child.RemoveConstraints(constraintsToRemove);

				AddConstraints(innerConstraints);
			}

			public ScrollItemWrapper(UIScrollView innerView, NestedScrollView observer)
			{
				_innerView = innerView;
				_observer = observer;
				AddSubview(_innerView);

				_innerView.ScrollEnabled = false;
				_innerView.TranslatesAutoresizingMaskIntoConstraints = false;

				_containerHeightConstraint = NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1f, GetContentHeight());
				AddConstraint(_containerHeightConstraint);

				_innerView.AddObserver(this, SELECTOR_CONTENT_SIZE, NSKeyValueObservingOptions.Old, Handle);

				NSLayoutConstraint[] innerConstraints =
				{
					NSLayoutConstraint.Create(this, NSLayoutAttribute.Width,   NSLayoutRelation.Equal, innerView, NSLayoutAttribute.Width, 1f, 0),
					NSLayoutConstraint.Create(this, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, innerView, NSLayoutAttribute.CenterX, 1f, 0),
					_innerTopConstraint = NSLayoutConstraint.Create(innerView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1f, 0),
					_innerHeightConstraint = NSLayoutConstraint.Create(innerView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1f, 10),
				};

				AddConstraints(innerConstraints);
			}

			private nfloat GetContentHeight()
			{
				nfloat result = _innerView.ContentSize.Height;
				if(_boundsRegisteredViews != null)
				{
					foreach(UIView v in _boundsRegisteredViews)
					{
						nfloat height = v.Bounds.Height;
						if(result < height)
						{
							result = height;
						}
					}
				}
				return result;
			}

			public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
			{
				if (context != Handle)
				{
					base.ObserveValue(keyPath, ofObject, change, context);
					return;
				}

				if (keyPath == SELECTOR_CONTENT_SIZE || keyPath == SELECTOR_BOUNDS_SIZE)
				{
					nfloat height = GetContentHeight();

					if (_containerHeightConstraint.Constant != height)
					{
						_containerHeightConstraint.Constant = height;
						Superview?.SetNeedsLayout();
						Superview?.LayoutIfNeeded();
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(keyPath), keyPath, "Unsupported KVO notification keypath");
				}
			}

			public bool SetVisibleArea(nfloat top, nfloat bottom)
			{
				if (top < 0)
				{
					top = 0;
				}

				nfloat contentHeight = GetContentHeight();
				if (bottom > contentHeight)
				{
					bottom = contentHeight;
				}

				nfloat height = bottom - top;
				if (contentHeight > MINIMAL_RENDER_HEIGHT && height < MINIMAL_RENDER_HEIGHT) //arbitrary value to render at least one cell
				{
					height = MINIMAL_RENDER_HEIGHT;
					nfloat maxTop = contentHeight - height;
					if (top + height > maxTop)
					{
						top = maxTop;
					}
				}

				bool constraintsChanged = false;
				if (_innerTopConstraint.Constant != top)
				{
					constraintsChanged = true;
					_innerTopConstraint.Constant = top;
					if(_innerChildTopConstraint != null)
					{
						_innerChildTopConstraint.Constant = top;
					}
				}

				if (_innerHeightConstraint.Constant != height)
				{
					constraintsChanged = true;
					_innerHeightConstraint.Constant = height;

					if (_innerChildHeightConstraint != null)
					{
						_innerChildHeightConstraint.Constant = height;
					}
				}

				if (_innerView.ContentOffset.Y != top)
				{
					_innerView.SetContentOffset(new CGPoint(0, top), false);
				}

				return constraintsChanged;
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					_innerView.RemoveObserver(_observer, SELECTOR_CONTENT_SIZE, _observer.Handle);
					_observer = null;
					
					_innerView.RemoveObserver(this, SELECTOR_CONTENT_SIZE, Handle);
					_innerView.Dispose();
					_innerView = null;

					if(_boundsRegisteredViews != null)
					{
						foreach(UIView v in _boundsRegisteredViews)
						{
							v.RemoveObserver(this, SELECTOR_BOUNDS_SIZE, Handle);
						}
					}

					_boundsRegisteredViews?.Clear();
					_boundsRegisteredViews = null;

					_containerHeightConstraint.Dispose();
					_innerTopConstraint.Dispose();
					_innerHeightConstraint.Dispose();

					_innerChildTopConstraint?.Dispose();
					_innerChildHeightConstraint?.Dispose();

					_child = null;
					_containerHeightConstraint = null;
					_innerTopConstraint = null;
					_innerHeightConstraint = null;
					_innerChildTopConstraint = null;
					_innerChildHeightConstraint = null;
				}
				base.Dispose(disposing);
			}
		}

		private class StickyViewData
		{
			public UIView StickableView { get; set; }
			public UIView LinearLayoutPlaceholder { get; set; }
			public NSLayoutConstraint StickedViewYPosition { get; set; }
			public NSLayoutConstraint UnstickedViewYPosition { get; set; }
			public void SetSticked(bool isSticked)
			{
				if (isSticked)
				{
					UnstickedViewYPosition.Active = false;
					StickedViewYPosition.Active = true;
				}
				else
				{
					StickedViewYPosition.Active = false;
					UnstickedViewYPosition.Active = true;
				}
			}
		}

		#endregion Nested Views
	}

	public class PaddingContainer : UIView
	{
		public PaddingContainer(UIView content, int top, int left, int bottom, int right)
		{
			Add(content);

			this.AnchorTop(content, top)
				.AnchorBottom(content, bottom)
				.AnchorLeft(content, left)
				.AnchorRight(content, right);
		}
		
		public PaddingContainer(UIView content, int vertical, int horizontal) : this(content, vertical, horizontal, vertical, horizontal) { }

		public PaddingContainer(UIView content, int allDirections) : this(content, allDirections, allDirections) { }
	}

	public static class PaddingContainerExtensions
	{
		public static UIView WrapWithPadding(this UIView content, int top, int left, int bottom, int right) => new PaddingContainer(content, top, left, bottom, right);
		public static UIView WrapWithPadding(this UIView content, int vertical, int horizontal) => new PaddingContainer(content, vertical, horizontal);
		public static UIView WrapWithPadding(this UIView content, int allDirections) => new PaddingContainer(content, allDirections);
	}
}