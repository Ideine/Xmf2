using CoreGraphics;
using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public abstract class BaseDialogViewController : UIViewController
	{
		private readonly bool _allowDismiss;
		private readonly UIColor _backgroundColor;
		protected abstract UIView PopupContentView { get; }

		public BaseDialogViewController(bool allowDismiss, UIColor backgroundColor = null)
		{
			_allowDismiss = allowDismiss;
			_backgroundColor = backgroundColor ?? UIColor.Black.ColorWithAlpha(200f);
			LoadModalContext();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			LoadModalContext();

			View.BackgroundColor = _backgroundColor;

			if (_allowDismiss)
			{
				View.AddGestureRecognizer(new UITapGestureRecognizer(OnTapped));
			}
			View.BringSubviewToFront(PopupContentView);
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			AutoLayout();
		}

		protected virtual void AutoLayout() { }

		protected void LoadModalContext()
		{
			ModalPresentationStyle = UIModalPresentationStyle.OverCurrentContext;
		}

		protected virtual void OnTapped(UITapGestureRecognizer recognizer)
		{
			if (!PopupContentView.Frame.IntersectsWith(new CGRect(recognizer.LocationInView(View), new CGSize(1, 1))))
			{
				CloseView();
			}
		}

		protected void CloseView()
		{
			DismissModalViewController(false);
		}
	}
}
