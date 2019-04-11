using System;
using UIKit;
using Xmf2.Core.Helpers;
using Xmf2.Core.iOS.Controls;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.iOS.Helpers
{
	public class LoadingViewHelper : IDisposable
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();
		private readonly object _mutex = new object();

		private LoadingEnableHelper _throttling;
		private UILoadingView _loadingView;
		private bool _isBusy;

		private bool _loadingIconUp;

		public virtual bool IsBusy
		{
			get => _isBusy;
			set
			{
				CreateLoadingViewIfNeeded();
				if (_isBusy != value)
				{
					_isBusy = value;
					_throttling.Set(value);
				}
			}
		}

		private UIView _parent;

		public LoadingViewHelper(UIView parent)
		{
			_parent = parent;
			_throttling = new LoadingEnableHelper(v => UIApplication.SharedApplication.BeginInvokeOnMainThread(() => _loadingView?.UpdateViewState(v)))
				.DisposeWith(_disposable);
		}

		private void CreateLoadingViewIfNeeded()
		{
			if (_loadingView != null)
			{
				return;
			}

			lock (_mutex)
			{
				if (_loadingView != null)
				{
					return;
				}
				
				_loadingView = new UILoadingView(_parent, _loadingIconUp).DisposeViewWith(_disposable);
			}
		}

		public void WithLoadingIconUp()
		{
			_loadingIconUp = true;
		}

		#region Dispose

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable.Dispose();

				_parent = null;
				_throttling = null;
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