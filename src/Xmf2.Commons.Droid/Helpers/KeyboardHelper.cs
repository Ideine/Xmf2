using Android.Content;
using Android.Views.InputMethods;
using Android.Widget;

namespace Xmf2.Commons.Droid.Helpers
{
    public static class KeyboardHelper
	{
		public static void HideFrom(EditText intput)
		{
			InputMethodManager inputManager = (InputMethodManager)intput.Context.GetSystemService(Context.InputMethodService);
			inputManager?.HideSoftInputFromWindow(intput.WindowToken, 0);
		}

		public static void Show(EditText intput)
		{
			InputMethodManager inputManager = (InputMethodManager)intput.Context.GetSystemService(Context.InputMethodService);
			intput.RequestFocus();
			inputManager?.ShowSoftInput(intput, ShowFlags.Forced);
		}
	}
}
