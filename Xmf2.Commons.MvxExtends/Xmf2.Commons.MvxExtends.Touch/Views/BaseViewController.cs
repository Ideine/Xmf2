using Foundation;
using MvvmCross.iOS.Support.Views;
using System;
using UIKit;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Touch.Views
{
    public abstract class BaseViewController<TViewModel>
        : MvxBaseViewController<TViewModel> where TViewModel : BaseViewModel
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
            ViewModel?.OnEnter();
            BindControls();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewModel?.OnResume();
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
                NavigationController.NavigationBar.BarStyle = UIBarStyle.Default;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewModel?.OnPause();
        }

        public override void ViewWillUnload()
        {
            base.ViewWillUnload();
            ViewModel?.OnStop();
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
