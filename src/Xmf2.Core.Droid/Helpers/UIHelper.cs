﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;

namespace Xmf2.Core.Droid.Helpers
{
	public static class UIHelper
	{
		public static void SetTranslucentStatusBar(this Activity activity)
		{
			void SetWindowFlag(WindowManagerFlags flags, bool activate)
			{
				Window win = activity.Window;
				WindowManagerLayoutParams winParams = win.Attributes;

				if (activate)
				{
					winParams.Flags |= flags;
				}
				else
				{
					winParams.Flags = flags;
				}

				win.Attributes = winParams;
			}

			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				SetWindowFlag(WindowManagerFlags.TranslucentStatus, activate: false);
				activity.Window.SetStatusBarColor(Color.Transparent);
				activity.Window.DecorView.SystemUiVisibility = StatusBarVisibility.Visible;
			}
		}

		public static int GetDeviceWidth(Context context)
		{
			var metrics = context.Resources.DisplayMetrics;
			var widthInDp = PxToDp(context, metrics.WidthPixels);
			return widthInDp;
		}

		public static Color ColorFromHex(this uint color, int? alpha = null)
		{
			byte a = (byte)(color >> 24);
			byte r = (byte)(color >> 16);
			byte g = (byte)(color >> 8);
			byte b = (byte)(color >> 0);
			return Color.Argb(alpha ?? a, r, g, b);
		}

		public static int PxToDp(Context context, float pixelValue)
		{
			return (int)(pixelValue / context.Resources.DisplayMetrics.Density);
		}

		public static int DpToPx(Context context, float dpValue)
		{
			return (int)(dpValue * context.Resources.DisplayMetrics.Density);
		}

		public static float SpToPx(Context context, float spValue)
		{
			return spValue * context.Resources.DisplayMetrics.ScaledDensity;
		}

		public static void SetColorFilter(ProgressBar view, Color color)
		{
			if (view == null)
			{
				return;
			}

			if (view.Indeterminate)
			{
				view.IndeterminateDrawable.SetColorFilter(new PorterDuffColorFilter(color, PorterDuff.Mode.SrcIn));
			}
			else
			{
				view.ProgressDrawable.SetColorFilter(new PorterDuffColorFilter(color, PorterDuff.Mode.SrcIn));
			}
		}

		public static Color GetColor(int color, int alpha = 255)
		{
			return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
		}

		public static Color GetColor(Context context, int resId)
		{
			return new Color(ContextCompat.GetColor(context, resId));
		}

		public static int GetPxDimension(Context context, int resId)
		{
			return context.Resources.GetDimensionPixelSize(resId);
		}

		public static void SetVisibleOrGone(this View view, bool visible)
		{
			view.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
		}

		public static void SetVisibleOrInvisible(this View view, bool visible)
		{
			view.Visibility = visible ? ViewStates.Visible : ViewStates.Invisible;
		}
	}
}