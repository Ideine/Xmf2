using System;
using Android;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
#if __ANDROID_29__
using AndroidX.Core.Content;
#else
using Android.Support.V4.Content;
#endif
using Plugin.CurrentActivity;
using Xmf2.Core.Helpers;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.Droid.Helpers
{
	public class LoadingViewHelper : IDisposable
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();

		private LoadingEnableHelper _throttling;

		private Context _context;
		private ViewGroup _viewGroup;

		private View _loadingView;
		private TextView _textView;

		private bool _isBusy;

		public virtual bool IsBusy
		{
			get => _isBusy;
			set
			{
				if (_isBusy != value)
				{
					_isBusy = value;
					_throttling.Set(value);
				}
			}
		}

		public string LoadingText
		{
			get => _textView.Text;
			set => _textView.Text = value;
		}

		public void SetTextColor(Color color)
		{
			_textView.SetTextColor(color);
		}

		public void SetTextColor(int colorId)
		{
			_textView.SetTextColor(ContextCompat.GetColorStateList(_context, colorId));
		}

		public LoadingViewHelper(ViewGroup view)
		{
			_throttling = new LoadingEnableHelper(v =>
			{
				new Handler(Looper.MainLooper).DisposeWith(_disposable).Post(() =>
				{
					this.WrapForDisposedException(() => _loadingView.SetVisibleOrGone(v));
				});
			}).DisposeWith(_disposable);

			_viewGroup = view ?? CrossCurrentActivity.Current.Activity.FindViewById<ViewGroup>(Android.Resource.Id.Content);
			_context = _viewGroup.Context;
			CreateLoadingView();
		}

		private void CreateLoadingView()
		{
			if (_loadingView == null)
			{
				View loadingView;
				using (var inflater = LayoutInflater.From(_context))
				{
					loadingView = inflater.Inflate(Resource.Layout.LoadingView, null).DisposeViewWith(_disposable);
				}

				using (var progressView = loadingView.FindViewById<ProgressBar>(Resource.Id.LoadingProgress))
				{
					UIHelper.SetColorFilter(progressView, Color.White);
				}

				_textView = loadingView.FindViewById<TextView>(Resource.Id.LoadingText).DisposeViewWith(_disposable);

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

		#region Dispose

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable.Dispose();

				_throttling = null;
				_viewGroup = null;
				_context = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~LoadingViewHelper()
		{
			Dispose(false);
		}

		#endregion
	}
}