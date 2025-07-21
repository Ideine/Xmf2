using CoreGraphics;
using Foundation;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using UIKit;
using Xmf2.Rx.ViewModels;

namespace Xmf2.Rx.iOS
{
	public abstract class BaseViewController<TViewModel> : ReactiveViewController<TViewModel> where TViewModel : BaseViewModel
	{
		protected readonly CompositeDisposable _uiDisposables = new CompositeDisposable();

		protected abstract TViewModel GetViewModel();

		private bool _layoutDone;

		public BaseViewController() : base() { }

		public BaseViewController(IntPtr handle) : base(handle) { }

		#region Lifecycle

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			ViewModel = GetViewModel();
			ViewModel.LifecycleManager.Start();
			BindControls();
			SetNeedsStatusBarAppearanceUpdate();
		}

		protected virtual void BindControls() { }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			ViewModel?.LifecycleManager.Resume();
			if (!_layoutDone)
			{
				_layoutDone = true;
				AutoLayout();
			}

			NavigationController?.SetNavigationBarHidden(true, false);

			if (RespondsToSelector(new ObjCRuntime.Selector("edgesForExtendedLayout")))
			{
				EdgesForExtendedLayout = UIRectEdge.None;
			}

			if (NavigationController != null)
			{
				NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
			}
		}

		protected virtual void AutoLayout() { }

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			ViewModel?.LifecycleManager.Pause();
		}

		public override void ViewWillUnload()
		{
			base.ViewWillUnload();
			ViewModel?.LifecycleManager.Stop();
		}

		#endregion

		protected void RequestViewModelClose()
		{
			ViewModel?.CloseCommand.TryExecute();
		}

		public override UIStatusBarStyle PreferredStatusBarStyle() => UIStatusBarStyle.LightContent;

		#region Keyboard Features
		//Code from MvvmCross https://github.com/MvvmCross/MvvmCross
		/// <summary>
		/// The view to center on keyboard shown
		/// </summary>
		protected UIView ViewToCenterOnKeyboardShown { get; set; }

		/// <summary>
		/// The scroll to center on keyboard shown
		/// </summary>
		protected UIScrollView ScrollToCenterOnKeyboardShown { get; set; }

		private NSObject _keyboardShowObserver;
		private NSObject _keyboardHideObserver;

		protected virtual void InitKeyboardHandling(bool enableAutoDismiss = true)
		{
			RegisterForKeyboardNotifications();
			if (enableAutoDismiss)
			{
				DismissKeyboardOnBackgroundTap();
			}
		}

		public virtual bool HandlesKeyboardNotifications()
		{
			return false;
		}
		protected virtual void RegisterForKeyboardNotifications()
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

		protected virtual void UnregisterForKeyboardNotifications()
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
		protected virtual UIView KeyboardGetActiveView()
		{
			return View.FindFirstResponder();
		}

		/// <summary>
		/// Called when keyboard notifications are produced.
		/// </summary>
		/// <param name="notification">The notification.</param>
		private void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded)
			{
				return;
			}

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

		/// <summary>
		/// Override this method to apply custom logic when the keyboard is shown/hidden
		/// </summary>
		/// <param name='visible'>
		/// If the keyboard is visible
		/// </param>
		/// <param name='keyboardFrame'>
		/// Frame of the keyboard
		/// </param>
		protected virtual void OnKeyboardChanged(bool visible, CGRect keyboardFrame)
		{
			var activeView = ViewToCenterOnKeyboardShown ?? KeyboardGetActiveView();
			if (activeView == null)
			{
				return;
			}
			var scrollView = ScrollToCenterOnKeyboardShown ?? activeView.FindTopSuperviewOfType(View, typeof(UIScrollView)) as UIScrollView;
			if (scrollView == null)
			{
				return;
			}

			if (!visible)
			{
				scrollView.RestoreScrollPosition();
			}
			else
			{
				scrollView.CenterView(activeView, keyboardFrame);
			}
		}

		/// <summary>
		/// Call it to force dismiss keyboard when background is tapped
		/// </summary>
		protected void DismissKeyboardOnBackgroundTap()
		{
			// Add gesture recognizer to hide keyboard
			var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
			tap.AddTarget(() => View.EndEditing(true));
			tap.ShouldReceiveTouch = (recognizer, touch) => !(touch.View is UIControl || touch.View.FindSuperviewOfType(View, typeof(UITableViewCell)) != null);
			View.AddGestureRecognizer(tap);
		}

		protected void HideKeyboard()
		{
			View.EndEditing(true);
		}
		#endregion


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_uiDisposables?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
