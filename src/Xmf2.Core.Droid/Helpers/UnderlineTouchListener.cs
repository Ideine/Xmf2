using System;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xmf2.Core.Droid.Helpers
{
	public class UnderlineTouchListener : Java.Lang.Object, View.IOnTouchListener
	{
		public UnderlineTouchListener() { }
		protected UnderlineTouchListener(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public bool OnTouch(View v, MotionEvent e)
		{
			var cgu = (TextView)v;
			switch (e.Action)
			{
				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
					cgu.PaintFlags = cgu.PaintFlags | PaintFlags.UnderlineText;
					break;
				case MotionEventActions.Down:
					cgu.PaintFlags = cgu.PaintFlags ^ PaintFlags.UnderlineText;
					break;
			}

			return false;
		}
	}
}