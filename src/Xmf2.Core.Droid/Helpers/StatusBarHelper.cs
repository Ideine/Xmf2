using Android.OS;
using Android.Views;
using AndroidX.Core.View;

namespace Xmf2.Core.Droid.Helpers
{
	public class StatusBarHelper : Java.Lang.Object, IOnApplyWindowInsetsListener
	{
		public StatusBarHelper(View container)
		{
			ViewCompat.SetOnApplyWindowInsetsListener(container, this);
		}

		public WindowInsetsCompat OnApplyWindowInsets(View v, WindowInsetsCompat insets)
		{
			int left, top, right, bottom;

			if (Build.VERSION.SdkInt >= BuildVersionCodes.R)
			{
				// Marche a partir de Android 11 (API 30)
				var sysInsets = insets.GetInsets(WindowInsets.Type.SystemBars());
				left = sysInsets.Left;
				top = sysInsets.Top;
				right = sysInsets.Right;
				bottom = sysInsets.Bottom;
			}
			else
			{
				// Marche jusqu'a Android 10 (API 29)
				left = insets.SystemWindowInsetLeft;
				top = insets.SystemWindowInsetTop;
				right = insets.SystemWindowInsetRight;
				bottom = insets.SystemWindowInsetBottom;
			}

			v.SetPadding(left, top, right, bottom);

			return insets;
		}
	}
}