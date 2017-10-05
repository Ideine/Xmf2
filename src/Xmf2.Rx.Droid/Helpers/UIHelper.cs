using System;
using Android.Content;
using Android.Graphics;
using Android.Widget;

namespace Xmf2.Rx.Droid.Helpers
{
	public static class UIHelper
	{
		public static int GetDeviceWidth(Context context)
		{
			var metrics = context.Resources.DisplayMetrics;
			var widthInDp = PxToDp(context, metrics.WidthPixels);
			return widthInDp;
		}

		public static int PxToDp(Context context, float pixelValue)
		{
			var dp = (int)((pixelValue) / context.Resources.DisplayMetrics.Density);
			return dp;
		}

		public static int DpToPx(Context context, float dpValue)
		{
			var px = (int)((dpValue) * context.Resources.DisplayMetrics.Density);
			return px;
		}

		public static float SpToPx(Context context, float px)
		{
			float scaledDensity = context.Resources.DisplayMetrics.ScaledDensity;
			return px * scaledDensity;
		}

		public static void SetColorFilter(ProgressBar view, Color color)
		{
			if (view.Indeterminate)
			{
				view.IndeterminateDrawable.SetColorFilter(color, PorterDuff.Mode.SrcIn);
			}
			else
			{
				view.ProgressDrawable.SetColorFilter(color, PorterDuff.Mode.SrcIn);
			}
		}

		public static Color GetColor(int color, int alpha = 255)
		{
			return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
		}
	}
}
