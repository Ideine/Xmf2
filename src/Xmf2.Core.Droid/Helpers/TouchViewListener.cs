﻿using System;
using Android.Views;
using Android.Runtime;

namespace Xmf2.Core.Droid.Helpers
{
	public class TouchViewListener<TView> : Java.Lang.Object, View.IOnTouchListener where TView : View
	{
		public TView View { get; private set; }

		private Action<TView> _fromHighlight;
		private Action<TView> _toHighlight;

		public TouchViewListener(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public TouchViewListener(TView view)
		{
			View = view;
			View?.SetOnTouchListener(this);
		}

		public TouchViewListener<TView> WithTouchHighlight(Action<TView> fromHighlight, Action<TView> toHighlight)
		{
			_fromHighlight = fromHighlight;
			_toHighlight = toHighlight;

			return this;
		}

		public bool OnTouch(View v, MotionEvent e)
		{
			switch (e.Action)
			{
				case MotionEventActions.Up:
				case MotionEventActions.Cancel:
					OnUp(v);
					break;
				case MotionEventActions.Down:
					OnDown(v);
					break;
			}
			return false;
		}

		protected void OnUp(View view) => _fromHighlight?.Invoke((TView)view);

		protected void OnDown(View view) => _toHighlight?.Invoke((TView)view);


		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (View != null)
				{
					View.SetOnTouchListener(null);
					View = null;
				}
				_fromHighlight = null;
				_toHighlight = null;
			}
			base.Dispose(disposing);
		}
	}
}
