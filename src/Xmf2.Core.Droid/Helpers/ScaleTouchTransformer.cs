using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xmf2.Core.Droid.Helpers
{
	public class ScaleTouchTransformer : Java.Lang.Object, View.IOnTouchListener
	{
		private View _outerView;
		private readonly float _ratio;

		protected ScaleTouchTransformer(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public ScaleTouchTransformer(float ratio = 0.95f)
		{
			_ratio = ratio;
			_outerView = null;
		}

		public ScaleTouchTransformer(View outerView, float ratio = 0.95f) : this(ratio)
		{
			_outerView = outerView;
		}

		protected virtual void OnUp(View view)
		{
			view.ScaleX = 1f;
			view.ScaleY = 1f;
		}

		protected virtual void OnDown(View view)
		{
			view.ScaleX = _ratio;
			view.ScaleY = _ratio;
		}

		public bool OnTouch(View v, MotionEvent e)
		{
			var view = v;

			if (_outerView != null)
			{
				view = _outerView;
			}

			switch (e.Action)
			{
				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
					OnUp(view);
					break;
				case MotionEventActions.Down:
					OnDown(view);
					break;
			}

			return false;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_outerView = null;
			}

			base.Dispose(disposing);
		}
	}
}