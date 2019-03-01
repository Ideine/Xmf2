using UIKit;
using Xmf2.Core.Helpers;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Core.iOS.Controls
{
	public class UILoadingView : UIView
	{
		private UIView _parentView;
		private UIActivityIndicatorView _progressView;

		private NSLayoutConstraint[] _constraints; 
		
		public UILoadingView(UIView parentView)
		{
			_parentView = parentView;
			_progressView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			AddSubviews(_progressView);

			this.CenterVertically(_progressView)
				.CenterHorizontally(_progressView);

			BackgroundColor = UIColor.Black.ColorWithAlpha(0.5f);
			TranslatesAutoresizingMaskIntoConstraints = false;

			_constraints = new[]
			{
				NSLayoutConstraint.Create(this, CenterX, Equal, parentView, CenterX, 1, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, CenterY, Equal, parentView, CenterY, 1, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, Width, Equal, parentView, Width, 1, 0).WithAutomaticIdentifier(),
				NSLayoutConstraint.Create(this, Height, Equal, parentView, Height, 1, 0).WithAutomaticIdentifier(),
			};
		}

		public void UpdateViewState(bool isBusy)
		{
			if (isBusy)
			{
				_parentView.EndEditing(true);
				Alpha = 0;
				_parentView.Add(this);
				_parentView.AddConstraints(_constraints);
				_progressView.StartAnimating();
				UIView.Animate(0.05, () => Alpha = 1, ActionHelper.NoOp);
			}
			else
			{
				UIView.Animate(0.05, () => Alpha = 0, () =>
				{
					_parentView.RemoveConstraints(_constraints);
					RemoveFromSuperview();
					_progressView.StopAnimating();
				});
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_progressView.Dispose();
				_progressView = null;
				_constraints = null;
			}
			base.Dispose(disposing);
			
		}
	}
}