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

		private Dictionary<UIScrollView, UIEdgeInsets> _contentInset = new Dictionary<UIScrollView, UIEdgeInsets>();
		private Dictionary<UIScrollView, UIEdgeInsets> _scrollIndicatorInsets = new Dictionary<UIScrollView, UIEdgeInsets>();

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
			if (activeView?.FindTopSuperviewOfType(_controller.View, typeof(UIScrollView)) is UIScrollView scrollView)
			{
				if (!visible)
				{
					scrollView.RestoreScrollPosition();

					if (_contentInset.ContainsKey(scrollView))
					{
						scrollView.ContentInset = _contentInset[scrollView];
						scrollView.ScrollIndicatorInsets = _scrollIndicatorInsets[scrollView];
					}
				}
				else
				{
					_contentInset[scrollView] = scrollView.ContentInset;
					_scrollIndicatorInsets[scrollView] = scrollView.ScrollIndicatorInsets;

					scrollView.CenterView(activeView, keyboardFrame);
				}
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

				_contentInset.Clear();
				_scrollIndicatorInsets.Clear();
				_contentInset = null;
				_scrollIndicatorInsets = null;
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