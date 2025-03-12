using Android.OS;
using Android.Text;
using Android.Widget;
using Android.Text.Method;

namespace Xmf2.Core.Droid.Extensions
{
	public static class TextExtensions
	{
		public static void SetHtmlText(this TextView textView, string htmlText, bool linkifyText = false)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
			{
				textView.TextFormatted = Html.FromHtml(htmlText, FromHtmlOptions.ModeCompact);
			}
			else
			{
#pragma warning disable 618
				textView.TextFormatted = Html.FromHtml(htmlText);
#pragma warning restore 618
			}

			if (linkifyText)
			{
				textView.MovementMethod = LinkMovementMethod.Instance;
			}
		}

		public static void BindText(this EditText editText, string text)
		{
			if (editText != null && editText.Text != text)
			{
				editText.Text = text;
				editText.SetSelection(text?.Length ?? 0);
			}
		}
	}
}