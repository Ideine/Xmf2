using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public class UILoadingView : UIView
	{
		private readonly UIView _parentView;
		private readonly UIActivityIndicatorView _progressView;

		public UILoadingView(UIView parentView)
		{
			_parentView = parentView;
			_progressView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			this.AddSubviews(_progressView);
			this.BackgroundColor = UIColor.Black.ColorWithAlpha(0.5f);
		}

		private void ApplyAutoLayout()
		{
			_parentView.Same(_parentView, this);
			this.CenterVertically(_progressView)
				.CenterHorizontally(_progressView);
		}

		public void UpdateViewState(bool isBusy)
		{
			if (isBusy)
			{
				_parentView.EndEditing(true);
				Alpha = 0;
				_parentView.Add(this);
				ApplyAutoLayout();
				_progressView.StartAnimating();
				UIView.Animate(0.2, () => Alpha = 1, ActionHelper.NoOp);
			}
			else
			{
				UIView.Animate(0.2, () => Alpha = 0, () =>
				{
					this.RemoveFromSuperview();
					_progressView.StopAnimating();
				});
			}
		}
	}
}
