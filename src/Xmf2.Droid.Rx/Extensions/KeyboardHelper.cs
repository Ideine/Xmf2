using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace Xmf2.Droid.Rx.Extensions
{
	public static class KeyboardHelper
	{
		public static void HideFrom(View input)
		{
			using var inputManager = (InputMethodManager)input.Context.GetSystemService(Context.InputMethodService);
			inputManager?.HideSoftInputFromWindow(input.WindowToken, 0);
		}

		public static void Show(EditText input)
		{
			input.Focusable = true;
			input.FocusableInTouchMode = true;
			input.RequestFocus();

			using var inputManager = (InputMethodManager)input.Context.GetSystemService(Context.InputMethodService);
			inputManager?.ShowSoftInput(input, ShowFlags.Forced);
		}
	}
}
