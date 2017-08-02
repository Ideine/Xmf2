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
            string clearImage = null;
            bool isEditable = true;

            return SetupFloatingLabel(input, placeholder, next, placeholderColor, textColor, placeholderFont, textFont, clearImage, isEditable, onEnterCallback);
        }

        public static FloatLabeledTextField SetupFloatingLabelDarkTheme(this FloatLabeledTextField input, string placeholder, UIResponder next = null, Action onEnterCallback = null)
        {
            UIColor placeholderColor = UIColor.FromRGBA(1f, 1f, 1f, 0.5f);
            UIColor textColor = UIColor.White;
            UIFont placeholderFont = UIFont.SystemFontOfSize(12);
            UIFont textFont = UIFont.SystemFontOfSize(14);
            string clearImage = null;
            bool isEditable = true;

            return SetupFloatingLabel(input, placeholder, next, placeholderColor, textColor, placeholderFont, textFont, clearImage, isEditable, onEnterCallback);
        }

        public static FloatLabeledTextField SetupFloatingLabelLightThemeDisabled(this FloatLabeledTextField input, string placeholder)
        {
            UIColor placeholderColor = UIColor.FromRGBA(0f, 0f, 0f, 0.5f);
            UIColor textColor = UIColor.Black;
            UIFont placeholderFont = UIFont.SystemFontOfSize(12);
            UIFont textFont = UIFont.SystemFontOfSize(14);
            string clearImage = null;
            bool isEditable = false;

            return SetupFloatingLabel(input, placeholder, null, placeholderColor, textColor, placeholderFont, textFont, clearImage, isEditable, null);
        }

        public static FloatLabeledTextField SetupFloatingLabelDarkThemeDisabled(this FloatLabeledTextField input, string placeholder)
        {
            UIColor placeholderColor = UIColor.FromRGBA(1f, 1f, 1f, 0.5f);
            UIColor textColor = UIColor.White;
            UIFont placeholderFont = UIFont.SystemFontOfSize(12);
            UIFont textFont = UIFont.SystemFontOfSize(14);
            string clearImage = null;
            bool isEditable = false;

            return SetupFloatingLabel(input, placeholder, null, placeholderColor, textColor, placeholderFont, textFont, clearImage, isEditable, null);
        }

        public static FloatLabeledTextField SetupFloatingLabel(this FloatLabeledTextField input, string placeholder, UIResponder next, UIColor placeholderColor, UIColor textColor, UIFont floatingLabelFont, UIFont textFont, string clearImage, bool isEditable, Action onEnterCallback = null)
        {
            if (input == null)
            {
                return input;
            }

            //placeholder
            input.Placeholder = placeholder;
            UIStringAttributes firstAttributes = new UIStringAttributes
            {
                ForegroundColor = placeholderColor,
                Font = floatingLabelFont
            };
            NSMutableAttributedString ph = new NSMutableAttributedString(placeholder);
            ph.SetAttributes(firstAttributes.Dictionary, new NSRange(0, placeholder.Length));
            input.AttributedPlaceholder = ph;

            input.FloatingLabelActiveTextColor = placeholderColor;
            input.FloatingLabelFont = floatingLabelFont;
            input.FloatingLabelTextColor = placeholderColor;
            input.TextColor = textColor;
            input.Font = textFont;
            input.TintColor = textColor;
            input.UserInteractionEnabled = isEditable;
            input.ClearButtonMode = UITextFieldViewMode.Never;

            if (isEditable)
            {
                if (next != null)
                {
                    input.ShouldReturn += (textField) =>
                    {
                        onEnterCallback?.Invoke();
                        return NextFirstResponder(next);
                    };
                }
                else
                {
                    input.ShouldReturn += (textField) =>
                    {
                        input.EndEditing(true);
                        onEnterCallback?.Invoke();
                        return true;
                    };
                }

                const int clearSize = 30;
                const int height = 40;

                UIButton button = new UIButton(new CGRect(0, height / 2 - clearSize / 2, clearSize, clearSize));

                if (clearImage != null)
                {
                    button.SetImage(UIImage.FromFile(clearImage), UIControlState.Normal);
                }

                button.TouchUpInside += (sender, e) => input.ClearText();
                input.Enabled = true;
                input.RightViewMode = UITextFieldViewMode.Always;
                input.RightView = button;
            }
            else
            {
                input.RightViewMode = UITextFieldViewMode.Never;
            }
			return input;
        }

        private static bool NextFirstResponder(UIResponder nextReponder)
        {
            if (nextReponder == null)
            {
                return false;
            }
            nextReponder.BecomeFirstResponder();
            return true;
        }
    }
}
