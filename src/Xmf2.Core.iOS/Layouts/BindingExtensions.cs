using UIKit;

namespace Xmf2.Core.iOS.Layouts
{
	public static class BindingExtensions
	{
		public static void BindText(this UITextField field, string textValue)
		{
			if (field.Text == textValue)
			{
				return;
			}

			field.Text = textValue;
			if (field.IsFirstResponder)
			{
				field.SelectedTextRange = field.GetTextRange(field.EndOfDocument, field.EndOfDocument);
			}
		}

		public static void BindText(this UITextView field, string textValue)
		{
			if (field.Text == textValue)
			{
				return;
			}

			field.Text = textValue;
			if (field.IsFirstResponder)
			{
				field.SelectedTextRange = field.GetTextRange(field.EndOfDocument, field.EndOfDocument);
			}
		}
	}
}