using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
#if NET7_0_OR_GREATER
using Microsoft.Maui.ApplicationModel;

#else
using Plugin.CurrentActivity;
#endif

namespace Xmf2.Core.Droid.Helpers
{
	public static class KeyboardHelper
	{
		public static bool IsActive()
		{
#if NET7_0_OR_GREATER
			Activity activity = Platform.CurrentActivity!;
#else
			Activity activity = CrossCurrentActivity.Current.Activity;
#endif
			InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
			return inputMethodManager?.IsActive ?? false;
		}

		public static void HideFrom(this View view)
		{
			InputMethodManager inputManager = (InputMethodManager)view.Context!.GetSystemService(Context.InputMethodService);
			inputManager?.HideSoftInputFromWindow(view.WindowToken, 0);
		}

		public static void Show(this EditText input)
		{
			InputMethodManager inputManager = (InputMethodManager)input.Context!.GetSystemService(Context.InputMethodService);
			input.RequestFocus();
			inputManager?.ShowSoftInput(input, ShowFlags.Forced);
		}
	}
}