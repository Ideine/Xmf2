using System;
using UIKit;
using CoreGraphics;

public static class AnimationExtensions
{
	public static TView TranslateY<TView>(this TView view, nfloat offset) where TView : UIView
	{
		CGRect frame = view.Frame;
		frame.Y += offset;
		view.Frame = frame;

		return view;
	}

	public static TView TranslateX<TView>(this TView view, nfloat offset) where TView : UIView
	{
		CGRect frame = view.Frame;
		frame.X += offset;
		view.Frame = frame;

		return view;
	}

	public static TView FadeIn<TView>(this TView view) where TView : UIView
	{
		view.Alpha = 1f;

		return view;
	}

	public static TView FadeOut<TView>(this TView view) where TView : UIView
	{
		view.Alpha = 0f;

		return view;
	}
}
