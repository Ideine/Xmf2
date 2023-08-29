using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Xmf2.Core.Subscriptions;
using AlertDialog = AndroidX.AppCompat.App.AlertDialog;
#if NET7_0_OR_GREATER
using Microsoft.Maui.ApplicationModel;
#else
using Plugin.CurrentActivity;
#endif

namespace Xmf2.Core.Droid.Dialogs
{
	public class BaseDialog : AlertDialog
	{
		protected Xmf2Disposable Disposable = new Xmf2Disposable();

		protected BaseDialog(Context context) : base(context) { }
		protected BaseDialog(Context context, int themeResId) : base(context, themeResId) { }
		protected BaseDialog(IntPtr javaRef, JniHandleOwnership transfer) : base(javaRef, transfer) { }

		protected void DoAction(Action action)
		{
			ForceDismiss();
			action?.Invoke();
		}

		private void ApplySize(float widthRatio, float heightRatio)
		{
#if NET7_0_OR_GREATER
			Activity currentActivity = Platform.CurrentActivity!;
#else
			Activity currentActivity = CrossCurrentActivity.Current.Activity;
#endif

			if (currentActivity != null && !currentActivity.IsFinishing)
			{
				var display = currentActivity.WindowManager.DefaultDisplay;
				var size = new Point();
				display.GetSize(size);
				var width = (int)(size.X * widthRatio);
				var height = (int)(size.Y * heightRatio);
				Window.SetLayout(width, height);
			}
		}

		protected void StretchContent() => ApplySize(1f, 1f);

		public void ForceDismiss()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
			Dismiss();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Disposable?.Dispose();
				Disposable = null;
			}

			base.Dispose(disposing);
		}
	}
}