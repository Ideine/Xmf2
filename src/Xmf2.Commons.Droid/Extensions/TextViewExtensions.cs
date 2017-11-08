using System;
using Android.Text;
using Java.Lang;

namespace Android.Widget
{
	public static class TextViewExtensions
	{
		public static void SetTextFromHtml(this TextView textView, string htmlText)
		{
			ICharSequence text = null;
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.N)
			{
				text = Html.FromHtml(htmlText, FromHtmlOptions.ModeCompact);
			}
			else
			{
				text = Html.FromHtml(htmlText);
			}
			textView.SetText(text, TextView.BufferType.Spannable);
		}
	}
}
