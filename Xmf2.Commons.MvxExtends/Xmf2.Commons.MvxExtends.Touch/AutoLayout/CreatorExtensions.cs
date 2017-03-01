﻿using System;
using CoreGraphics;
using UIKit;



public static class CreatorExtensions
{
	#region UIButton

	public static UIButton CreateButton(this object parent)
	{
		return new UIButton(UIButtonType.Custom);
	}

	public static TUIButton WithTitle<TUIButton>(this TUIButton button, string title) where TUIButton : UIButton
	{
		button.SetTitle(title, UIControlState.Normal);
		return button;
	}

	public static TUIButton WithTextColor<TUIButton>(this TUIButton button, UIColor color) where TUIButton : UIButton
	{
		button.SetTitleColor(color, UIControlState.Normal);
		return button;
	}

	public static TUIButton WithTextColorHighlight<TUIButton>(this TUIButton button, UIColor color) where TUIButton : UIButton
	{
		button.SetTitleColor(color, UIControlState.Highlighted);
		return button;
	}

	public static TUIButton WithTextColorSelected<TUIButton>(this TUIButton button, UIColor color) where TUIButton : UIButton
	{
		button.SetTitleColor(color, UIControlState.Selected);
		return button;
	}

	public static TUIButton WithImage<TUIButton>(this TUIButton button, string image) where TUIButton : UIButton
	{
		button.SetImage(new UIImage(image), UIControlState.Normal);
		return button;
	}

	public static TUIButton WithImage<TUIButton>(this TUIButton button, UIImage image) where TUIButton : UIButton
	{
		button.SetImage(image, UIControlState.Normal);
		return button;
	}

	public static TUIButton WithImageHighlight<TUIButton>(this TUIButton button, string image) where TUIButton : UIButton
	{
		button.SetImage(new UIImage(image), UIControlState.Highlighted);
		return button;
	}

	public static TUIButton WithImageHighlight<TUIButton>(this TUIButton button, UIImage image) where TUIButton : UIButton
	{
		button.SetImage(image, UIControlState.Highlighted);
		return button;
	}

	public static TUIButton WithImageSelected<TUIButton>(this TUIButton button, string image) where TUIButton : UIButton
	{
		button.SetImage(new UIImage(image), UIControlState.Selected);
		return button;
	}

	public static TUIButton WithImageSelected<TUIButton>(this TUIButton button, UIImage image) where TUIButton : UIButton
	{
		button.SetImage(image, UIControlState.Selected);
		return button;
	}

	public static TUIButton WithImageFocused<TUIButton>(this TUIButton button, string image) where TUIButton : UIButton
	{
		button.SetImage(new UIImage(image), UIControlState.Focused);
		return button;
	}

	public static TUIButton WithImageFocused<TUIButton>(this TUIButton button, UIImage image) where TUIButton : UIButton
	{
		button.SetImage(image, UIControlState.Focused);
		return button;
	}

	public static TUIButton WithFont<TUIButton>(this TUIButton button, UIFont font) where TUIButton : UIButton
	{
		button.Font = font;
		return button;
	}

	public static TUIButton WithSystemFont<TUIButton>(this TUIButton button, int size, UIFontWeight weight = UIFontWeight.Regular) where TUIButton : UIButton
	{
		button.Font = UIFont.SystemFontOfSize(size, weight);
		return button;
	}

	public static TUIButton OnClick<TUIButton>(this TUIButton button, Action action) where TUIButton : UIButton
	{
		button.TouchUpInside += (sender, e) => action?.Invoke();
		return button;
	}

	#endregion

	#region UIView

	public static UIView CreateSeparator(this object parent)
	{
		return new UIView();
	}

	public static UIView CreateView(this object parent)
	{
		return new UIView();
	}

	#endregion

	#region ScrollView

	public static UIScrollView CreateVerticalScroll(this object parent)
	{
		return new UIScrollView
		{
			AlwaysBounceHorizontal = false,
			AlwaysBounceVertical = false,
			Bounces = false,
			BouncesZoom = false,
			ShowsVerticalScrollIndicator = true,
			ShowsHorizontalScrollIndicator = false
		};
	}

	public static UIScrollView Disable(this UIScrollView view)
	{
		view.ScrollEnabled = false;
		return view;
	}

	public static UIScrollView Enable(this UIScrollView view)
	{
		view.ScrollEnabled = true;
		return view;
	}

	#endregion

	#region UILabel

	public static UILabel CreateLabel(this object parent)
	{
		return new UILabel();
	}

	public static UILabel WithText(this UILabel label, string text)
	{
		label.Text = text;
		return label;
	}

	public static UILabel WithTextColor(this UILabel label, UIColor color)
	{
		label.TextColor = color;
		return label;
	}

	public static UILabel WithFont(this UILabel label, UIFont font)
	{
		label.Font = font;
		return label;
	}

	public static UILabel WithAlignment(this UILabel label, UITextAlignment alignment)
	{
		label.TextAlignment = alignment;
		return label;
	}

	public static UILabel WithTextWrapping(this UILabel label, int maxLine = 0)
	{
		label.Lines = maxLine;
		label.LineBreakMode = UILineBreakMode.WordWrap;
		return label;
	}

	public static UILabel WithSystemFont(this UILabel label, int size, UIFontWeight weight = UIFontWeight.Regular)
	{
		label.Font = UIFont.SystemFontOfSize(size, weight);
		return label;
	}

	#endregion UILabel

	#region UITextField

	public static UITextField CreateTextField(this object parent)
	{
		return new UITextField();
	}

	public static UITextField AsPasswordField(this UITextField input, UIReturnKeyType returnKeyType)
	{
		input.KeyboardType = UIKeyboardType.Default;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.ReturnKeyType = returnKeyType;
		input.AutocorrectionType = UITextAutocorrectionType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		input.SecureTextEntry = true;
		return input;
	}

	public static UITextField AsEmailField(this UITextField input, UIReturnKeyType returnKeyType)
	{
		input.KeyboardType = UIKeyboardType.EmailAddress;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.ReturnKeyType = returnKeyType;
		input.AutocorrectionType = UITextAutocorrectionType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}
    public static UITextField AsNumpadField(this UITextField input, UIReturnKeyType returnKeyType)
    {
        input.KeyboardType = UIKeyboardType.NumberPad;
        input.SpellCheckingType = UITextSpellCheckingType.No;
        input.ReturnKeyType = returnKeyType;
        input.AutocorrectionType = UITextAutocorrectionType.No;
        input.AutocapitalizationType = UITextAutocapitalizationType.None;
        return input;
    }

	public static UITextField WithTextColor(this UITextField input, UIColor color)
	{
		input.TextColor = color;
		return input;
	}

	public static UITextField WithText(this UITextField input, string text)
	{
		input.Text = text;
		return input;
	}

	public static UITextField WithFont(this UITextField input, UIFont font)
	{
		input.Font = font;
		return input;
	}

	public static UITextField WithAlignment(this UITextField input, UITextAlignment alignment)
	{
		input.TextAlignment = alignment;
		return input;
	}

	public static UITextField WithSystemFont(this UITextField input, int size, UIFontWeight weight = UIFontWeight.Regular)
	{
		input.Font = UIFont.SystemFontOfSize(size, weight);
		return input;
	}

	public static UITextField WithPlaceholder(this UITextField input, string placeholder)
	{
		input.Placeholder = placeholder;
		return input;
	}

	public static UITextField OnReturn(this UITextField input, Action action, bool dismissKeyboard)
	{
		input.ShouldReturn += (textField) =>
		{
			if (dismissKeyboard)
			{
				textField.ResignFirstResponder();
			}
			action?.Invoke();
			return true;
		};
		return input;
	}

	#endregion UITextField

	#region UIImageView

	public static UIImageView CreateImageView(this object parent)
	{
		return new UIImageView();
	}

	public static UIImageView WithImage(this UIImageView view, UIImage image)
	{
		view.Image = image;
		return view;
	}

	public static UIImageView WithImage(this UIImageView view, string imageName)
	{
		view.Image = new UIImage(imageName);
		return view;
	}

	public static UIImageView UniformToFit(this UIImageView view)
	{
		view.ContentMode = UIViewContentMode.ScaleAspectFit;
		return view;
	}

	#endregion

	#region Generic

	public static TView Hide<TView>(this TView view) where TView : UIView
	{
		view.Hidden = true;
		return view;
	}

	public static TView Show<TView>(this TView view) where TView : UIView
	{
		view.Hidden = false;
		return view;
	}

	public static TView Invisible<TView>(this TView view) where TView : UIView
	{
		view.Alpha = 0f;
		return view;
	}

	public static TView Visible<TView>(this TView view) where TView : UIView
	{
		view.Alpha = 1f;
		return view;
	}

	public static TView WithBackgroundColor<TView>(this TView view, UIColor color) where TView : UIView
	{
		view.BackgroundColor = color;
		return view;
	}

	public static TView WithCornerRadius<TView>(this TView view, int size) where TView : UIView
	{
		view.Layer.CornerRadius = size;
		return view;
	}

	public static TView Disable<TView>(this TView view) where TView : UIView
	{
		view.UserInteractionEnabled = false;
		return view;
	}

	public static TView Enable<TView>(this TView view) where TView : UIView
	{
		view.UserInteractionEnabled = true;
		return view;
	}

	public static TView AddTapAction<TView>(this TView view, Action tapped) where TView : UIView
	{
		UITapGestureRecognizer recognizer = new UITapGestureRecognizer(tapped);
		view.AddGestureRecognizer(recognizer);
		return view;
	}

	#endregion

}

