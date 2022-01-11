using System;
using Foundation;
using MvvmCross.Platforms.Ios.Views;
using UIKit;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Touch.Views
{
    public abstract class BaseViewController<TViewModel, TParameter> : MvxBaseViewController<TViewModel>
		where TViewModel : BaseViewModel<TParameter>
		where TParameter : class
	{
		private bool _layoutDone;

		#region Constructors

		public BaseViewController() : base() { }
		public BaseViewController(IntPtr handle) : base(handle) { }
		protected BaseViewController(string nibName, NSBundle bundle) : base(nibName, bundle) { }

		#endregion Constructors

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.ViewModel?.OnEnter();
			this.BindControls();

			EdgesForExtendedLayout = UIRectEdge.None;
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			if (!_layoutDone)
			{
				_layoutDone = true;
				this.AutoLayout();
			}

			this.NavigationController.SetNavigationBarHidden(true, false);

			EdgesForExtendedLayout = UIRectEdge.None;

			this.NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
			this.ViewModel?.OnResume();
		}

		public override void ViewDidDisappear(bool animated)
		{
			base.ViewDidDisappear(animated);
			this.ViewModel?.OnPause();
		}

		public override void ViewWillUnload()
		{
			base.ViewWillUnload();
			this.ViewModel?.OnStop();
		}

		protected virtual void BindControls()
		{
		}

		protected virtual void AutoLayout()
		{
		}

		#region helpers methods

		protected void HookHideKeyboard(UIButton button, Action onReturnCallback = null)
		{
			if (button == null)
			{
				return;
			}
			button.TouchUpInside += (sender, args) =>
			{
				this.View.EndEditing(true);
				onReturnCallback?.Invoke();
			};
		}

		protected void HookHideKeyboardOnReturn(UITextField field, Action onReturnCallback = null)
		{
			if (field == null)
			{
				return;
			}
			field.ShouldReturn += (textField) =>
			{
				this.View.EndEditing(true);
				onReturnCallback?.Invoke();
				return true;
			};
		}

		protected void HideKeyboard()
		{
			this.View.EndEditing(true);
		}

		#endregion helpers methods
	}
}
