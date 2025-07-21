using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using AndroidX.Core.Graphics;
using AndroidX.Core.View;

namespace Xmf2.Core.Droid.Helpers
{
	public class StatusBarHelper : Java.Lang.Object, IOnApplyWindowInsetsListener
	{
		private static int _topPadding;
		private Context _context;
		private View _container;
		private readonly int _paddingTopInDp;

		public StatusBarHelper(Context context, View container, int paddingTopInDp = 12)
		{
			_context = context;
			_container = container;
			_paddingTopInDp = paddingTopInDp;
			SetPaddingTopOnContainer();
			ViewCompat.SetOnApplyWindowInsetsListener(container, this);
		}

		protected StatusBarHelper(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		private void SetPaddingTopOnContainer()
		{
			_container.SetPadding(_container.PaddingStart, _topPadding, _container.PaddingEnd, _container.PaddingBottom);
		}

		public WindowInsetsCompat OnApplyWindowInsets(View v, WindowInsetsCompat insets)
		{
			if (_context != null && _container != null)
			{
				Insets statusBarInset = insets.GetInsets(WindowInsetsCompat.Type.StatusBars());
				Insets displayCutoutInset = insets.GetInsets(WindowInsetsCompat.Type.DisplayCutout());
				int paddingTop = UIHelper.DpToPx(_context, _paddingTopInDp) + Math.Max(statusBarInset.Top, displayCutoutInset.Top);
				_topPadding = paddingTop;
				SetPaddingTopOnContainer();
			}

			return insets;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_context = null;
				_container = null;
			}

			base.Dispose(disposing);
		}
	}
}