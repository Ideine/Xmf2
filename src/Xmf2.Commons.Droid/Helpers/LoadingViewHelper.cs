using System;
using Android;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;

namespace Xmf2.Commons.Droid.Helpers
{
	public class LoadingViewHelper
	{
		private readonly Activity _activity;

		public View LoadingView { get; private set; }

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
					LoadingView.Visibility = _isBusy ? ViewStates.Visible : ViewStates.Gone;
				}
			}
		}

		public LoadingViewHelper(Activity activity, int loadingViewResource, int progress)
		{
			_activity = activity;
			_loadingViewResource = loadingViewResource;
			_progress = progress;
			CreateLoadingView();
		}

		private void CreateLoadingView()
		{
			if (LoadingView == null)
			{
				var decorView = _activity.Window.DecorView;
				LoadingView = LayoutInflater.From(_activity).Inflate(_loadingViewResource, null);
				UIHelper.SetColorFilter(LoadingView.FindViewById<ProgressBar>(_progress), Color.White);
				if (decorView is ViewGroup viewGroup)
				{
					viewGroup.AddView(LoadingView, GetLayoutParams());
					LoadingView.Visibility = ViewStates.Gone;
				}
			}
		}

		private ViewGroup.LayoutParams GetLayoutParams()
		{
			return new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}
	}
}
