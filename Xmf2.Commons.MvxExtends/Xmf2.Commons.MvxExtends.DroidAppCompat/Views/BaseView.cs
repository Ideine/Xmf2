using System;
using Xmf2.Commons.MvxExtends.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Views
{
	public abstract class BaseView<TViewModel, TParameter> : MvxAppCompatActivity<TViewModel> where TViewModel : BaseViewModel<TParameter> where TParameter : class
	{
		public BaseView() : base() { }

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
			this.DisposeManagedObjects();
		}

		#region Dispose

		private bool disposed = false;

		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!disposed)
				{
					if (disposing)
					{
						// Manual release of managed resources.
						this.DisposeManagedObjects();
					}
					// Release unmanaged resources.
					this.DisposeUnmanagedObjects();

					disposed = true;

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
			if (this.ViewModel != null)
			{
				this.ViewModel.Dispose();
			}
		}

		protected virtual void DisposeUnmanagedObjects() { }

		#endregion
	}
}