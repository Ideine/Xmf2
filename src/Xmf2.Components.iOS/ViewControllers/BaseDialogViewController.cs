using System;
using CoreGraphics;
using UIKit;
using Xmf2.Core.Helpers;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.iOS.ViewControllers
{
	public abstract class BaseDialogViewController : UIViewController
	{
		private readonly bool _allowDismiss;
		private readonly UIColor _backgroundColor;

		protected abstract UIView PopupContentView { get; }
		protected Xmf2Disposable Disposable = new Xmf2Disposable();

		public BaseDialogViewController(UIColor backgroundColor, bool allowDismiss)
		{
			_allowDismiss = allowDismiss;
			_backgroundColor = backgroundColor;
			LoadModalContext();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			View.BackgroundColor = _backgroundColor;

			if (_allowDismiss)
			{
				View.AddGestureRecognizer(new UITapGestureRecognizer(OnTapped)
				{
					CancelsTouchesInView = false
				});
			}

			View.BringSubviewToFront(PopupContentView);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			AutoLayout();
		}

		protected virtual void AutoLayout() { }

		protected virtual void LoadModalContext() => ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;

		protected virtual void OnTapped(UITapGestureRecognizer recognizer)
		{
			if (!PopupContentView.Frame.IntersectsWith(new CGRect(recognizer.LocationInView(View), new CGSize(1, 1))))
			{
				CloseView();
			}
		}

		protected virtual void CloseView(Action action = null)
		{
			DismissViewController(false, action ?? ActionHelper.NoOp);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Disposable?.Dispose();
				Disposable = null;
			}

			base.Dispose(disposing);
		}
	}
}