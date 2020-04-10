using Android.Content;
using Android.Views.InputMethods;
using Android.Widget;

namespace Xmf2.Core.Droid.Helpers
{
	public static class KeyboardHelper
	{
		public static void HideFrom(this EditText intput)
		{
			InputMethodManager inputManager = (InputMethodManager)intput.Context.GetSystemService(Context.InputMethodService);
			inputManager?.HideSoftInputFromWindow(intput.WindowToken, 0);
		}

		public static void Show(this EditText intput)
		{
			InputMethodManager inputManager = (InputMethodManager)intput.Context.GetSystemService(Context.InputMethodService);
			intput.RequestFocus();
			inputManager?.ShowSoftInput(intput, ShowFlags.Forced);
		}
	}
}
