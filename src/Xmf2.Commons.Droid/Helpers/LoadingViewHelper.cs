using System;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Xmf2.Commons.Droid.Helpers
{
	public class LoadingViewHelper : IDisposable
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
				View loadingView;
				using (var inflater = LayoutInflater.From(_context))
				{
					loadingView = inflater.Inflate(_loadingViewResource, null);
				}
				using (var progressView = loadingView.FindViewById<ProgressBar>(_progress))
				{
					UIHelper.SetColorFilter(progressView, Color.White);
				}
				if (_viewGroup != null)
				{
					_loadingView = loadingView;
					using (var layoutManager = GetLayoutManager())
					{
						_viewGroup.AddView(_loadingView, layoutManager);
					}
					_loadingView.Visibility = ViewStates.Gone;
				}
			}
		}

		private ViewGroup.LayoutParams GetLayoutManager()
		{
			return new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}

		public void Dispose()
		{
			_viewGroup.Dispose();
			_loadingView.Dispose();
		}
	}
}