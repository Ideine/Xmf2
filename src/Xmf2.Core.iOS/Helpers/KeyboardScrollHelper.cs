using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xmf2.Core.iOS.Helpers
{
	public class KeyboardScrollHelper : IDisposable
	{
		private UIViewController _controller;
		private NSObject _keyboardShowObserver;
		private NSObject _keyboardHideObserver;
		private CGRect _lastKeyboardFrame = CGRect.Empty;
		private readonly WeakReference<UIView?> _lastActiveView = new WeakReference<UIView?>(null);

		public KeyboardScrollHelper(UIViewController controller)
		{
			_controller = controller;

			RegisterForKeyboardNotifications();
			DismissKeyboardOnBackgroundTap();
		}

		private void RegisterForKeyboardNotifications()
		{
			if (_keyboardShowObserver == null)
			{
				_keyboardShowObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
			}

			if (_keyboardHideObserver == null)
			{
				_keyboardHideObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			}
		}

		private void UnregisterForKeyboardNotifications()
		{
			if (_keyboardShowObserver != null)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardShowObserver);
				_keyboardShowObserver.Dispose();
				_keyboardShowObserver = null;
			}

			if (_keyboardHideObserver != null)
			{
				NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardHideObserver);
				_keyboardHideObserver.Dispose();
				_keyboardHideObserver = null;
			}
		}

		/// <summary>
		/// Gets the UIView that represents the "active" user input control (e.g. textfield, or button under a text field)
		/// </summary>
		/// <returns>
		/// A <see cref="UIView"/>
		/// </returns>
		private UIView KeyboardGetActiveView()
		{
			return _controller.View.FindFirstResponder();
		}

		/// <summary>
		/// Called when keyboard notifications are produced.
		/// </summary>
		/// <param name="notification">The notification.</param>
		private void OnKeyboardNotification(NSNotification notification)
		{
			if (_controller.IsViewLoaded)
			{
				//Check if the keyboard is becoming visible
				var visible = notification.Name == UIKeyboard.WillShowNotification;

				//Start an animation, using values from the keyboard
				UIView.BeginAnimations("AnimateForKeyboard");
				UIView.SetAnimationBeginsFromCurrentState(true);
				UIView.SetAnimationDuration(UIKeyboard.AnimationDurationFromNotification(notification));
				UIView.SetAnimationCurve((UIViewAnimationCurve)UIKeyboard.AnimationCurveFromNotification(notification));

				//Pass the notification, calculating keyboard height, etc.
				var keyboardFrame = visible
					? UIKeyboard.FrameEndFromNotification(notification)
					: UIKeyboard.FrameBeginFromNotification(notification);

				OnKeyboardChanged(visible, keyboardFrame);

				//Commit the animation
				UIView.CommitAnimations();
			}

		}

		/// <summary>
		/// Override this method to apply custom logic when the keyboard is shown/hidden
		/// </summary>
		/// <param name='visible'>
		/// If the keyboard is visible
		/// </param>
		/// <param name='keyboardFrame'>
		/// Frame of the keyboard
		/// </param>
		private void OnKeyboardChanged(bool visible, CGRect keyboardFrame)
		{
			var activeView = KeyboardGetActiveView();
			if (activeView == null)
			{
				_lastKeyboardFrame = CGRect.Empty;
				_lastActiveView.SetTarget(null);
				return;
			}

			var scrollView = activeView.FindTopSuperviewOfType(_controller.View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
			{
				_lastKeyboardFrame = CGRect.Empty;
				_lastActiveView.SetTarget(null);
				return;
			}

			if (!visible)
			{
				_lastKeyboardFrame = CGRect.Empty;
				_lastActiveView.SetTarget(null);
				scrollView.RestoreScrollPosition();
			}
			else
			{
				//avoid recalculation if the activeView is the same.
				if (_lastKeyboardFrame == keyboardFrame && _lastActiveView.TryGetTarget(out var lastActiveView) &&
					lastActiveView?.Equals(activeView) == true)
				{
					return;
				}

				_lastKeyboardFrame = keyboardFrame;
				_lastActiveView.SetTarget(activeView);
				if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
				{
					keyboardFrame.Height -= scrollView.SafeAreaInsets.Bottom;
				}
				scrollView.CenterView(activeView, keyboardFrame, adjustContentInsets: false);
				//CLA: 15/06/2022 adjustContentInsets adds an extra space to the bottom of the scroll content while the keyboard is out.
				//The scroll view now goes back to its normal size after dismissing the keyboard, but we don't want that extra space when it's out either
			}
		}

		/// <summary>
		/// Call it to force dismiss keyboard when background is tapped
		/// </summary>
		private void DismissKeyboardOnBackgroundTap()
		{
			// Add gesture recognizer to hide keyboard
			var tap = new UITapGestureRecognizer {CancelsTouchesInView = false};
			tap.AddTarget(() => _controller.View.EndEditing(true));
			tap.ShouldReceiveTouch = (recognizer, touch) => !(touch.View is UIControl || touch.View.FindSuperviewOfType(_controller.View, typeof(UITableViewCell)) != null);
			_controller.View.AddGestureRecognizer(tap);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				UnregisterForKeyboardNotifications();
				_controller = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~KeyboardScrollHelper()
		{
			Dispose(false);
		}
	}
}