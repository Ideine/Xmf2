﻿using System;
using Android.Runtime;
using Android.Webkit;

namespace Xmf2.Core.Droid.Helpers
{
	public class ProgressWebChromeClient : WebChromeClient
	{
		private int _currentProgress;
		private LoadingViewHelper _loadingView;

		protected ProgressWebChromeClient(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public ProgressWebChromeClient(LoadingViewHelper loadingView)
		{
			_loadingView = loadingView;
			_currentProgress = 0;
		}

		public override void OnProgressChanged(WebView view, int newProgress)
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
