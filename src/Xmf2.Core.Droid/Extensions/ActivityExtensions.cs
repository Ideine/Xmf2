using System;
using Android.App;

namespace Xmf2.Core.Droid.Extensions
{
	public static class ActivityExtensions
	{
		public static void SetFullscreen(this Activity activity, bool isFullScreen)
		{
			if (isFullScreen)
			{
				activity.Window.AddFlags(Android.Views.WindowManagerFlags.Fullscreen);
			}
			else
			{
				activity.Window.ClearFlags(Android.Views.WindowManagerFlags.Fullscreen);
			}
		}

		public static void SetOrientation(this Activity activity, bool portrait)
		{
			if (portrait)
			{
				activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
			}
			else
			{
				activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Landscape;
			}
		}

		public static void UnlockScreenOrientation(this Activity activity)
		{
			activity.RequestedOrientation = Android.Content.PM.ScreenOrientation.FullSensor;
		}
	}
}
