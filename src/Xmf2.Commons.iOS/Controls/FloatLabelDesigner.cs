using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public static class FloatLabelDesigner
	{
		public static FloatLabeledTextField SetupFloatingLabelLightTheme(this FloatLabeledTextField input, string placeholder, UIResponder next = null, Action onEnterCallback = null)
		{
			UIColor placeholderColor = UIColor.FromRGBA(0f, 0f, 0f, 0.5f);
			UIColor textColor = UIColor.Black;
			UIFont placeholderFont = UIFont.SystemFontOfSize(12);
			UIFont textFont = UIFont.SystemFontOfSize(14);

			var floatLabel = FloatLabeledTextField.CreateOnFirstCharTextField(textColor, textFont, placeholder, textColor, placeholderColor, placeholderFont);
			floatLabel.OnReturnNextResponder(next, onEnterCallback)
					  .WithReturnKey(next != null ? UIReturnKeyType.Next : UIReturnKeyType.Done);
			return floatLabel;
		}

		public static FloatLabeledTextField SetupFloatingLabelDarkTheme(this FloatLabeledTextField input, string placeholder, UIResponder next = null, Action onEnterCallback = null)
		{
			UIColor placeholderColor = UIColor.FromRGBA(1f, 1f, 1f, 0.5f);
			UIColor textColor = UIColor.White;
			UIFont placeholderFont = UIFont.SystemFontOfSize(12);
			UIFont textFont = UIFont.SystemFontOfSize(14);

			var floatLabel = FloatLabeledTextField.CreateOnFirstCharTextField(textColor, textFont, placeholder, textColor, placeholderColor, placeholderFont);
			floatLabel.OnReturnNextResponder(next, onEnterCallback)
					  .WithReturnKey(next != null ? UIReturnKeyType.Next : UIReturnKeyType.Done);
			return floatLabel;
		}

		public static FloatLabeledTextField SetupFloatingLabelLightThemeDisabled(this FloatLabeledTextField input, string placeholder)
		{
			UIColor placeholderColor = UIColor.FromRGBA(0f, 0f, 0f, 0.5f);
			UIColor textColor = UIColor.Black;
			UIFont placeholderFont = UIFont.SystemFontOfSize(12);
			UIFont textFont = UIFont.SystemFontOfSize(14);

			var floatLabel = FloatLabeledTextField.CreateOnFirstCharTextField(textColor, textFont, placeholder, textColor, placeholderColor, placeholderFont);
			floatLabel.Enabled = false;
			return floatLabel;
		}

		public static FloatLabeledTextField SetupFloatingLabelDarkThemeDisabled(this FloatLabeledTextField input, string placeholder)
		{
			UIColor placeholderColor = UIColor.FromRGBA(1f, 1f, 1f, 0.5f);
			UIColor textColor = UIColor.White;
			UIFont placeholderFont = UIFont.SystemFontOfSize(12);
			UIFont textFont = UIFont.SystemFontOfSize(14);

			var floatLabel = FloatLabeledTextField.CreateOnFirstCharTextField(textColor, textFont, placeholder, textColor, placeholderColor, placeholderFont);
			floatLabel.Enabled = false;
			return floatLabel;
		}
	}
}
