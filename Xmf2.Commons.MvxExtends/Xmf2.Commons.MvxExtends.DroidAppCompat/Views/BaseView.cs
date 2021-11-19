using System;
using Xmf2.Commons.Subscriptions;
using Xmf2.Commons.MvxExtends.ViewModels;
using MvvmCross.Platforms.Android.Views;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Views
{
	public abstract class BaseView<TViewModel, TParameter> : MvxActivity<TViewModel> where TViewModel : BaseViewModel<TParameter> where TParameter : class
	{
		protected Xmf2Disposable Disposable = new Xmf2Disposable();

		public BaseView() { }

		protected BaseView(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

		#region LifeCycle

		protected override void OnStart()
		{
			base.OnStart();
			ViewModel?.OnEnter();
		}

		protected override void OnResume()
		{
			base.OnResume();
			ViewModel?.OnResume();
		}

		protected override void OnPause()
		{
			ViewModel.OnPause();
			base.OnPause();
		}

		protected override void OnStop()
		{
			ViewModel?.OnStop();
			base.OnStop();
		}

		#endregion

		protected override void OnDestroy()
		{
			base.OnDestroy();
			DisposeManagedObjects();
		}

		#region Dispose

		private bool _disposed = false;

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!_disposed)
				{
					if (disposing)
					{
						// Manual release of managed resources.
						DisposeManagedObjects();
					}

					// Release unmanaged resources.
					DisposeUnmanagedObjects();

					_disposed = true;

					base.Dispose(disposing);
				}
			}
			catch { }
		}

		~BaseView()
		{
			Dispose(false);
		}

		protected virtual void DisposeManagedObjects()
		{
			ViewModel?.Dispose();
			Disposable?.Dispose();
			Disposable = null;
		}

		protected virtual void DisposeUnmanagedObjects() { }

		#endregion
	}
}