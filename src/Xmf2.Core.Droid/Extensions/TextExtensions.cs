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
				textView.TextFormatted = Html.FromHtml(htmlText, Html.FromHtmlModeCompact);
			}
			else
			{
				textView.TextFormatted = Html.FromHtml(htmlText);
			}
			if (linkifyText)
			{
				textView.MovementMethod = LinkMovementMethod.Instance;
			}
		}
	}
}
