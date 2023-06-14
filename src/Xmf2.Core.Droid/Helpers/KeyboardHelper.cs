using Android.Content;
using Android.Views.InputMethods;
using Android.Widget;

namespace Xmf2.Core.Droid.Helpers
{
	public static class KeyboardHelper
	{
		public static bool IsActive()
		{
			var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
			InputMethodManager inputMethodManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
			return inputMethodManager?.IsActive ?? false;
		}

		public static void HideFrom(this EditText input)
		{
			InputMethodManager inputManager = (InputMethodManager)input.Context.GetSystemService(Context.InputMethodService);
			inputManager?.HideSoftInputFromWindow(input.WindowToken, 0);
		}

		public static void Show(this EditText input)
		{
			InputMethodManager inputManager = (InputMethodManager)input.Context.GetSystemService(Context.InputMethodService);
			input.RequestFocus();
			inputManager?.ShowSoftInput(input, ShowFlags.Forced);
		}
	}
}