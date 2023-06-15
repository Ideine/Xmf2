using System;
using Android.App;
using Android.Views;
using Android.Graphics;
using Android.Runtime;

namespace Xmf2.Core.Droid.Helpers
{
	public class ResizeViewHelper : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
	{
		private View _rootView;
		private View _target;
		private Action<bool> _onKeyBoardVisibilityChanged;

		protected ResizeViewHelper(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public ResizeViewHelper(Activity activity, View target, Action<bool> onKeyBoardVisible = null)
		{
			_rootView = activity.FindViewById(Android.Resource.Id.Content);
			_target = target;
			_onKeyBoardVisibilityChanged = onKeyBoardVisible;
			_rootView?.ViewTreeObserver?.AddOnGlobalLayoutListener(this);
		}

		public void OnGlobalLayout()
		{
			using var r = new Rect();
			try
			{
				_rootView.GetWindowVisibleDisplayFrame(r);

				float keyboardHeight = _rootView.Height - r.Bottom;
				OnKeyboardVisibilityChanged(r.Bottom, keyboardHeight, keyboardHeight > 0);
			}
			catch (Exception)
			{
				//view can be disposed
			}
		}

		private void OnKeyboardVisibilityChanged(float visibleScreenHeight, float keyboardHeight, bool visible)
		{
			if (_target.LayoutParameters is ViewGroup.MarginLayoutParams lp)
			{
				int targetHeight = visible ? (int)visibleScreenHeight : ViewGroup.MarginLayoutParams.MatchParent;
				if (lp.Height != targetHeight)
				{
					lp.Height = targetHeight;
					_target.LayoutParameters = lp;
				}
			}

			_onKeyBoardVisibilityChanged?.Invoke(visible);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_rootView?.ViewTreeObserver?.RemoveOnGlobalLayoutListener(this);

				_rootView?.Dispose();
				_rootView = null;
				_target = null;
				_onKeyBoardVisibilityChanged = null;
			}

			base.Dispose(disposing);
		}
	}
}