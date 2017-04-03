﻿using System;
using UIKit;

public static class CustomAutoLayoutExtensions
{
	public static UIView CenterAndFillWidth(this UIView containerView, params UIView[] views)
	{
		if (views == null)
		{
			throw new ArgumentNullException(nameof(views));
		}

		foreach (UIView view in views)
		{
			containerView.ConstrainLayout(() =>
										  view.CenterX() == containerView.CenterX()
										  && view.Width() == containerView.Width()
										 );
		}

		return containerView;
	}

    public static UIView CenterAndFillWidth(this UIView containerView, int margin, params UIView[] views)
    {
        if (views == null)
        {
            throw new ArgumentNullException(nameof(views));
        }

        foreach (UIView view in views)
        {
            containerView.ConstrainLayout(() =>
                                          view.CenterX() == containerView.CenterX()
                                          && view.Width() == containerView.Width() - margin
                                         );
        }

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

	public static UIView ConstrainSides(this UIView view, int widthAndHeight)
	{
		view.ConstrainLayout(() => view.Width()  == widthAndHeight
								&& view.Height() == widthAndHeight);
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
		scroll.ConstrainLayout(() => scroll.Left() == content.Left()
							   && scroll.Right() == content.Right()
							   && scroll.Top() == content.Top()
							   && scroll.Bottom() == content.Bottom()
							   && scroll.CenterX() == content.CenterX());
		return scroll;
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
}
