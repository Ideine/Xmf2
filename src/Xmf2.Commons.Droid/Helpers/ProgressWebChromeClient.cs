using System;
using Android.Runtime;

namespace Xmf2.Commons.Droid.Helpers
{
    public class ProgressWebChromeClient : Android.Webkit.WebChromeClient
	{
		private int _currentProgress;
		private LoadingViewHelper _loadingView;

		protected ProgressWebChromeClient(IntPtr handle, JniHandleOwnership transer) : base(handle, transer) { }

		public ProgressWebChromeClient(LoadingViewHelper loadingView)
		{
			_loadingView = loadingView;
			_currentProgress = 0;
		}

		public override void OnProgressChanged(Android.Webkit.WebView view, int newProgress)
		{
			try
			{
				if (newProgress > _currentProgress && !_loadingView.IsBusy)
				{
					_loadingView.IsBusy = true;
				}
				if (newProgress == 100)
				{
					_loadingView.IsBusy = false;
				}
				_currentProgress = newProgress;
			}
			catch (NullReferenceException e)
			{
				System.Diagnostics.Debug.WriteLine(e);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_loadingView = null;
			}
			base.Dispose(disposing);

		}
	}
}
