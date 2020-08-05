using MvvmCross.Platforms.Android.Views;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Droid.Views
{
	public abstract class BaseView<TViewModel, TParameter> : MvxActivity<TViewModel> where TViewModel : BaseViewModel<TParameter>
	{
		protected override void OnDestroy()
		{
			base.OnDestroy();
			DisposeManagedObjects();
		}

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

		~BaseView()
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