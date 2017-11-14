using System;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Widget;

namespace Xmf2.Commons.Droid.Helpers
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
			return (int)((pixelValue) / context.Resources.DisplayMetrics.Density);
		}

		public static int DpToPx(Context context, float dpValue)
		{
			return (int)((dpValue) * context.Resources.DisplayMetrics.Density);
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

		public static Color GetColor(Context context, int resId)
		{
			return new Color(ContextCompat.GetColor(context, resId));
		}
	}
}
