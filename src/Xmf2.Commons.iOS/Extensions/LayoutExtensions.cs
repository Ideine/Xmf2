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
		return CenterAndFillWidth(containerView, 0, views);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillWidth(this UIView containerView, int margin, params UIView[] views)
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
	public static UIView CenterAndFillWidth(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => view.CenterX() == containerView.CenterX()
										 && view.Width()   == containerView.Width() - margin
									);
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillHeight(this UIView containerView, params UIView[] views)
	{
		return CenterAndFillHeight(containerView, 0, views);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CenterAndFillHeight(this UIView containerView, int margin, params UIView[] views)
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

	public static UIView CenterAndFillHeight(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => view.CenterY() == containerView.CenterY()
										 && view.Height()  == containerView.Height() - margin
										 );
		return containerView;
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

			containerView.ConstrainLayout(() => v1.Width() == v2.Width());
		}

		return containerView;
	}

	public static UIView IncloseFromBottom(this UIView containerView, UIView view)
	{
		return IncloseFromBottom(containerView, view, 0);
	}

	public static UIView IncloseFromBottom(this UIView containerView, UIView view, int margin)
	{
		containerView.ConstrainLayout(() => containerView.Bottom() >= view.Bottom() + margin);
		return containerView;
	}

	public static UIView IncloseFromTop(this UIView containerView, UIView view)
	{
		return IncloseFromTop(containerView, view, 0);
	}

	public static UIView IncloseFromTop(this UIView containerView, UIView view, int margin)
	{
		containerView.ConstrainLayout(() => containerView.Top() <= view.Top() - margin);
		return containerView;
	}

	public static UIView IncloseFromRight(this UIView containerView, UIView view)
	{
		return IncloseFromRight(containerView, view, 0);
	}

	public static UIView IncloseFromRight(this UIView containerView, UIView view, int margin)
	{
		containerView.ConstrainLayout(() => containerView.Right() >= view.Right() + margin);
		return containerView;
	}

	public static UIView IncloseFromLeft(this UIView containerView, UIView view)
	{
		return IncloseFromLeft(containerView, view, 0);
	}

	public static UIView IncloseFromLeft(this UIView containerView, UIView view, int margin)
	{
		containerView.ConstrainLayout(() => containerView.Left() <= view.Left() - margin);
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView AnchorLeft(this UIView containerView, UIView view, float margin = 0)
	{
		containerView.AddConstraint(NSLayoutConstraint.Create(containerView, Left, Equal, view, Left, 1, -margin));
		containerView.TranslatesAutoresizingMaskIntoConstraints = false;
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		return containerView;
	}

	public static UIView CenterHorizontally(this UIView containerView, UIView view)
	{
		containerView.ConstrainLayout(() => view.CenterX() == containerView.CenterX());
		return containerView;
	}
	public static UIView CenterHorizontally(this UIView containerView, params UIView[] views)
	{
		foreach (var view in views)
		{
			containerView.ConstrainLayout(() => view.CenterX() == containerView.CenterX());
		}
		return containerView;
	}

	public static UIView CenterVertically(this UIView containerView, UIView view)
	{
		containerView.ConstrainLayout(() => view.CenterY() == containerView.CenterY());
		return containerView;
	}

	public static UIView CenterVertically(this UIView containerView, params UIView[] views)
	{
		foreach (UIView view in views)
		{
			containerView.ConstrainLayout(() => view.CenterY() == containerView.CenterY());
		}
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
	public static UIView FillHeight(this UIView containerView, params UIView[] views)
	{
		foreach (var view in views)
		{
			containerView.ConstrainLayout(() => containerView.Height() == view.Height() + 0);
		}
		return containerView;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView VerticalSpace(this UIView containerView, UIView topView, UIView bottomView, float margin = 0)
	{
		containerView.AddConstraint(NSLayoutConstraint.Create(topView, Bottom, Equal, bottomView, Top, 1f, -margin));
		topView.TranslatesAutoresizingMaskIntoConstraints = false;
		bottomView.TranslatesAutoresizingMaskIntoConstraints = false;
		return containerView;
	}
	public static UIView MinVerticalSpace(this UIView containerView, UIView top, UIView bottom, int margin = 0)
	{
		containerView.ConstrainLayout(() => bottom.Top() >= top.Bottom() + margin);
		return containerView;
	}

	public static UIView HorizontalSpace(this UIView containerView, UIView left, UIView right, int margin = 0)
	{
		containerView.ConstrainLayout(() => right.Left() == left.Right() + margin);
		return containerView;
	}
	public static UIView MinHorizontalSpace(this UIView containerView, UIView left, UIView right, int margin = 0)
	{
		containerView.ConstrainLayout(() => right.Left() >= left.Right() + margin);
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

	public static UIView ConstrainSize(this UIView view, int width, int height)
	{
		view.ConstrainLayout(() => view.Width() == width && view.Height() == height);
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView Same(this UIView view, UIView childView)
	{
		return view.Same(view, childView);
	}

	public static UIView Same(this UIView view, UIView reference, UIView dest)
	{
		view.ConstrainLayout(() => reference.CenterY() == dest.CenterY()
							 && reference.CenterX() == dest.CenterX()
							 && reference.Height() == dest.Height()
							 && reference.Width() == dest.Width());
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView SameWidth(this UIView view, UIView v1, UIView v2)
	{
		view.AddConstraint(NSLayoutConstraint.Create(v1, Width, Equal, v2, Width, 1f, 0f));
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		return view;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView SameHeight(this UIView view, UIView v1, UIView v2)
	{
		view.AddConstraint(NSLayoutConstraint.Create(v1, Height, Equal, v2, Height, 1f, 0f));
		view.TranslatesAutoresizingMaskIntoConstraints = false;
		return view;
	}

	public static UIScrollView VerticalScrollContentConstraint(this UIScrollView scroll, UIView content)
	{
		scroll.ConstrainLayout(() => scroll.Left() == content.Left()
							   && scroll.Right() == content.Right()
							   && scroll.Top() == content.Top()
							   && scroll.Bottom() == content.Bottom()
							   && scroll.CenterX() == content.CenterX());
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
		return EnsureRemove(view, constraintsToRemove.ToArray());
	}
	public static UIView EnsureRemove(this UIView view, params NSLayoutConstraint[] constraintsToRemove)
	{
		var delta = view.Constraints.Intersect(constraintsToRemove).ToArray();
		view.RemoveConstraints(delta);
		return view;
	}
	public static UIView EnsureAdd(this UIView view, IEnumerable<NSLayoutConstraint> constraintsToAdd)
	{
		return EnsureAdd(view, constraintsToAdd.ToArray());
	}
	public static UIView EnsureAdd(this UIView view, params NSLayoutConstraint[] constraintsToAdd)
	{
		var delta = constraintsToAdd.Except(view.Constraints).ToArray();
		view.AddConstraints(delta);
		return view;
	}
	public static UIView EnsureAdd(this UIView view, params UIGestureRecognizer[] gestureRecognizersToAdd)
	{
		var delta = gestureRecognizersToAdd.Except(view.GestureRecognizers).ToArray();
		foreach (var recognizer in delta)
		{
			view.AddGestureRecognizer(recognizer);
		}
		return view;
	}
	public static UIView EnsureRemove(this UIView view, params UIGestureRecognizer[] gestureRecognizersToRemove)
	{
		var delta = view.GestureRecognizers.Intersect(gestureRecognizersToRemove).ToArray();
		foreach (var recognizer in delta)
		{
			view.RemoveGestureRecognizer(recognizer);
		}
		return view;
	}
	public static UIView EnsureRemove(this UIView view, params UIView[] subviewsToRemove)
	{
		var delta = view.Subviews.Intersect(subviewsToRemove).ToArray();
		foreach (var subView in delta)
		{
			subView.RemoveFromSuperview();
		}
		return view;
	}

	public static UIView EnsureAdd(this UIView view, params UIView[] subviewsToAdd)
	{
		var delta = subviewsToAdd.Except(view.Subviews).ToArray();
		view.AddSubviews(delta);
		return view;
	}
}
