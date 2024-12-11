using Android.Content;
using Android.Graphics;
using Android.Util;
using AndroidX.Core.Content;

namespace LST.Droid.Helpers
{
	public static class UIHelper
	{
		public static int GetDeviceWidth(Context context)
		{
			DisplayMetrics metrics = context.Resources.DisplayMetrics;
			int widthInDp = PxToDp(context, metrics.WidthPixels);
			return widthInDp;
		}

		public static int PxToDp(Context context, float pixelValue)
		{
			int dp = (int)(pixelValue / context.Resources.DisplayMetrics.Density);
			return dp;
		}

		public static int DpToPx(Context context, float dpValue)
		{
			int px = (int)(dpValue * context.Resources.DisplayMetrics.Density);
			return px;
		}

		public static float SpToPx(Context context, float px)
		{
			float scaledDensity = context.Resources.DisplayMetrics.ScaledDensity;
			return px * scaledDensity;
		}

		public static Color GetColor(int color, int alpha = 255)
		{
			return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
		}

		public static Color GetColor(Context context, int colorId)
		{
			return new Color(ContextCompat.GetColor(context, colorId));
		}

		/// <summary>
		/// Get Id of drawable by the name of the resource
		/// </summary>
		/// <returns>Id of the resource</returns>
		public static int GetDrawableByName(this Context context, string name)
		{
			return context.Resources!.GetIdentifier(name, "drawable", context.PackageName);
		}
	}
}