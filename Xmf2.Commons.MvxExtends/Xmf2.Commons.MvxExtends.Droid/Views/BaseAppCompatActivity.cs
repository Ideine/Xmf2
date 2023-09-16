using System;
using Android.Runtime;
using MvvmCross.Platforms.Android.Views;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Droid.Views
{
	public abstract class BaseAppCompatActivity<TViewModel, TParameter> : MvxActivity<TViewModel> where TViewModel : BaseViewModel<TParameter>
	{
		protected override void OnDestroy()
		{
			base.OnDestroy();
			DisposeManagedObjects();
		}

		protected BaseAppCompatActivity(IntPtr ptr, JniHandleOwnership ownership) : base(ptr, ownership) { }

		protected BaseAppCompatActivity() { }

		#region Dispose

		private bool _disposed;

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

		~BaseAppCompatActivity()
		{
			Dispose(false);
		}

		protected virtual void DisposeManagedObjects()
		{
			ViewModel?.Dispose();
		}

		protected virtual void DisposeUnmanagedObjects() { }

		#endregion
	}
}