using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;

namespace Xmf2.Core.Droid.Extensions
{
	public static class ActivityExtensions
	{
		public static void SetFullscreen(this Activity activity, bool isFullScreen)
		{
			if (isFullScreen)
			{
				activity.Window.AddFlags(WindowManagerFlags.Fullscreen);
			}
			else
			{
				activity.Window.ClearFlags(WindowManagerFlags.Fullscreen);
			}


			if (Build.VERSION.SdkInt > BuildVersionCodes.Kitkat)
			{
				WindowCompat.SetDecorFitsSystemWindows(activity.Window!, !isFullScreen);

				var controllerCompat = new WindowInsetsControllerCompat(activity.Window!, activity.Window!.DecorView);
				if (isFullScreen)
				{
					controllerCompat.Hide(WindowInsetsCompat.Type.SystemBars() | WindowInsetsCompat.Type.StatusBars() | WindowInsetsCompat.Type.NavigationBars());
				}
				else
				{
					controllerCompat.Show(WindowInsetsCompat.Type.SystemBars() | WindowInsetsCompat.Type.StatusBars() | WindowInsetsCompat.Type.NavigationBars());
				}

				controllerCompat.SystemBarsBehavior = WindowInsetsControllerCompat.BehaviorShowTransientBarsBySwipe;
			}
		}

		public static void SetOrientation(this Activity activity, bool portrait)
		{
			activity.RequestedOrientation = portrait ? ScreenOrientation.Portrait : ScreenOrientation.Landscape;
		}

		public static void UnlockScreenOrientation(this Activity activity)
		{
			activity.RequestedOrientation = ScreenOrientation.FullSensor;
		}
	}
}