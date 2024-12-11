using System;
using Android.App;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xmf2.Droid.Rx.Extensions;

namespace Xmf2.Droid.Rx.ViewHandlers
{
	public class ExtendedViewHandler : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
	{
		private Activity _activity;

		private View _rootView;

		private ScrollView _scrollView;
		private readonly int _defaultBottomPadding;

		public ExtendedViewHandler(Activity activity, ScrollView scrollContent)
		{
			_activity = activity;

			_rootView = activity.FindViewById(Android.Resource.Id.Content);
			_scrollView = scrollContent;

			_defaultBottomPadding = _scrollView.PaddingBottom;
			_rootView?.ViewTreeObserver?.AddOnGlobalLayoutListener(this);
		}

		protected ExtendedViewHandler(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public void OnGlobalLayout()
		{
			using var r = new Rect();
			_rootView.GetWindowVisibleDisplayFrame(r);

			int screenHeight = _rootView.Height;
			int keyboardHeight = screenHeight - (r.Bottom - r.Top);

			OnKeyboardVisibilityChanged(keyboardHeight, keyboardHeight > 0);
		}

		private void OnKeyboardVisibilityChanged(int keyboardHeight, bool visible)
		{
			int keyboardPadding = visible ? keyboardHeight : _defaultBottomPadding;
			_scrollView.SetPadding(_scrollView.PaddingLeft, _scrollView.PaddingTop, _scrollView.PaddingRight, keyboardPadding);

			if (visible)
			{
				if (_activity.CurrentFocus is EditText inputView)
				{
					int distance = GetDistanceToTop(inputView);
					_scrollView.SmoothScrollTo(0, distance);
				}
			}
		}

		protected virtual int GetDistanceToTop(EditText inputView)
		{
			//TODO TABLET
			return inputView.GetDistanceTopToTop<ScrollView>();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_rootView?.ViewTreeObserver?.RemoveOnGlobalLayoutListener(this);
				_rootView?.Dispose();
				_rootView = null;
				_activity = null;
				_scrollView = null;
			}

			base.Dispose(disposing);
		}
	}
}