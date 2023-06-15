using System;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace Xmf2.Core.iOS.Helpers
{
	internal static class KeyboardViewExtensions
	{
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}

			return view.Subviews.Select(FindFirstResponder).FirstOrDefault(firstResponder => firstResponder != null);
		}

		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsInstanceOfType(view.Superview))
				{
					return view.Superview;
				}

				if (!Equals(view.Superview, stopAt))
				{
					return view.Superview.FindSuperviewOfType(stopAt, type);
				}
			}
			return null;
		}

		public static UIView FindTopSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			var superview = view.FindSuperviewOfType(stopAt, type);
			var topSuperView = superview;
			while (superview != null && !Equals(superview, stopAt))
			{
				superview = superview.FindSuperviewOfType(stopAt, type);
				if (superview != null)
				{
					topSuperView = superview;
				}
			}
			return topSuperView;
		}

		public static void RestoreScrollPosition(this UIScrollView scrollView)
		{
			scrollView.ContentInset = UIEdgeInsets.Zero;
			scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
		}

		public static void CenterView(this UIScrollView scrollView, UIView viewToCenter, CGRect keyboardFrame, bool adjustContentInsets = true, bool animated = false)
		{
			var adjustedFrame = UIApplication.SharedApplication.KeyWindow.ConvertRectFromView(scrollView.Frame, scrollView.Superview);
			var intersect = CGRect.Intersect(adjustedFrame, keyboardFrame);
			var height = intersect.Height;
			if (!UIDevice.CurrentDevice.CheckSystemVersion(8, 0) && IsLandscape())
			{
				height = intersect.Width;
			}
			scrollView.CenterView(viewToCenter, height, adjustContentInsets : adjustContentInsets, animated: animated);
		}

		private static void CenterView(this UIScrollView scrollView, UIView viewToCenter, nfloat keyboardHeight = default(nfloat), bool adjustContentInsets = true, bool animated = false)
		{
			nfloat topInset = scrollView.ContentInset.Top;

			if (adjustContentInsets)
			{
				scrollView.ContentInset = new UIEdgeInsets(0, 0, keyboardHeight, 0);
				scrollView.ScrollIndicatorInsets = new UIEdgeInsets(0, 0, keyboardHeight, 0);
			}

			// Position of the active field relative isnside the scroll view
			CGRect relativeFrame = viewToCenter.Superview.ConvertRectToView(viewToCenter.Frame, scrollView);

			var spaceAboveKeyboard = scrollView.Frame.Height - keyboardHeight;

			// Move the active field to the center of the available space
			var offset = relativeFrame.Y - (spaceAboveKeyboard - viewToCenter.Frame.Height) / 2 - topInset;
			if (scrollView.ContentOffset.Y < offset)
			{
				scrollView.SetContentOffset(new CGPoint(0, offset), animated);
			}
		}

		private static void MakeViewVisible(this UIScrollView scrollView, UIView viewToCenter, nfloat keyboardHeight = default, bool adjustContentInsets = true, bool animated = false)
		{
			if (adjustContentInsets)
			{
				scrollView.ContentInset = new UIEdgeInsets(0, 0, keyboardHeight, 0);
				scrollView.ScrollIndicatorInsets = new UIEdgeInsets(0, 0, keyboardHeight, 0);
			}

			// Position of the active field relative isnside the scroll view
			CGRect relativeFrame = viewToCenter.Superview.ConvertRectToView(viewToCenter.Frame, scrollView);

			var spaceAboveKeyboard = scrollView.Frame.Height - keyboardHeight;

			// Move the active field to the center of the available space
			var offset = (relativeFrame.Bottom + 12) - spaceAboveKeyboard;
			if (scrollView.ContentOffset.Y < offset)
			{
				scrollView.SetContentOffset(new CGPoint(0, offset), animated);
			}
		}

		private static bool IsLandscape()
		{
			var orientation = UIApplication.SharedApplication.StatusBarOrientation;
			bool landscape = orientation == UIInterfaceOrientation.LandscapeLeft || orientation == UIInterfaceOrientation.LandscapeRight;
			return landscape;
		}
	}
}