using System;
using CoreGraphics;
using UIKit;
using Xmf2.Commons.iOS.Controls;
using System.Runtime.CompilerServices;

public static class CreatorExtensions
{
	#region UIButton

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIButton CreateButton(this UIResponder _)
    {
        return new UIButton(UIButtonType.Custom);
    }

    public static UIButton CreateButton(this UIResponder parent, UIButtonType type)
    {
        return new UIButton(type);
    }

    public static UIHighlightButton CreateHighlightButton(this UIResponder parent)
    {
        return new UIHighlightButton();
    }

	public static UISwappedImageButton CreateSwappedImageButton(this UIResponder _)
	{
		return new UISwappedImageButton();
	}

	public static TUIButton WithHighlightBackgroundColor<TUIButton>(this TUIButton button, int color) where TUIButton : UIHighlightButton
	{
		button.HighlightColor = color.ColorFromHex();
		return button;
	}
	public static TUIButton WithHighlightBackgroundColor<TUIButton>(this TUIButton button, UIColor color) where TUIButton : UIHighlightButton
    {
        button.HighlightColor = color;
        return button;
    }

	public static TUIButton WithTitle<TUIButton>(this TUIButton button, string title) where TUIButton : UIButton
	{
		button.SetTitle(title, UIControlState.Normal);
		return button;
	}

	public static TUIButton WithTextColor<TUIButton>(this TUIButton button, int color, UIControlState forState) where TUIButton : UIButton
	{
		button.SetTitleColor(color.ColorFromHex(), forState);
		return button;
	}

	public static TUIButton WithTextColor<TUIButton>(this TUIButton button, int color) where TUIButton : UIButton
	{
		button.SetTitleColor(color.ColorFromHex(), UIControlState.Normal);
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
        button.WithImage(image, UIControlState.Normal);
        return button;
	}
	public static TUIButton WithImage<TUIButton>(this TUIButton button, string image, UIControlState state) where TUIButton : UIButton
	{
		if (String.IsNullOrEmpty(image))
		{
			button.SetImage(null, state);
		}
		else
		{
			button.SetImage(new UIImage(image), state);
		}
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

	public static TUIButton OnClick<TUIButton>(this TUIButton button, Action<TUIButton, EventArgs> action) where TUIButton : UIButton
	{
		button.TouchUpInside += (sender, e) => action?.Invoke((TUIButton)sender, e);
		return button;
	}

	public static TUIButton OnClick<TUIButton>(this TUIButton button, Action action, out Action unregisterAction) where TUIButton : UIButton
    {
        EventHandler onTouchUpInside = (sender, e) => action?.Invoke();
        button.TouchUpInside += onTouchUpInside;
        unregisterAction = () =>
        {
            button.TouchUpInside -= onTouchUpInside;
        };
        return button;
    }

	#endregion

	#region UIView

	public static UIView CreateSeparator(this UIResponder parent)
	{
		return new UIView();
	}

	public static UIView CreateSeparator(this UIResponder parent, UIColor backgroundColor)
	{
		return new UIView().WithBackgroundColor(backgroundColor);
	}

    public static UIView CreateView(this UIResponder parent)
    {
        return new UIView();
    }

	public static TParentView WithSubviews<TParentView>(this TParentView parentView, UIView view) where TParentView : UIView
	{
		parentView.AddSubview(view);
		return parentView;
	}
	public static TParentView WithSubviews<TParentView>(this TParentView parentView, UIView view1, UIView view2) where TParentView : UIView
	{
		parentView.AddSubviews(view1, view2);
		return parentView;
	}
	public static TParentView WithSubviews<TParentView>(this TParentView parentView, UIView view1, UIView view2, UIView view3) where TParentView : UIView
	{
		parentView.AddSubviews(view1, view2, view3);
		return parentView;
	}
	public static TParentView WithSubviews<TParentView>(this TParentView parentView, UIView view1, UIView view2, UIView view3, UIView view4) where TParentView : UIView
	{
		parentView.AddSubviews(view1, view2, view3, view4);
		return parentView;
	}
	public static TParentView WithSubviews<TParentView>(this TParentView parentView, UIView view1, UIView view2, UIView view3, UIView view4, UIView view5) where TParentView : UIView
	{
		parentView.AddSubviews(view1, view2, view3, view4, view5);
		return parentView;
	}
	public static TParentView WithSubviews<TParentView>(this TParentView parentView, UIView view1, UIView view2, UIView view3, UIView view4, UIView view5, UIView view6) where TParentView : UIView
	{
		parentView.AddSubviews(view1, view2, view3, view4, view5, view6);
		return parentView;
	}
	public static TParentView WithSubviews<TParentView>(this TParentView parentView, UIView view1, UIView view2, UIView view3, UIView view4, UIView view5, UIView view6, params UIView[] views) where TParentView : UIView
	{
		parentView.AddSubviews(view1, view2, view3, view4, view5, view6);
		parentView.AddSubviews(views);
		return parentView;
	}

	#endregion

	#region ScrollView

	public static UIScrollView CreateVerticalScroll(this UIResponder _)
    {
        return new UIScrollView()
        {
            AlwaysBounceHorizontal = false,
            AlwaysBounceVertical = false,
            Bounces = false,
            BouncesZoom = false,
            ShowsVerticalScrollIndicator = true,
            ShowsHorizontalScrollIndicator = false
        }.WithContentInsetAdjustementBehavior(UIScrollViewContentInsetAdjustmentBehavior.Never);
	}

	public static UIScrollView CreateHorizontalScroll(this UIResponder _)
	{
		return new UIScrollView()
		{
			AlwaysBounceHorizontal = false,
			AlwaysBounceVertical = false,
			Bounces = false,
			BouncesZoom = false,
			ShowsVerticalScrollIndicator = false,
			ShowsHorizontalScrollIndicator = true
		}.WithContentInsetAdjustementBehavior(UIScrollViewContentInsetAdjustmentBehavior.Never);
	}

	public static UIScrollView WithContentInsetAdjustementBehavior(this UIScrollView view, UIScrollViewContentInsetAdjustmentBehavior behavior)
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
		{
			view.ContentInsetAdjustmentBehavior = behavior;
		}
		return view;
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

    public static UILabel CreateLabel(this UIResponder parent)
    {
        return new UILabel();
    }

    public static UILabel WithText(this UILabel label, string text)
    {
        label.Text = text;
        return label;
    }

	public static UILabel WithTextColor(this UILabel label, int color)
	{
		return WithTextColor(label, color.ColorFromHex());
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

    public static UILabel WithEllipsis(this UILabel label)
    {
        label.LineBreakMode = UILineBreakMode.TailTruncation;
        return label;
    }

    public static UILabel WithSystemFont(this UILabel label, int size, UIFontWeight weight = UIFontWeight.Regular)
    {
        label.Font = UIFont.SystemFontOfSize(size, weight);
        return label;
    }

    #endregion UILabel

    #region UITextField

    public static UITextField CreateTextField(this UIResponder parent)
    {
        return new UITextField();
    }

	public static UITextField WithLeftPadding(this UITextField input, int leftPadding)
	{
		input.LeftView = new UIView(new CGRect(0, 0, leftPadding, 5));
		input.LeftViewMode = UITextFieldViewMode.Always;
		return input;
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
	
	public static UITextField AsSearchField(this UITextField input, UIReturnKeyType returnKeyType = UIReturnKeyType.Search)
	{
		input.KeyboardType = UIKeyboardType.Default;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.ReturnKeyType = returnKeyType;
		input.AutocorrectionType = UITextAutocorrectionType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
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

    public static UITextField WithAutocapitalization(this UITextField input, UITextAutocapitalizationType autocapitalizationType = UITextAutocapitalizationType.Words)
    {
        input.AutocapitalizationType = autocapitalizationType;
        return input;
    }

	public static UITextField OnReturnNextResponder(this UITextField input, UIResponder nextReponder, UIReturnKeyType returnKeyType = UIReturnKeyType.Default, Action action = null)
    {
        input.ReturnKeyType = returnKeyType;
        input.ShouldReturn += (textField) =>
        {
            action?.Invoke();
	        nextReponder?.BecomeFirstResponder();
			return false;
        };
        return input;
    }

    public static UITextField WithBorderStyle(this UITextField input, UITextBorderStyle style)
	{
        input.BorderStyle = style;
		return input;
	}

	#endregion 

	#region UISearchBar

	public static UISearchBar CreateSearchBar(this UIResponder parent)
    {
        return new UISearchBar();
    }

    public static UISearchBar WithPlaceholder(this UISearchBar input, string text)
    {
        input.Placeholder = text;
        return input;
    }

    public static UISearchBar WithTintColor(this UISearchBar input, UIColor color)
    {
        input.TintColor = color;
        return input;
    }

    public static UISearchBar WithBarTintColor(this UISearchBar input, UIColor color)
    {
        input.BarTintColor = color;
        return input;
    }

	public static UISearchBar WithBorderWidthAndColor(this UISearchBar input, UIColor color, int width)
	{
        input.Layer.BorderColor = color.CGColor;
        input.Layer.BorderWidth = width;
		return input;
	}

	public static UISearchBar WithSearchBarStyle(this UISearchBar input, UISearchBarStyle style)
    {
        input.SearchBarStyle = style;
        return input;
    }

    public static UISearchBar WithBarStyle(this UISearchBar input, UIBarStyle style)
	{
        input.BarStyle = style;
		return input;
	}

    public static UISearchBar WithTranslucent(this UISearchBar input, bool isTranslucent = true)
    {
        input.Translucent = isTranslucent;
        return input;
    }

    public static UISearchBar WithOpaque(this UISearchBar input, bool isOpaque = true)
    {
        input.Opaque = isOpaque;
        return input;
    }

    #endregion

    #region UITextView

    public static UITextView CreateTextView(this UIResponder parent)
    {
        return new UITextView();
    }

    public static UITextView WithText(this UITextView input, string text)
    {
        input.Text = text;
        return input;
    }

    #endregion UITextView

    #region UIImageView

    public static UIImageView CreateImageView(this UIResponder parent)
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
	#if DEBUG
	    try
	    {
		    view.Image = new UIImage(imageName);
	    }
	    catch (Exception ex)
	    {
		    System.Diagnostics.Debug.WriteLine($"Missing image: {imageName} {ex}");
		    throw;
	    }
	#else
		view.Image = new UIImage(imageName);
	#endif
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

	public static TView Visible<TView>(this TView view, bool value) where TView : UIView
	{
		return value ? view.Visible() : view.Invisible();
	}

	public static TView WithBackgroundColor<TView>(this TView view, int color) where TView : UIView
	{
		view.BackgroundColor = color.ColorFromHex();
		return view;
	}

	[Obsolete("Cette méthode ne doit être utilisée qu'en développement. Pour définir le fond d'une View utilisez WithBackgroundColor")]
	public static TView WithDraftBackground<TView>(this TView view, int color) where TView : UIView
	{
#if DEBUG
		view.BackgroundColor = color.ColorFromHex();
#endif
		return view;
	}

	[Obsolete("Cette méthode ne doit être utilisée qu'en développement. Pour définir le fond d'une View utilisez WithBackgroundColor")]
	public static TView WithDraftBackground<TView>(this TView view, UIColor color = null) where TView : UIView
	{
#if DEBUG
		view.BackgroundColor = color ?? UIColor.Orange;
#endif
		return view;
	}

	public static TView WithBackgroundColor<TView>(this TView view, UIColor color) where TView : UIView
    {
        view.BackgroundColor = color;
        return view;
    }

	public static TView WithBorder<TView>(this TView view, int borderColor, int size) where TView : UIView
	{
		return view.WithBorder(borderColor.ColorFromHex(), size);
	}
	public static TView WithBorder<TView>(this TView view, UIColor borderColor, int size) where TView : UIView
    {
        view.Layer.BorderColor = borderColor.CGColor;
        view.Layer.BorderWidth = size;
        return view;
    }

    public static TView WithShadow<TView>(this TView view, UIColor shadowColor, int left, int top, float radius = 8f, float opacity = 1f) where TView : UIView
    {
        view.Layer.ShadowColor = shadowColor.CGColor;
        view.Layer.ShadowOpacity = opacity;
        view.Layer.ShadowRadius = radius;
        view.Layer.ShadowOffset = new CGSize(left, top);

        return view;
    }

    public static TView WithClipping<TView>(this TView view) where TView : UIView
    {
        view.ClipsToBounds = true;

        return view;
    }

    public static TView WithoutShadow<TView>(this TView view) where TView : UIView
    {
        view.Layer.ShadowColor = UIColor.Clear.CGColor;
        view.Layer.ShadowOpacity = 0;
        view.Layer.ShadowRadius = 0;
        view.Layer.ShadowOffset = CGSize.Empty;

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
	public static TView Enable<TView>(this TView view, bool value) where TView : UIView
	{
		return value ? view.Enable() : view.Disable();
	}

	public static TView AddTapAction<TView>(this TView view, Action tapped) where TView : UIView
    {
        UITapGestureRecognizer recognizer = new UITapGestureRecognizer(tapped);
        view.AddGestureRecognizer(recognizer);
        return view;
    }

	#endregion

	#region UIDatePicker

	public static UIDatePicker CreateDatePicker(this UIResponder _)
	{
		return new UIDatePicker();
	}

	public static UIDatePicker WithMode(this UIDatePicker view, UIDatePickerMode pickerMode)
	{
		view.Mode = pickerMode;
		return view;
	}

	#endregion UIDatePicker


}