using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Plugin.CurrentActivity;

namespace Xmf2.Core.Droid.Helpers
{
	public static class KeyboardHelper
	{
		public static bool IsActive()
		{
			var activity = CrossCurrentActivity.Current.Activity;
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