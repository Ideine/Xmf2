using System;
using UIKit;
using Xmf2.Core.Helpers;
using Xmf2.Core.iOS.Controls;
using Xmf2.Core.Subscriptions;
using Xmf2.Core.iOS.Extensions;
using Xmf2.iOS.Extensions.Extensions;

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
		private UIActivityIndicatorViewStyle _style;
		private UIColor _backgroundColor;
		private UIView _parent;

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
		
		public LoadingViewHelper(UIView parent)
		{
			_parent = parent;
			_throttling = new LoadingEnableHelper(v => UIApplication.SharedApplication.BeginInvokeOnMainThread(() => _loadingView?.UpdateViewState(v)))
				.DisposeWith(_disposable);
		}

		private void CreateLoadingViewIfNeeded()
		{
			if (_loadingView is null)
			{
				lock (_mutex)
				{
					if (_loadingView is null)
					{
						_loadingView = new UILoadingView(_parent, _loadingIconUp)
							.DisposeViewWith(_disposable)
							.WithActivityIndicatorViewStyle(_style);
						if (_backgroundColor != null)
						{
							_loadingView.WithBackgroundColor(_backgroundColor);
						}
					}
				}
			}
		}

		public LoadingViewHelper WithLoadingIconUp()
		{
			_loadingIconUp = true;
			return this;
		}
		public LoadingViewHelper WithActivityIndicatorViewStyle(UIActivityIndicatorViewStyle style)
		{
			_style = style;
			_loadingView?.WithActivityIndicatorViewStyle(_style);
			return this;
		}
		public LoadingViewHelper WithBackgroundColor(uint color)
		{
			_backgroundColor = color.ColorFromHex();
			_loadingView?.WithBackgroundColor(_backgroundColor);
			return this;
		}
		public LoadingViewHelper WithBackgroundColor(UIColor color)
		{
			_backgroundColor = color;
			_loadingView?.WithBackgroundColor(_backgroundColor);
			return this;
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