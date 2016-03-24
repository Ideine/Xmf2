using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xmf2.Commons.MvxExtends.ViewModels;
using MvvmCross.Droid.Views;

namespace Xmf2.Commons.MvxExtends.Droid.Views
{
    public abstract class BaseView<TViewModel> : MvxActivity<TViewModel> where TViewModel : BaseViewModel
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
                this.ViewModel.Dispose();
        }

        protected virtual void DisposeUnmanagedObjects()
        { }

        #endregion
    }
}