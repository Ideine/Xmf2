using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public class UILoadingView : UIView
	{
		private readonly UIView _parentView;

		private readonly UIView _container;
		private readonly UIActivityIndicatorView _progressView;
		private readonly UILabel _title;

		public UILoadingView(UIView parentView)
		{
			_parentView = parentView;

			this.WithSubviews(
				_container = this.CreateView().WithSubviews(
					_progressView = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge),
					_title = this.CreateLabel()
					.WithSystemFont(15)
					.WithAlignment(UITextAlignment.Center)
					.WithTextColor(UIColor.White)
				));

			this.BackgroundColor = UIColor.Black.ColorWithAlpha(0.5f);
		}

		public void WithTitle(string title) => _title.WithText(title);

		private void ApplyAutoLayout()
		{
			_parentView.Same(_parentView, this);
			this.CenterVertically(_container)
				.CenterHorizontally(_container);

			_container.CenterAndFillWidth(_title)
					  .CenterHorizontally(_progressView)
					  .AnchorTop(_progressView)
					  .VerticalSpace(_progressView, _title, 10)
					  .AnchorBottom(_title)
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
