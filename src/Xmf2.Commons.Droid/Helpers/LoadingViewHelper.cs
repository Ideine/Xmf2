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

		private View _loadingView;

		private int LoadingView;
		private int Progress;

		private bool _isBusy;
		public virtual bool IsBusy
		{
			get => _isBusy;
			set
			{
				if (_isBusy != value)
				{
					_isBusy = value;
					if (_isBusy)
					{
						_loadingView.Visibility = ViewStates.Visible;
					}
					else
					{
						_loadingView.Visibility = ViewStates.Gone;
					}
				}
			}
		}

		public LoadingViewHelper(Activity activity, int loadingView, int progress)
		{
			_activity = activity;
			LoadingView = loadingView;
			Progress = progress;
			CreateLoadingView();
		}

		private void CreateLoadingView()
		{
			if (_loadingView == null)
			{
				var decorView = _activity.Window.DecorView;
				_loadingView = LayoutInflater.From(_activity).Inflate(LoadingView, null);
				UIHelper.SetColorFilter(_loadingView.FindViewById<ProgressBar>(Progress), Color.White);
				var viewGroup = decorView as ViewGroup;
				if (viewGroup != null)
				{
					viewGroup.AddView(_loadingView, GeLayoutManager());
					_loadingView.Visibility = ViewStates.Gone;
				}
			}
		}

		private ViewGroup.LayoutParams GeLayoutManager()
		{
			return new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
		}
	}
}
