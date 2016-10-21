using System;
using UIKit;

public static class CustomAutoLayoutExtensions
{
	public static void CenterAndFillWidth(this UIView containerView, params UIView[] views)
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

		for (int i = 1; i < views.Length; ++i)
		{
			containerView.VerticalSpace(views[i - 1], views[i]);
		}
	}

	public static void AnchorTop(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => view.Top() == containerView.Top() + margin);
	}

	public static void AnchorBottom(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => containerView.Bottom() == view.Bottom() + margin);
	}

	public static void AnchorRight(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => containerView.Right() == view.Right() + margin);
	}

	public static void AnchorLeft(this UIView containerView, UIView view, int margin = 0)
	{
		containerView.ConstrainLayout(() => view.Left() == containerView.Left() + margin);
	}

	public static void VerticalSpace(this UIView containerView, UIView top, UIView bottom, int margin = 0)
	{
		containerView.ConstrainLayout(() => bottom.Top() == top.Bottom() + margin);
	}

	public static UIView ConstrainHeight(this UIView view, int height)
	{
		view.ConstrainLayout(() => view.Height() == height);
		return view;
	}
}
