using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Xmf2.Commons.Droid.Helpers
{
	public class LoadingViewHelper
	{
		private readonly Context _context;
		private readonly ViewGroup _viewGroup;

		private View _loadingView;

		private readonly int _loadingViewResource;
		private readonly int _progress;

		private bool _isBusy;

		public virtual bool IsBusy
		{
			get => _isBusy;
			set
			{
				if (_isBusy != value)
				{
					_isBusy = value;
					_loadingView.Visibility = _isBusy ? ViewStates.Visible : ViewStates.Gone;
				}
			}
		}

		public LoadingViewHelper(ViewGroup view, int loadingViewResource, int progress)
		{
			_context = view.Context;
			_viewGroup = view;
			_loadingViewResource = loadingViewResource;
			_progress = progress;
			CreateLoadingView();
		}

		private void CreateLoadingView()
		{
			if (_loadingView == null)
			{
				var loadingView = LayoutInflater.From(_context).Inflate(_loadingViewResource, null);
				UIHelper.SetColorFilter(loadingView.FindViewById<ProgressBar>(_progress), Color.White);
				if (_viewGroup != null)
				{
					_loadingView = loadingView;
					_viewGroup.AddView(_loadingView, GetLayoutManager());
					_loadingView.Visibility = ViewStates.Gone;
				}
			}
		}

		private ViewGroup.LayoutParams GetLayoutManager()
		{
			return new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}
	}
}