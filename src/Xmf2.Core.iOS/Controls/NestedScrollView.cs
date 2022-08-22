using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using Xmf2.Core.iOS.Extensions;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Core.iOS.Controls
{
	public class NestedScrollView : UIScrollView
	{
		private const string SELECTOR_BOUNDS_SIZE = "bounds";
		private const string SELECTOR_CONTENT_SIZE = "contentSize";
		private readonly List<UIView> _subviewsInLayoutOrder = new(4);
		private readonly List<StickyViewData> _stickyViewList = new();

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
				NSLayoutConstraint.Create(this, Left, Equal, _contentView, Left, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, Right, Equal, _contentView, Right, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, Top, Equal, _contentView, Top, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, Bottom, Equal, _contentView, Bottom, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, CenterX, Equal, _contentView, CenterX, 1f, 0).WithAutomaticIdentifier()
			});
		}

		public override void AddSubview(UIView view)
		{
			const string IOS_13_SCROLLVIEW_CLASS_NAME = "_UIScrollViewScrollIndicator";
			if (view is UIScrollView scrollView)
			{
				scrollView.AddObserver(this, SELECTOR_CONTENT_SIZE, NSKeyValueObservingOptions.Old, Handle);
				view = new ScrollItemWrapper(scrollView, this);
			}
			else if (view?.Class?.Name == IOS_13_SCROLLVIEW_CLASS_NAME || (view is UIImageView && view.Frame.Equals(new CGRect(0, 0, 2.5f, 2.5f))))
			{
				//vju 12/11/2019 in ios 13 scroll indicator have a private class
				//before it's an imageview with constant frame
				base.AddSubview(view);
				return;
			}
			else if (view is UIRefreshControl)
			{
				base.AddSubview(view);
				return;
			}
			else
			{
				UIScrollView scrollViewChild = view.Subviews.OfType<UIScrollView>().FirstOrDefault();
				if (scrollViewChild != null)
				{
					scrollViewChild.AddObserver(this, SELECTOR_CONTENT_SIZE, NSKeyValueObservingOptions.Old, Handle);
					view = new ScrollItemWrapper(view, scrollViewChild, this);
				}
			}

			_subviewsInLayoutOrder.Add(view);
			_contentView.AddSubview(view);

			view.AddObserver(this, SELECTOR_BOUNDS_SIZE, NSKeyValueObservingOptions.Old, Handle);
		}

		public void AddStickableView(UIView stickableView)
		{
			UIView placeholderView = new UIView()
			{
				BackgroundColor = UIColor.Clear
			};

			base.AddSubview(stickableView);
			AddSubview(placeholderView);

			stickableView.TranslatesAutoresizingMaskIntoConstraints = false;
			NSLayoutConstraint stickedViewYPosition = NSLayoutConstraint.Create(stickableView, Top, Equal, this, Top, 1f, 0f).WithAutomaticIdentifier();
			NSLayoutConstraint unstickedViewYPosition = NSLayoutConstraint.Create(stickableView, CenterY, Equal, placeholderView, CenterY, 1f, 0f).WithAutomaticIdentifier();
			AddConstraints(new[]
			{
				NSLayoutConstraint.Create(stickableView, Height, Equal, placeholderView, Height, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, Width, Equal, stickableView, Width, 1f, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, CenterX, Equal, stickableView, CenterX, 1f, 0).WithAutomaticIdentifier()
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
			foreach ((UIView view, bool stickable) in views)
			{
				if (stickable)
				{
					AddStickableView(view);
				}
				else
				{
					AddSubview(view);
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

			for (int i = 0 ; i < _subviewsInLayoutOrder.Count ; i++)
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
				nfloat scrollViewYOffset = ContentOffset.Y;
				foreach (StickyViewData wrapper in _stickyViewList)
				{
					nfloat stickyViewYOffset = Max(wrapper.LinearLayoutPlaceholder.Frame.Y, scrollViewYOffset);
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

				_containerHeightConstraint = NSLayoutConstraint.Create(this, Height, Equal, 1f, GetContentHeight()).WithAutomaticIdentifier();
				AddConstraint(_containerHeightConstraint);

				_innerView.AddObserver(this, SELECTOR_CONTENT_SIZE, NSKeyValueObservingOptions.Old, Handle);

				foreach (UIView v in _boundsRegisteredViews)
				{
					v.AddObserver(this, SELECTOR_BOUNDS_SIZE, NSKeyValueObservingOptions.Old, Handle);
				}

				NSLayoutConstraint[] innerConstraints =
				{
					NSLayoutConstraint.Create(this, Width, Equal, innerView, Width, 1f, 0).WithAutomaticIdentifier(),
					NSLayoutConstraint.Create(this, CenterX, Equal, innerView, CenterX, 1f, 0).WithAutomaticIdentifier(),
					_innerTopConstraint = NSLayoutConstraint.Create(innerView, Top, Equal, this, Top, 1f, 0).WithAutomaticIdentifier(),
					_innerHeightConstraint = NSLayoutConstraint.Create(innerView, Height, Equal, 1f, 10).WithAutomaticIdentifier(),
					NSLayoutConstraint.Create(this, Width, Equal, child, Width, 1f, 0).WithAutomaticIdentifier(),
					NSLayoutConstraint.Create(this, CenterX, Equal, child, CenterX, 1f, 0).WithAutomaticIdentifier(),
					_innerChildTopConstraint = NSLayoutConstraint.Create(child, Top, Equal, this, Top, 1f, 0).WithAutomaticIdentifier(),
					_innerChildHeightConstraint = NSLayoutConstraint.Create(child, Height, Equal, 1f, 10).WithAutomaticIdentifier()
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

				_containerHeightConstraint = NSLayoutConstraint.Create(this, Height, Equal, 1f, GetContentHeight()).WithAutomaticIdentifier();
				AddConstraint(_containerHeightConstraint);

				_innerView.AddObserver(this, SELECTOR_CONTENT_SIZE, NSKeyValueObservingOptions.Old, Handle);

				NSLayoutConstraint[] innerConstraints =
				{
					NSLayoutConstraint.Create(this, Width, Equal, innerView, Width, 1f, 0).WithAutomaticIdentifier(),
					NSLayoutConstraint.Create(this, CenterX, Equal, innerView, CenterX, 1f, 0).WithAutomaticIdentifier(),
					_innerTopConstraint = NSLayoutConstraint.Create(innerView, Top, Equal, this, Top, 1f, 0).WithAutomaticIdentifier(),
					_innerHeightConstraint = NSLayoutConstraint.Create(innerView, Height, Equal, 1f, 10).WithAutomaticIdentifier()
				};

				AddConstraints(innerConstraints);
			}

			private nfloat GetContentHeight()
			{
				nfloat result = _innerView.ContentSize.Height;
				if (_boundsRegisteredViews != null)
				{
					foreach (UIView v in _boundsRegisteredViews)
					{
						nfloat height = v.Bounds.Height;
						if (result < height)
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

				nfloat height = NMath.Max(bottom - top, 0f); //avoid negative height.
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
					if (_innerChildTopConstraint != null)
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

					if (_boundsRegisteredViews != null)
					{
						foreach (UIView v in _boundsRegisteredViews)
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
}