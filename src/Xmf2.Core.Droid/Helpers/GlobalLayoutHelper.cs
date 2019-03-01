using System;
using Android;
using Android.Runtime;
using Android.Views;

namespace Xmf2.Core.Droid.Helpers
{
	public class GlobalLayoutHelper : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
	{
		//TODO VOIR POURQUOI ON A PAS LE ADDONGLOBALLAYOUTLISTENER
		private View _view;
		private Action _action;

		private bool _isDisposed;

		protected GlobalLayoutHelper(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public GlobalLayoutHelper(View view, Action action)
		{
			_view = view;
			_action = action;
		}

		public void OnGlobalLayout()
		{
			if (_isDisposed)
			{
				return;
			}

			this.WrapForDisposedException(() => _action?.Invoke());
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				try
				{
					_view?.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine(e);
				}
				finally
				{
					_isDisposed = true;
					_view = null;
					_action = null;
				}
			}
			base.Dispose(disposing);
		}
	}
}
