using System;
using MvvmCross.Platforms.Android.Views;
using Xmf2.Commons.MvxExtends.ViewModels;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Views
{
    public abstract class BaseView<TViewModel, TParameter> : MvxActivity<TViewModel> where TViewModel : BaseViewModel<TParameter> where TParameter : class
    {
        public BaseView() : base() { }

        protected BaseView(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

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
                this.ViewModel.Dispose();
        }

        protected virtual void DisposeUnmanagedObjects()
        { }

        #endregion
    }
}