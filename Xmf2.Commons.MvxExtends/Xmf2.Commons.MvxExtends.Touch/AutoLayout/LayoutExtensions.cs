using System;
using System.Runtime.CompilerServices;
using UIKit;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

public static class CustomAutoLayoutExtensions
{
	#region old

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
	public static UIView CenterAndFillWidth(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(view, CenterX, Equal, containerView, CenterX, 1f, 0f)
			.WithConstraint(view, Width, Equal, containerView, Width, 1f, -margin);
	}

	public static void VerticalFlow(this UIView containerView, params UIView[] views)
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

		for (int i = 1 ; i < views.Length ; ++i)
		{
			containerView.VerticalSpace(views[i - 1], views[i]);
		}
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

		for (int i = 1 ; i < views.Length ; ++i)
		{
			UIView v1 = views[i - 1];
			UIView v2 = views[i];

			containerView.ConstrainLayout(() => v1.Width() == v2.Width());
		}

		return containerView;
	}

	public static UIView AnchorTop(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => view.Top() == containerView.Top() + margin);
		return containerView;
	}

	public static UIView AnchorBottom(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => containerView.Bottom() == view.Bottom() + margin);
		return containerView;
	}

	public static UIView AnchorRight(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => containerView.Right() == view.Right() + margin);
		return containerView;
	}

	public static UIView AnchorLeft(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => view.Left() == containerView.Left() + margin);
		return containerView;
	}

	public static UIView CenterHorizontally(this UIView containerView, UIView view)
	{
		containerView.ConstrainLayout(() => view.CenterX() == containerView.CenterX());
		return containerView;
	}

	public static UIView CenterVertically(this UIView containerView, UIView view)
	{
		containerView.ConstrainLayout(() => view.CenterY() == containerView.CenterY());
		return containerView;
	}

	public static UIView FillWidth(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => containerView.Width() == view.Width() + margin);
		return containerView;
	}

	public static UIView FillHeight(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => containerView.Height() == view.Height() + margin);
		return containerView;
	}

	public static UIView VerticalSpace(this UIView containerView, UIView top, UIView bottom, int margin = 0)
	{
		containerView.ConstrainLayout(() => bottom.Top() == top.Bottom() + margin);
		return containerView;
	}

	public static UIView HorizontalSpace(this UIView containerView, UIView left, UIView right, int margin = 0)
	{
		containerView.ConstrainLayout(() => right.Left() == left.Right() + margin);
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView MinHorizontalSpace(this UIView containerView, UIView left, UIView right, float margin = 0f)
	{
		containerView.WithConstraint(right, Left, GreaterThanOrEqual, left, Right, 1f, margin);
		return containerView;
	}

	public static UIView ConstrainHeight(this UIView view, int height)
	{
		view.ConstrainLayout(() => view.Height() == height);
		return view;
	}

	public static UIView ConstrainWidth(this UIView view, int width)
	{
		view.ConstrainLayout(() => view.Width() == width);
		return view;
	}

	public static UIView ConstrainMinHeight(this UIView view, int height)
	{
		view.ConstrainLayout(() => view.Height() >= height);
		return view;
	}

	public static UIView ConstrainMinWidth(this UIView view, int width)
	{
		view.ConstrainLayout(() => view.Width() >= width);
		return view;
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

	public static UIView AlignOnLeft(this UIView view, UIView v1, UIView v2, int offset = 0)
	{
		view.ConstrainLayout(() => v1.Left() == v2.Left() + offset);
		return view;
	}

	public static UIView AlignOnRight(this UIView view, UIView v1, UIView v2, int offset = 0)
	{
		view.ConstrainLayout(() => v1.Right() == v2.Right() + offset);
		return view;
	}

	public static UIView AlignOnTop(this UIView view, UIView v1, UIView v2, int offset = 0)
	{
		view.ConstrainLayout(() => v1.Top() == v2.Top() + offset);
		return view;
	}

	public static UIView AlignOnBottom(this UIView view, UIView v1, UIView v2, int offset = 0)
	{
		view.ConstrainLayout(() => v1.Bottom() == v2.Bottom() + offset);
		return view;
	}

	public static UIView AlignOnCenterX(this UIView view, UIView v1, UIView v2, int offset = 0)
	{
		view.ConstrainLayout(() => v1.CenterX() == v2.CenterX() + offset);
		return view;
	}

	public static UIView AlignOnCenterY(this UIView view, UIView v1, UIView v2, int offset = 0)
	{
		view.ConstrainLayout(() => v1.CenterY() == v2.CenterY() + offset);
		return view;
	}

	public static UIView Same(this UIView view, UIView reference, UIView dest)
	{
		view.ConstrainLayout(() => reference.CenterY() == dest.CenterY()
		                           && reference.CenterX() == dest.CenterX()
		                           && reference.Height() == dest.Height()
		                           && reference.Width() == dest.Width());
		return view;
	}

	public static UIView SameWidth(this UIView view, UIView v1, UIView v2)
	{
		view.ConstrainLayout(() => v1.Width() == v2.Width());
		return view;
	}

	public static UIView SameHeight(this UIView view, UIView v1, UIView v2)
	{
		view.ConstrainLayout(() => v1.Height() == v2.Height());
		return view;
	}

	public static UIScrollView VerticalScrollContentConstraint(this UIScrollView scroll, UIView content)
	{
		scroll.AnchorLeft(content)
			.AnchorRight(content)
			.AnchorTop(content)
			.AnchorBottom(content)
			.AlignOnCenterX(scroll, content);

		return scroll;
	}

	public static UIView NameConstraint(this UIView view, string name)
	{
#if DEBUG
		view.ConstrainLayout(() => view.Name() == name);
#endif

		return view;
	}

	#endregion

	#region new

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromRight(this UIView constrainedView, UIView view, float margin = 0f)
	{
		return constrainedView.WithConstraint(constrainedView, Right, GreaterThanOrEqual, view, Right, 1f, margin, nameof(IncloseFromRight));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromLeft(this UIView containerView, UIView view, float margin = 0f)
	{
		return containerView.WithConstraint(containerView, Left, LessThanOrEqual, view, Left, 1f, -margin, nameof(IncloseFromLeft));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithRatio(this UIView view, float referenceWidth, float referenceHeight)
	{
		return view.WithRatio(view, referenceWidth, referenceHeight);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithRatio(this UIView constrainedView, UIView view, float referenceWidth, float referenceHeight)
	{
		return constrainedView.WithRatio(view, referenceWidth / referenceHeight);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithRatio(this UIView constrainedView, UIView view, float widthOnHeightRatio)
	{
		return constrainedView.WithConstraint(view, Width, Equal, view, Height, widthOnHeightRatio, 0f, nameof(WithRatio));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndLimitWidth(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(view, CenterX, Equal, containerView, CenterX, 1f, 0f, $"{nameof(CenterAndLimitWidth)}-CenterX")
			.WithConstraint(view, Width, LessThanOrEqual, containerView, Width, 1f, -margin, $"{nameof(CenterAndLimitWidth)}-Width");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView MinVerticalSpace(this UIView containerView, UIView top, UIView bottom, float margin = 0)
	{
		return containerView.WithConstraint(bottom, Top, GreaterThanOrEqual, top, Bottom, 1f, margin, nameof(MinVerticalSpace));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainSize(this UIView view, float width, float height)
	{
		return view.WithConstraint(view, Width, Equal, 1f, width, $"{nameof(ConstrainSize)}-Width")
			.WithConstraint(view, Height, Equal, 1f, height, $"{nameof(ConstrainSize)}-Height");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithConstraint(this UIView constrainedView, UIView view, NSLayoutAttribute attribute, NSLayoutRelation relation, nfloat multiplier, nfloat constant, string identifier = null)
	{
		if (view != null && view != constrainedView)
		{
			view.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		var constraint = NSLayoutConstraint.Create(view, attribute, relation, multiplier, constant);
		if (identifier != null)
		{
			constraint.SetIdentifier(identifier);
		}

		constrainedView.AddConstraint(constraint);
		return constrainedView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithConstraint(this UIView constrainedView, UIView view1, NSLayoutAttribute attribute1, NSLayoutRelation relation, UIView view2, NSLayoutAttribute attribute2, nfloat multiplier, nfloat constant, string identifier = null)
	{
		if (view1 != null && view1 != constrainedView)
		{
			view1.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		if (view2 != null && view2 != constrainedView)
		{
			view2.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		var constraint = NSLayoutConstraint.Create(view1, attribute1, relation, view2, attribute2, multiplier, constant);
		if (identifier != null)
		{
			constraint.SetIdentifier(identifier);
		}

		constrainedView.AddConstraint(constraint);
		return constrainedView;
	}

	#endregion
}