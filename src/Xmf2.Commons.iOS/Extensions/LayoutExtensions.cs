using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UIKit;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

public static class CustomAutoLayoutExtensions
{

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillWidth(this UIView containerView, params UIView[] views)
	{
		return CenterAndFillWidth(containerView, 0f, views);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillWidth(this UIView containerView, float margin, params UIView[] views)
	{
		if (views == null)
		{
			throw new ArgumentNullException(nameof(views));
		}
		foreach (UIView view in views)
		{
			containerView.CenterAndFillWidth(view, margin);
		}
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndLimitWidth(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(view, CenterX, Equal, containerView, CenterX, 1f, 0f)
							.WithConstraint(view, Width, LessThanOrEqual, containerView, Width, 1f, -margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndLimitWidth(this UIView containerView, params UIView[] views)
	{
		return CenterAndFillWidth(containerView, 0f, views);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndLimitWidth(this UIView containerView, float margin, params UIView[] views)
	{
		if (views == null)
		{
			throw new ArgumentNullException(nameof(views));
		}
		foreach (UIView view in views)
		{
			containerView.CenterAndLimitWidth(view, margin);
		}
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillWidth(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(view, CenterX, Equal, containerView, CenterX, 1f, 0f)
							.WithConstraint(view, Width, Equal, containerView, Width, 1f, -margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillHeight(this UIView containerView, params UIView[] views)
	{
		return CenterAndFillHeight(containerView, 0, views);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillHeight(this UIView containerView, float margin, params UIView[] views)
	{
		if (views == null)
		{
			throw new ArgumentNullException(nameof(views));
		}
		foreach (UIView view in views)
		{
			containerView.CenterAndFillHeight(view, margin);
		}
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillHeight(this UIView containerView, UIView view, float margin = 0f)
	{
		containerView.WithConstraint(view, CenterY, Equal, containerView, CenterY, 1f, 0f)
					 .WithConstraint(containerView, Height, Equal, view, Height, 1f, margin);
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndLimitHeight(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(view, CenterY, Equal, containerView, CenterY, 1f, 0f)
							.WithConstraint(view, Height, LessThanOrEqual, containerView, Height, 1f, -margin);
	}

	public static UIView VerticalFlow(this UIView containerView, params UIView[] views)
	{
		if (views == null)
		{
			throw new ArgumentNullException(nameof(views));
		}

		if (views.Length == 0)
		{
			throw new ArgumentException("views must contains at least one element", nameof(views));
		}

		containerView.AnchorTop(views[0]);
		containerView.AnchorBottom(views[views.Length - 1]);

		for (int i = 1; i < views.Length; ++i)
		{
			containerView.VerticalSpace(views[i - 1], views[i]);
		}
		return containerView;
	}

	public static UIView ViewsEqualWidth(this UIView containerView, params UIView[] views)
	{
		if (views == null)
		{
			throw new ArgumentNullException(nameof(views));
		}

		if (views.Length < 2)
		{
			throw new ArgumentException("views must contains at least two elements", nameof(views));
		}

		for (int i = 1; i < views.Length; ++i)
		{
			UIView v1 = views[i - 1];
			UIView v2 = views[i];

			containerView.WithConstraint(v1, Width, Equal, v2, Width, 1f, 0f);
		}

		return containerView;
	}

	public static UIView IncloseFromBottom(this UIView containerView, UIView view)
	{
		return IncloseFromBottom(containerView, view, 0f);
	}

	public static UIView IncloseFromBottom(this UIView containerView, UIView view, float margin)
	{
		return IncloseFromBottom(containerView, containerView, view, margin);
	}
	public static UIView IncloseFromBottom(this UIView constrainedView, UIView inclosingView, UIView view, float margin)
	{
		return constrainedView.WithConstraint(inclosingView, Bottom, GreaterThanOrEqual, view, Bottom, 1f, margin);
	}

	public static UIView IncloseFromTop(this UIView containerView, UIView view)
	{
		return IncloseFromTop(containerView, view, 0f);
	}

	public static UIView IncloseFromTop(this UIView containerView, UIView view, float margin)
	{
		return IncloseFromTop(containerView, containerView, view, margin);
	}

	public static UIView IncloseFromTop(this UIView constrainedView, UIView inclosingView, UIView view, float margin)
	{
		return constrainedView.WithConstraint(inclosingView, Top, LessThanOrEqual, view, Top, 1f, -margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromRight(this UIView containerView, UIView view)
	{
		return IncloseFromRight(containerView, view, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromRight(this UIView containerView, UIView view, float margin)
	{
		return IncloseFromRight(containerView, containerView, view, margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromRight(this UIView constrainedView, UIView inclosingView, UIView view, float margin)
	{
		return constrainedView.WithConstraint(inclosingView, Right, GreaterThanOrEqual, view, Right, 1f, margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromLeft(this UIView containerView, UIView view)
	{
		return IncloseFromLeft(containerView, view, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromLeft(this UIView containerView, UIView view, float margin)
	{
		return containerView.WithConstraint(containerView, Left, LessThanOrEqual, view, Left, 1f, -margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorTop(this UIView containerView, UIView view, float margin = 0f)
	{
		return containerView.WithConstraint(view, Top, Equal, containerView, Top, 1f, margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorBottom(this UIView containerView, UIView view, float margin = 0f)
	{
		return containerView.WithConstraint(containerView, Bottom, Equal, view, Bottom, 1f, margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorRight(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(containerView, Right, Equal, view, Right, 1f, margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorLeft(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(containerView, Left, Equal, view, Left, 1, -margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterHorizontally(this UIView containerView, UIView view)
	{
		return containerView.WithConstraint(containerView, CenterX, Equal, view, CenterX, 1f, 0f);
	}

	public static UIView CenterHorizontally(this UIView containerView, params UIView[] views)
	{
		return containerView.CenterHorizontally(0f, views);
	}
	public static UIView CenterHorizontally(this UIView containerView, float offset, params UIView[] views)
	{
		foreach (var view in views)
		{
			containerView.WithConstraint(view, CenterX, Equal, containerView, CenterX, 1f, offset);
		}
		return containerView;
	}

	public static UIView CenterVertically(this UIView containerView, UIView view)
	{
		containerView.WithConstraint(view, CenterY, Equal, containerView, CenterY, 1f, 0f);
		return containerView;
	}
	public static UIView CenterVertically(this UIView containerView, params UIView[] views)
	{
		return containerView.CenterVertically(0f, views);
	}
	public static UIView CenterVertically(this UIView containerView, float offset, params UIView[] views)
	{
		foreach (UIView view in views)
		{
			containerView.WithConstraint(view, CenterY, Equal, containerView, CenterY, 1f, offset);
		}
		return containerView;
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView FillWidth(this UIView containerView, UIView view, float margin = 0f)
	{
		return containerView.WithConstraint(containerView, Width, Equal, view, Width, 1, margin);
	}

	public static UIView FillHeight(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(containerView, Height, Equal, view, Height, 1f,  margin);
	}
	public static UIView FillHeight(this UIView containerView, params UIView[] views)
	{
		foreach (var view in views)
		{
			containerView.FillHeight(view, 0f);
		}
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView VerticalSpace(this UIView containerView, UIView topView, UIView bottomView, float margin = 0)
	{
		return containerView.WithConstraint(topView, Bottom, Equal, bottomView, Top, 1f, -margin);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView MinVerticalSpace(this UIView containerView, UIView top, UIView bottom, float margin = 0)
	{
		containerView.WithConstraint(bottom, Top, GreaterThanOrEqual, top, Bottom, 1f, margin);
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView HorizontalSpace(this UIView containerView, UIView leftView, UIView rightView, float margin = 0)
	{
		return containerView.WithConstraint(rightView, Left, Equal, leftView, Right, 1f, margin);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView MinHorizontalSpace(this UIView containerView, UIView left, UIView right, float margin = 0f)
	{
		containerView.WithConstraint(right, Left, GreaterThanOrEqual, left, Right, 1f, margin);
		return containerView;
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainHeight(this UIView view, float height)
	{
		view.AddConstraint(NSLayoutConstraint.Create(view, Height, Equal, 1, height));
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainWidth(this UIView view, float width)
	{
		view.AddConstraint(NSLayoutConstraint.Create(view, Width, Equal, 1, width));
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainSize(this UIView view, float width, float height)
	{
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		view.AddConstraint(NSLayoutConstraint.Create(view, Width,  Equal, 1, width));
		view.AddConstraint(NSLayoutConstraint.Create(view, Height, Equal, 1, height));
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainMinHeight(this UIView view, float height)
	{
		view.AddConstraint(NSLayoutConstraint.Create(view, Height, GreaterThanOrEqual, 1f, height));
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainMinWidth(this UIView view, float width)
	{
		view.AddConstraint(NSLayoutConstraint.Create(view, Width, GreaterThanOrEqual, 1f, width));
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		return view;
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainMinWidth(this UIView view, UIView v1)
	{
		return view.WithConstraint(view, Width, LessThanOrEqual, v1, Width, 1f, 0f);
	}

	public static UIView ConstrainMaxHeight(this UIView view, int height)
	{
		view.ConstrainLayout(() => view.Height() <= height);
		return view;
	}

	public static UIView ConstrainMaxWidth(this UIView view, int width)
	{
		view.ConstrainLayout(() => view.Width() <= width);
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnLeft(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		return view.WithConstraint(v1, Left, Equal, v2, Left, 1f,  offset);
	}

	public static UIView AlignOnRight(this UIView view, UIView v1, UIView v2, int offset = 0)
	{
		view.ConstrainLayout(() => v1.Right() == v2.Right() + offset);
		return view;
	}

	public static UIView AlignOnTop(this UIView view, UIView v1, UIView v2, float offset = 0f)
	{
		return view.WithConstraint(v1, Top, Equal, v2, Top, 1f, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnBottom(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		view.WithConstraint(v1, Bottom, Equal, v2, Bottom, 1f, offset);
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnCenterX(this UIView view, UIView v1, float offset = 0)
	{
		return view.AlignOnCenterX(view, v1, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnCenterX(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		return view.WithConstraint(v1, CenterX, Equal, v2, CenterX, 1f, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnCenterY(this UIView view, UIView v1, float offset = 0)
	{
		return view.AlignOnCenterY(view, v1, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnCenterY(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		return view.WithConstraint(v1, CenterY, Equal, v2, CenterY, 1f, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView Same(this UIView view, UIView childView)
	{
		return view.Same(view, childView);
	}

	public static UIView Same(this UIView view, UIView reference, UIView dest)
	{
		return view.WithConstraint(reference, CenterY,Equal, dest, CenterY, 1f, 0f)
				   .WithConstraint(reference, CenterX,Equal, dest, CenterX, 1f, 0f)
				   .WithConstraint(reference, Height ,Equal, dest, Height , 1f, 0f)
				   .WithConstraint(reference, Width  ,Equal, dest, Width  , 1f, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView SameWidth(this UIView view, UIView v1, UIView v2, float margin = 0f)
	{
		return view.WithConstraint(v1, Width, Equal, v2, Width, 1f, margin);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView SameHeight(this UIView view, UIView v1)
	{
		return view.SameHeight(view, v1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView SameHeight(this UIView view, UIView v1, UIView v2)
	{
		return view.WithConstraint(v1, Height, Equal, v2, Height, 1f, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView VerticalScrollContentConstraint(this UIScrollView scroll, UIView content)
	{
		return scroll.VerticalScrollContentConstraint(content, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView VerticalScrollContentConstraint(this UIScrollView scroll, UIView content, float horizontalMargin)
	{
		scroll.WithConstraint(scroll,  Left,	Equal, content, Left,	1, -horizontalMargin / 2f)
			  .WithConstraint(scroll,  Right,	Equal, content, Right,	1f, horizontalMargin / 2f)
			  .WithConstraint(scroll,  Top,		Equal, content,	Top,	1f, 0f)
			  .WithConstraint(scroll,  Bottom,	Equal, content, Bottom, 1f, 0f)
			  .WithConstraint(scroll,  CenterX, Equal, content, CenterX,1f, 0f);
		return scroll;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView VerticalScrollFilledContentConstraint(this UIScrollView scroll, UIView content, float horizontalMargin)
	{
		scroll.WithConstraint(scroll,  Left,	Equal,				content, Left,	 1, -horizontalMargin / 2f)
			  .WithConstraint(scroll,  Right,	Equal,				content, Right,	 1f, horizontalMargin / 2f)
			  .WithConstraint(scroll,  Top,		Equal,				content, Top,	 1f, 0f)
			  .WithConstraint(scroll,  Bottom,	Equal,				content, Bottom, 1f, 0f)
			  .WithConstraint(scroll,  Height,  LessThanOrEqual,	content, Height, 1f, 0f)
			  .WithConstraint(scroll,  CenterX, Equal,				content, CenterX,1f, 0f);
		return scroll;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView HorizontalScrollContentConstraint(this UIScrollView scroll, UIView content)
	{
		scroll.WithConstraint(scroll, Left,		Equal, content, Left,	 1f, 0f)
			  .WithConstraint(scroll, Right,	Equal, content, Right,	 1f, 0f)
			  .WithConstraint(scroll, Top,		Equal, content, Top,	 1f, 0f)
			  .WithConstraint(scroll, Bottom,	Equal, content, Bottom,  1f, 0f)
			  .WithConstraint(scroll, CenterY,	Equal, content, CenterY, 1f, 0f);
		return scroll;
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView HorizontalScrollFilledContentConstraint(this UIScrollView scroll, UIView content, float verticalMargin)
	{
		scroll.WithConstraint(scroll,  Left,	Equal,				content, Left,	 1f, 0f)
			  .WithConstraint(scroll,  Right,	Equal,				content, Right,	 1f, 0f)
			  .WithConstraint(scroll,  Top,		Equal,				content, Top,	 1f, -verticalMargin/ 2f)
			  .WithConstraint(scroll,  Bottom,	Equal,				content, Bottom, 1f, verticalMargin / 2f)
			  .WithConstraint(scroll,  Width,	LessThanOrEqual,	content, Width,  1f, 0f)
			  .WithConstraint(scroll,  CenterY, Equal,				content, CenterY,1f, 0f);
		return scroll;
	}

	public static UILabel ConstrainHeightForOneLiner(this UILabel label)
	{
		var textSize = new Foundation.NSString("lq").GetSizeUsingAttributes(new UIStringAttributes
		{
			Font = label.Font,
		});

		label.ConstrainHeight(((int)textSize.Height) + 1);

		return label;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithRatio(this UIView view, float referenceWidth, float referenceHeight)
	{
		return view.WithRatio(view, referenceWidth, referenceHeight);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithRatio(this UIView constrainedView, UIView view, float referenceWidth, float referenceHeight)
	{
		return constrainedView.WithConstraint(view, Width, Equal, view, Height, referenceWidth / referenceHeight, 0f);
	}

#if DEBUG
	public static UIView NameConstraint(this UIView view, string name)
	{
		view.ConstrainLayout(() => view.Name() == name);

		return view;
	}

#else
	public static UIView NameConstraint(this UIView view, string name)
	{
		return view;
	}
#endif

	public static UIView EnsureRemove(this UIView view, IEnumerable<NSLayoutConstraint> constraintsToRemove)
	{
		return EnsureRemove(view, constraintsToRemove?.ToArray());
	}
	public static UIView EnsureRemove(this UIView view, params NSLayoutConstraint[] constraintsToRemove)
	{
		if (constraintsToRemove == null)
		{
			return view;
		}
		var delta = view.Constraints.Intersect(constraintsToRemove).ToArray();
		view.RemoveConstraints(delta);
		return view;
	}
	public static UIView EnsureAdd(this UIView view, IEnumerable<NSLayoutConstraint> constraintsToAdd)
	{
		return EnsureAdd(view, constraintsToAdd?.ToArray());
	}
	public static UIView EnsureAdd(this UIView view, params NSLayoutConstraint[] constraintsToAdd)
	{
		if (constraintsToAdd == null)
		{
			return view;
		}
		var delta = constraintsToAdd.Except(view.Constraints).ToArray();
		view.AddConstraints(delta);
		return view;
	}
	public static UIView EnsureAdd(this UIView view, params UIGestureRecognizer[] gestureRecognizersToAdd)
	{
		if (gestureRecognizersToAdd == null)
		{
			return view;
		}
		var delta = gestureRecognizersToAdd.Except(view.GestureRecognizers ?? new UIGestureRecognizer[]{}).ToArray();
		foreach (var recognizer in delta)
		{
			view.AddGestureRecognizer(recognizer);
		}
		return view;
	}
	public static UIView EnsureRemove(this UIView view, params UIGestureRecognizer[] gestureRecognizersToRemove)
	{
		if (gestureRecognizersToRemove == null)
		{
			return view;
		}
		var delta = view.GestureRecognizers?.Intersect(gestureRecognizersToRemove).ToArray() ?? new UIGestureRecognizer[]{};
		foreach (var recognizer in delta)
		{
			view.RemoveGestureRecognizer(recognizer);
		}
		return view;
	}
	public static UIView EnsureRemove(this UIView view, params UIView[] subviewsToRemove)
	{
		if (subviewsToRemove == null)
		{
			return view;
		}
		var delta = view.Subviews.Intersect(subviewsToRemove).ToArray();
		foreach (var subView in delta)
		{
			subView.RemoveFromSuperview();
		}
		return view;
	}

	public static UIView EnsureAdd(this UIView view, params UIView[] subviewsToAdd)
	{
		if (subviewsToAdd == null)
		{
			return view;
		}
		var delta = subviewsToAdd.Except(view.Subviews).ToArray();
		view.AddSubviews(delta);
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithConstraint(this UIView constrainedView, UIView view, NSLayoutAttribute attribute, NSLayoutRelation relation, nfloat multiplier, nfloat constant)
	{
		if (view != null && view != constrainedView)
		{
			view.TranslatesAutoresizingMaskIntoConstraints = false;
		}
		constrainedView.AddConstraint(NSLayoutConstraint.Create(view, attribute, relation, multiplier, constant));
		return constrainedView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithConstraint(this UIView constrainedView, UIView view1, NSLayoutAttribute attribute1, NSLayoutRelation relation, UIView view2, NSLayoutAttribute attribute2, nfloat multiplier, nfloat constant)
	{
		if (view1 != null && view1 != constrainedView)
		{
			view1.TranslatesAutoresizingMaskIntoConstraints = false;
		}
		if (view2 != null && view2 != constrainedView)
		{
			view2.TranslatesAutoresizingMaskIntoConstraints = false;
		}
		constrainedView.AddConstraint(NSLayoutConstraint.Create(view1, attribute1, relation, view2, attribute2, multiplier, constant));
		return constrainedView;
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithContentCompressionResistancePriority(this UIView view, UILayoutPriority priority, UILayoutConstraintAxis axis)
	{
		view.SetContentCompressionResistancePriority((float)priority, axis);
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithContentHuggingPriority(this UIView view, UILayoutPriority priority, UILayoutConstraintAxis axis)
	{
		view.SetContentHuggingPriority((float)priority, axis);
		return view;
	}
}
