using System;
using Android.Text;
using Android.Text.Style;
using Java.Lang;

namespace Android.Widget
{
	public static class TextViewExtensions
	{
		public static void SetTextFromHtml(this TextView textView, string htmlText)
		{
			if (!string.IsNullOrEmpty(htmlText))
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

		public static void Underline(this TextView textView)
		{
			if (textView != null)
			{
				textView.SetUnderlineText(textView.Text?.ToString() ?? string.Empty);
			}
		}

		public static void SetUnderlineText(this TextView textView, string text)
		{
			var spannableText = new SpannableString(text);
			spannableText.SetSpan(new UnderlineSpan(), 0, text.Length, 0);
			textView.SetText(spannableText, TextView.BufferType.Spannable);
		}
	}
}
