using MvvmCross.Droid.Views;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.Droid.Views
{
	public abstract class BaseView<TViewModel, TParmeter> : MvxActivity<TViewModel>
		where TViewModel : BaseViewModel<TParmeter> where TParmeter : class
	{
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