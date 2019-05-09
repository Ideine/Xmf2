using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UIKit;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

// ReSharper disable CheckNamespace
public static class CustomAutoLayoutExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static NSLayoutConstraint Enable(this NSLayoutConstraint constraint)
	{
		constraint.Active = true;
		return constraint;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static NSLayoutConstraint Disable(this NSLayoutConstraint constraint)
	{
		constraint.Active = false;
		return constraint;
	}

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
		return containerView.WithConstraint(view, CenterX, Equal, containerView, CenterX, 1f, 0f, identifier: $"{nameof(CenterAndLimitWidth)}-CenterX")
			.WithConstraint(view, Width, LessThanOrEqual, containerView, Width, 1f, -margin, identifier: $"{nameof(CenterAndLimitWidth)}-Width");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndLimitWidth(this UIView containerView, params UIView[] views)
	{
		return CenterAndLimitWidth(containerView, 0f, views);
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
	public static UIView CenterAndFillWidth(this UIView containerView, UIView view, float margin = 0, UILayoutPriority priority = UILayoutPriority.Required)
	{
		return containerView.WithConstraint(view, CenterX, Equal, containerView, CenterX, 1f, 0f, priority, $"{nameof(CenterAndFillWidth)}-CenterX")
							.WithConstraint(view, Width, Equal, containerView, Width, 1f, -margin, priority, $"{nameof(CenterAndFillWidth)}-Width");
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
	public static UIView CenterAndFillHeight(this UIView containerView, UIView view, float margin = 0f, UILayoutPriority priority = UILayoutPriority.Required)
	{
		containerView.WithConstraint(view, CenterY, Equal, containerView, CenterY, 1f, 0f, priority, $"{nameof(CenterAndFillHeight)}-CenterY")
					 .WithConstraint(containerView, Height, Equal, view, Height, 1f, margin, priority, $"{nameof(CenterAndFillHeight)}-Height");
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndLimitHeight(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(view, CenterY, Equal, containerView, CenterY, 1f, 0f, identifier: $"{nameof(CenterAndLimitHeight)}-CenterY")
							.WithConstraint(view, Height, LessThanOrEqual, containerView, Height, 1f, -margin, identifier: $"{nameof(CenterAndLimitHeight)}-Height");
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

		for (int i = 1 ; i < views.Length ; ++i)
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

		for (int i = 1 ; i < views.Length ; ++i)
		{
			UIView v1 = views[i - 1];
			UIView v2 = views[i];

			containerView.WithConstraint(v1, Width, Equal, v2, Width, 1f, 0f, identifier: nameof(ViewsEqualWidth));
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
		return constrainedView.WithConstraint(inclosingView, Bottom, GreaterThanOrEqual, view, Bottom, 1f, margin, identifier: nameof(IncloseFromBottom));
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
		return constrainedView.WithConstraint(inclosingView, Top, LessThanOrEqual, view, Top, 1f, -margin, identifier: nameof(IncloseFromTop));
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
		return constrainedView.WithConstraint(inclosingView, Right, GreaterThanOrEqual, view, Right, 1f, margin, identifier: nameof(IncloseFromRight));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromLeft(this UIView containerView, UIView view)
	{
		return IncloseFromLeft(containerView, view, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView IncloseFromLeft(this UIView containerView, UIView view, float margin)
	{
		return containerView.WithConstraint(containerView, Left, LessThanOrEqual, view, Left, 1f, -margin, identifier: nameof(IncloseFromLeft));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorTop(this UIView containerView, UIView view, float margin = 0f) => AnchorTop(containerView, view, (nfloat)margin);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorTop(this UIView containerView, UIView view, nfloat margin)
	{
		return containerView.WithConstraint(view, Top, Equal, containerView, Top, 1f, margin, identifier: nameof(AnchorTop));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorBottom(this UIView containerView, params UIView[] views)
	{
		for (int i = 0; i < views.Length; i++)
		{
			containerView.AnchorBottom(views[i]);
		}
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorBottom(this UIView containerView, UIView view, float margin = 0f) => containerView.AnchorBottom(view, (nfloat)margin);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorBottom(this UIView containerView, UIView view, nfloat margin)
	{
		return containerView.WithConstraint(containerView, Bottom, Equal, view, Bottom, 1f, margin, identifier: nameof(AnchorBottom));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorBottomUpToSafeArea(this UIView containerView, UIView view, float margin = 0f) => containerView.AnchorBottomUpToSafeArea(view, (nfloat)margin);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorBottomUpToSafeArea(this UIView containerView, UIView view, nfloat margin)
	{
		return UIDevice.CurrentDevice.CheckSystemVersion(11, 0)
			? containerView.WithConstraint(view.BottomAnchor.ConstraintEqualTo(containerView.SafeAreaLayoutGuide.BottomAnchor, -margin).WithIdentifier(nameof(AnchorBottomUpToSafeArea)))
			: containerView.WithConstraint(containerView, Bottom, Equal, view, Bottom, 1f, margin, identifier: nameof(AnchorBottomUpToSafeArea));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorRight(this UIView containerView, UIView view, float margin = 0) => containerView.AnchorRight(view, (nfloat)margin);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorRight(this UIView containerView, UIView view, nfloat margin)
	{
		return containerView.WithConstraint(containerView, Right, Equal, view, Right, 1f, margin, identifier: nameof(AnchorRight));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorLeft(this UIView containerView, UIView view, float margin = 0) => containerView.AnchorLeft(view, (nfloat)margin);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorLeft(this UIView containerView, UIView view, nfloat margin)
	{
		return containerView.WithConstraint(containerView, Left, Equal, view, Left, 1, -margin, identifier: nameof(AnchorLeft));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterHorizontally(this UIView containerView, UIView view)
	{
		return containerView.WithConstraint(containerView, CenterX, Equal, view, CenterX, 1f, 0f, identifier: nameof(CenterHorizontally));
	}

	public static UIView CenterHorizontally(this UIView containerView, params UIView[] views)
	{
		return containerView.CenterHorizontally(0f, views);
	}

	public static UIView CenterHorizontally(this UIView containerView, float offset, params UIView[] views)
	{
		foreach (var view in views)
		{
			containerView.WithConstraint(view, CenterX, Equal, containerView, CenterX, 1f, offset, identifier: nameof(CenterHorizontally));
		}

		return containerView;
	}

	public static UIView CenterVertically(this UIView containerView, UIView view)
	{
		containerView.WithConstraint(view, CenterY, Equal, containerView, CenterY, 1f, 0f, identifier: nameof(CenterVertically));
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
			containerView.WithConstraint(view, CenterY, Equal, containerView, CenterY, 1f, offset, identifier: nameof(CenterVertically));
		}

		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView FillWidth(this UIView containerView, UIView view, float margin = 0f)
	{
		return containerView.WithConstraint(containerView, Width, Equal, view, Width, 1, margin, identifier: nameof(FillWidth));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView FillHeight(this UIView containerView, UIView view, float margin = 0)
	{
		return containerView.WithConstraint(containerView, Height, Equal, view, Height, 1f, margin, identifier: nameof(FillHeight));
	}

	public static UIView FillHeight(this UIView containerView, params UIView[] views)
	{
		foreach (var view in views)
		{
			containerView.FillHeight(view);
		}

		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView VerticalSpace(this UIView containerView, UIView topView, UIView bottomView, float margin = 0)
	{
		return containerView.WithConstraint(topView, Bottom, Equal, bottomView, Top, 1f, -margin, identifier: nameof(VerticalSpace));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView MinVerticalSpace(this UIView containerView, UIView top, UIView bottom, float margin = 0)
	{
		return containerView.WithConstraint(bottom, Top, GreaterThanOrEqual, top, Bottom, 1f, margin, identifier: nameof(MinVerticalSpace));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView HorizontalSpace(this UIView containerView, UIView leftView, UIView rightView, float margin = 0)
	{
		return containerView.WithConstraint(rightView, Left, Equal, leftView, Right, 1f, margin, identifier: nameof(HorizontalSpace));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView MinHorizontalSpace(this UIView containerView, UIView left, UIView right, float margin = 0f)
	{
		return containerView.WithConstraint(right, Left, GreaterThanOrEqual, left, Right, 1f, margin, identifier: nameof(MinHorizontalSpace));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainHeight(this UIView view, float height)
	{
		return view.WithConstraint(view, Height, Equal, 1, height, nameof(ConstrainHeight));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainWidth(this UIView view, float width)
	{
		return view.WithConstraint(view, Width, Equal, 1, width, nameof(ConstrainWidth));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainSize(this UIView view, float width, float height)
	{
		return view.WithConstraint(view, Width, Equal, 1f, width, $"{nameof(ConstrainSize)}-Width")
				   .WithConstraint(view, Height, Equal, 1f, height, $"{nameof(ConstrainSize)}-Height");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainMinHeight(this UIView view, float height)
	{
		return view.WithConstraint(view, Height, GreaterThanOrEqual, 1f, height, nameof(ConstrainMinHeight));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainMinWidth(this UIView view, float width)
	{
		return view.WithConstraint(view, Width, GreaterThanOrEqual, 1f, width, nameof(ConstrainMinWidth));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainMinWidth(this UIView view, UIView v1)
	{
		return view.WithConstraint(view, Width, LessThanOrEqual, v1, Width, 1f, 0f, identifier: nameof(ConstrainMinWidth));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainMaxHeight(this UIView view, int height)
	{
		return view.WithConstraint(view, Height, LessThanOrEqual, 1f, height, nameof(ConstrainMaxHeight));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView ConstrainMaxWidth(this UIView view, float width)
	{
		return view.WithConstraint(view, Width, LessThanOrEqual, 1f, width, nameof(ConstrainMaxWidth));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnLeft(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		return view.WithConstraint(v1, Left, Equal, v2, Left, 1f, offset, identifier: nameof(AlignOnLeft));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnRight(this UIView view, UIView v1, UIView v2, int offset = 0)
	{
		return view.WithConstraint(v1, Right, Equal, v2, Right, 1f, offset, identifier: nameof(AlignOnRight));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnTop(this UIView view, UIView v1, UIView v2, float offset = 0f)
	{
		return view.WithConstraint(v1, Top, Equal, v2, Top, 1f, offset, identifier: nameof(AlignOnTop));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnBottom(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		return view.WithConstraint(v1, Bottom, Equal, v2, Bottom, 1f, offset, identifier: nameof(AlignOnBottom));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnCenterX(this UIView view, UIView v1, float offset = 0)
	{
		return view.AlignOnCenterX(view, v1, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnCenterX(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		return view.WithConstraint(v1, CenterX, Equal, v2, CenterX, 1f, offset, identifier: nameof(AlignOnCenterX));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnCenterY(this UIView view, UIView v1, float offset = 0)
	{
		return view.AlignOnCenterY(view, v1, offset);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnCenterY(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		return view.WithConstraint(v1, CenterY, Equal, v2, CenterY, 1f, offset, identifier: nameof(AlignOnCenterY));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AlignOnBaseLine(this UIView view, UIView v1, UIView v2, float offset = 0)
	{
		return view.WithConstraint(v1, Baseline, Equal, v2, Baseline, 1f, offset, identifier: nameof(AlignOnBaseLine));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView Same(this UIView view, UIView childView)
	{
		return view.Same(view, childView);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView Same(this UIView view, UIView reference, UIView dest)
	{
		return view.WithConstraint(reference, CenterY, Equal, dest, CenterY, 1f, 0f, identifier: $"{nameof(Same)}-CenterY")
			.WithConstraint(reference, CenterX, Equal, dest, CenterX, 1f, 0f, identifier: $"{nameof(Same)}-CenterX")
			.WithConstraint(reference, Height, Equal, dest, Height, 1f, 0f, identifier: $"{nameof(Same)}-Height")
			.WithConstraint(reference, Width, Equal, dest, Width, 1f, 0f, identifier: $"{nameof(Same)}-Width");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView SameWidth(this UIView view, UIView v1, UIView v2, float margin = 0f)
	{
		return view.WithConstraint(v1, Width, Equal, v2, Width, 1f, margin, identifier: nameof(SameWidth));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView SameHeight(this UIView view, UIView v1)
	{
		return view.SameHeight(view, v1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView SameHeight(this UIView view, UIView v1, UIView v2)
	{
		return view.WithConstraint(v1, Height, Equal, v2, Height, 1f, 0f, identifier: nameof(SameHeight));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView VerticalScrollContentConstraint(this UIScrollView scroll, UIView content)
	{
		return scroll.VerticalScrollContentConstraint(content, 0f);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView VerticalScrollContentConstraint(this UIScrollView scroll, UIView content, float horizontalMargin)
	{
		scroll.WithConstraint(scroll, Left		, Equal, content, Left		, 1, -horizontalMargin / 2f, identifier: $"{nameof(VerticalScrollContentConstraint)}-Left")
			  .WithConstraint(scroll, Right		, Equal, content, Right		, 1f, horizontalMargin / 2f, identifier: $"{nameof(VerticalScrollContentConstraint)}-Right")
			  .WithConstraint(scroll, Top		, Equal, content, Top		, 1f, 0f, identifier: $"{nameof(VerticalScrollContentConstraint)}-Top")
			  .WithConstraint(scroll, Bottom	, Equal, content, Bottom	, 1f, 0f, identifier: $"{nameof(VerticalScrollContentConstraint)}-Bottom")
			  .WithConstraint(scroll, CenterX	, Equal, content, CenterX	, 1f, 0f, identifier: $"{nameof(VerticalScrollContentConstraint)}-CenterX");
		return scroll;
	}

	[Obsolete("Use UIFilledScrollView instead")]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView VerticalScrollFilledContentConstraint(this UIScrollView scroll, UIView content, float horizontalMargin)
	{
		scroll	.WithConstraint(scroll, Left	, Equal			 	, content, Left,	1, -horizontalMargin / 2f, identifier: $"{nameof(VerticalScrollFilledContentConstraint)}-Left")
				.WithConstraint(scroll, Right	, Equal			 	, content, Right,	1f, horizontalMargin / 2f, identifier: $"{nameof(VerticalScrollFilledContentConstraint)}-Right")
				.WithConstraint(scroll, Top		, Equal				, content, Top,		1f, 0f, identifier: $"{nameof(VerticalScrollFilledContentConstraint)}-Top")
				.WithConstraint(scroll, Bottom	, Equal				, content, Bottom,	1f, 0f, identifier: $"{nameof(VerticalScrollFilledContentConstraint)}-Bottom")
				.WithConstraint(scroll, Height	, LessThanOrEqual	, content, Height,	1f, 0f, identifier: $"{nameof(VerticalScrollFilledContentConstraint)}-Height")
				.WithConstraint(scroll, CenterX	, Equal			 	, content, CenterX,	1f, 0f, identifier: $"{nameof(VerticalScrollFilledContentConstraint)}-CenterX");
		return scroll;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView HorizontalScrollContentConstraint(this UIScrollView scroll, UIView content)
	{
		scroll.WithConstraint(scroll, Left, Equal, content, Left, 1f, 0f, identifier: $"{nameof(HorizontalScrollContentConstraint)}-Left")
			.WithConstraint(scroll, Right, Equal, content, Right, 1f, 0f, identifier: $"{nameof(HorizontalScrollContentConstraint)}-Right")
			.WithConstraint(scroll, Top, Equal, content, Top, 1f, 0f, identifier: $"{nameof(HorizontalScrollContentConstraint)}-Top")
			.WithConstraint(scroll, Bottom, Equal, content, Bottom, 1f, 0f, identifier: $"{nameof(HorizontalScrollContentConstraint)}-Bottom")
			.WithConstraint(scroll, CenterY, Equal, content, CenterY, 1f, 0f, identifier: $"{nameof(HorizontalScrollContentConstraint)}-CenterY");
		return scroll;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView HorizontalScrollFilledContentConstraint(this UIScrollView scroll, UIView content, float verticalMargin)
	{
		scroll.WithConstraint(scroll, Left, Equal, content, Left, 1f, 0f, identifier: $"{nameof(HorizontalScrollFilledContentConstraint)}-Left")
			.WithConstraint(scroll, Right, Equal, content, Right, 1f, 0f, identifier: $"{nameof(HorizontalScrollFilledContentConstraint)}-Right")
			.WithConstraint(scroll, Top, Equal, content, Top, 1f, -verticalMargin / 2f, identifier: $"{nameof(HorizontalScrollFilledContentConstraint)}-Top")
			.WithConstraint(scroll, Bottom, Equal, content, Bottom, 1f, verticalMargin / 2f, identifier: $"{nameof(HorizontalScrollFilledContentConstraint)}-Bottom")
			.WithConstraint(scroll, Width, LessThanOrEqual, content, Width, 1f, 0f, identifier: $"{nameof(HorizontalScrollFilledContentConstraint)}-Width")
			.WithConstraint(scroll, CenterY, Equal, content, CenterY, 1f, 0f, identifier: $"{nameof(HorizontalScrollFilledContentConstraint)}-CenterY");
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
		return constrainedView.WithRatio(view, referenceWidth / referenceHeight);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView WithRatio(this UIView constrainedView, UIView view, float widthOnHeightRatio)
	{
		return constrainedView.WithConstraint(view, Width, Equal, view, Height, widthOnHeightRatio, 0f, identifier: nameof(WithRatio));
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

		var delta = gestureRecognizersToAdd.Except(view.GestureRecognizers ?? new UIGestureRecognizer[] { }).ToArray();
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

		var delta = view.GestureRecognizers?.Intersect(gestureRecognizersToRemove).ToArray() ?? new UIGestureRecognizer[] { };
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
	public static UIView WithConstraint(this UIView constrainedView, NSLayoutConstraint constraint)
	{
		if (constraint.FirstItem is UIView view1 && view1 != constrainedView)
		{
			view1.TranslatesAutoresizingMaskIntoConstraints = false;
		}
		if (constraint.SecondItem is UIView view2 && view2 != constrainedView)
		{
			view2.TranslatesAutoresizingMaskIntoConstraints = false;
		}
		constrainedView.AddConstraint(constraint);
		return constrainedView;
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
	public static UIView WithConstraint(this UIView constrainedView, UIView view1, NSLayoutAttribute attribute1, NSLayoutRelation relation, UIView view2, NSLayoutAttribute attribute2, nfloat multiplier, nfloat constant, UILayoutPriority uiLayoutPriority = UILayoutPriority.Required, string identifier = null)
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
		constraint.Priority = (float)uiLayoutPriority;
		if (identifier != null)
		{
			constraint.SetIdentifier(identifier);
		}
		constrainedView.AddConstraint(constraint);
		return constrainedView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void RemoveConstraints(this UIView container, params NSLayoutConstraint[] constraints) => container.RemoveConstraints(constraints);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void AddConstraints(this UIView container, params NSLayoutConstraint[] constraints) => container.AddConstraints(constraints);


	/// <param name="priority">The first to be compressed is the one with the lowest <see cref="UILayoutPriority"/></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TView WithContentCompressionResistancePriority<TView>(this TView view, UILayoutPriority priority, UILayoutConstraintAxis axis) where TView : UIView
	{
		view.SetContentCompressionResistancePriority((float)priority, axis);
		return view;
	}

	/// <summary>
	/// Sets the resistance to expansion beyond the UIKit.UIView's UIKit.UIView.IntrinsicContentSize.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TView WithContentHuggingPriority<TView>(this TView view, UILayoutPriority priority, UILayoutConstraintAxis axis) where TView : UIView
	{
		view.SetContentHuggingPriority((float)priority, axis);
		return view;
	}

	public static NSLayoutConstraint WithAutomaticIdentifier(this NSLayoutConstraint constraint, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		constraint.SetIdentifier($"{memberName}_line:{sourceLineNumber}");
		return constraint;
	}

	public static NSLayoutConstraint WithIdentifier(this NSLayoutConstraint constraint, string identifier)
	{
		constraint.SetIdentifier(identifier);
		return constraint;
	}

	#region IPhone X

	private const string IPHONE_8_CDMA = "iPhone10,1";
	private const string IPHONE_8_GSM = "iPhone10,4";
	private const string IPHONE_8_PLUS_CDMA = "iPhone10,2";
	private const string IPHONE_8_PLUS_GSM = "iPhone10,5";

	private const string IPHONE_XS = "iPhone11,1";
	private const string IPHONE_XS_MAX_GLOBAL = "iPhone11,4";
	private const string IPHONE_XS_MAX_CHINA = "iPhone11,6";
	private const string IPHONE_XR = "iPhone11,8";

	private static readonly string[] _lookLikeIPhoneXNames =
	{
		IPHONE_8_CDMA, IPHONE_8_GSM, IPHONE_8_PLUS_CDMA, IPHONE_8_PLUS_GSM,
		IPHONE_XS, IPHONE_XS_MAX_GLOBAL, IPHONE_XS_MAX_CHINA, IPHONE_XR
	};

	private static bool? _haveVirtualButton;
	public static bool HaveVirtualButton(this UIResponder _) => HaveVirtualButton();
	public static bool HaveVirtualButton()
	{
		if (!_haveVirtualButton.HasValue)
		{
			var currentDevice = UIDevice.CurrentDevice;
			//First iPhone with virtual home button released was 'iPhone X with iOS 11'...
			//...so we have to hanlde virtual button only if os >= 11.
			if (currentDevice.CheckSystemVersion(11, 0))
			{
				_haveVirtualButton = currentDevice.Model.StartsWith("iPhone10", StringComparison.InvariantCultureIgnoreCase)
								   || !_lookLikeIPhoneXNames.Contains(currentDevice.Model); //We must exclude iPhone 8, they start with 'iPhone10' but it's not 'iPhone X'.
			}
			else
			{
				_haveVirtualButton = false;
			}
		}
		return _haveVirtualButton.Value;
	}

	private static bool? _haveNotch;
	public static bool HaveNotch(this UIResponder _) => HaveNotch();
	public static bool HaveNotch()
	{
		if (!_haveNotch.HasValue)
		{
			var currentDevice = UIDevice.CurrentDevice;
			//First iPhone with notch released was 'iPhone X with iOS 11'...
			//...so we have to hanlde notch only if os >= 11.
			if (currentDevice.CheckSystemVersion(11, 0))
			{
				UIWindow window = UIApplication.SharedApplication.Delegate.GetWindow();
				if (window != null)
				{
					_haveNotch = window.SafeAreaInsets.Top > 20;
				}
				else
				{
					_haveNotch = false;
				}
			}
			else
			{
				_haveNotch = false;
			}
		}
		return _haveNotch.Value;
	}

	#endregion IPhone X
}