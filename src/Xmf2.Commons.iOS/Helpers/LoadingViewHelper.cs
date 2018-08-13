using UIKit;
using Xmf2.Commons.iOS.Controls;

namespace Xmf2.Commons.iOS.Helpers
{
	public class LoadingViewHelper
	{
		private UILoadingView _loadingView;
		private bool _isBusy;

		public virtual bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				CreateLoadingViewIfNeeded();
				if (_isBusy != value)
				{
					_isBusy = value;
					_loadingView?.UpdateViewState(_isBusy);
				}
			}
		}

		private readonly UIView _parent;

		public LoadingViewHelper(UIView parent)
		{
			_parent = parent;
		}

		private void CreateLoadingViewIfNeeded()
		{
			if (_loadingView == null)
			{
				_loadingView = new UILoadingView(_parent);
			}
		}

		public void WithTitle(string title) => _loadingView?.WithTitle(title);
	}
}
