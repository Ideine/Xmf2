using System;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using UIKit;
using Xmf2.Core.iOS.Controls;
using Xmf2.Core.iOS.Extensions;
using Xmf2.Core.iOS.Helpers;
using Xmf2.Core.iOS.Layouts;
using Xmf2.Core.Subscriptions;

// ReSharper disable CheckNamespace
public static class CreatorExtensions
{
	#region UIButton

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIButton CreateButton(this object _)
	{
		return new UIButton(UIButtonType.Custom);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIButton CreateButton(this object _, UIButtonType type)
	{
		return new UIButton(type);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TUIButton WithContentEdgeInset<TUIButton>(this TUIButton button, nfloat top, nfloat left, nfloat bottom, nfloat right) where TUIButton : UIButton
	{
		return button.WithContentEdgeInset(new UIEdgeInsets(top, left, bottom, right));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TUIButton WithTitleEdgeInset<TUIButton>(this TUIButton button, nfloat top, nfloat left, nfloat bottom, nfloat right) where TUIButton : UIButton
	{
		return button.WithTitleEdgeInset(new UIEdgeInsets(top, left, bottom, right));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TUIButton WithImageEdgeInset<TUIButton>(this TUIButton button, nfloat top, nfloat left, nfloat bottom, nfloat right) where TUIButton : UIButton
	{
		return button.WithImageEdgeInset(new UIEdgeInsets(top, left, bottom, right));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TUIButton WithContentEdgeInset<TUIButton>(this TUIButton button, UIEdgeInsets insets) where TUIButton : UIButton
	{
		button.ContentEdgeInsets = insets;
		return button;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TUIButton WithImageEdgeInset<TUIButton>(this TUIButton button, UIEdgeInsets insets) where TUIButton : UIButton
	{
		button.ImageEdgeInsets = insets;
		return button;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TUIButton WithTitleEdgeInset<TUIButton>(this TUIButton button, UIEdgeInsets insets) where TUIButton : UIButton
	{
		button.TitleEdgeInsets = insets;
		return button;
	}

	public static TUIButton WithTitle<TUIButton>(this TUIButton button, string title) where TUIButton : UIButton
	{
		button.SetTitle(title, UIControlState.Normal);
		return button;
	}

	public static TUIButton WithTitle<TUIButton>(this TUIButton button, NSAttributedString title) where TUIButton : UIButton
	{
		button.SetAttributedTitle(title, UIControlState.Normal);
		return button;
	}

	public static TUIButton WithTextColor<TUIButton>(this TUIButton button, uint color, UIControlState forState) where TUIButton : UIButton
	{
		button.SetTitleColor(color.ColorFromHex(), forState);
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

	public static TUIButton WithTextColor<TUIButton>(this TUIButton button, uint color) where TUIButton : UIButton
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

	public static TUIButton WithTextColorSelected<TUIButton>(this TUIButton button, uint color) where TUIButton : UIButton
	{
		button.SetTitleColor(color.ColorFromHex(), UIControlState.Selected);
		return button;
	}

	public static TUIButton WithTextColorHighlight<TUIButton>(this TUIButton button, uint color) where TUIButton : UIButton
	{
		return button.WithTextColorHighlight(color.ColorFromHex());
	}

	public static TUIButton WithTextColorSelected<TUIButton>(this TUIButton button, UIColor color) where TUIButton : UIButton
	{
		button.SetTitleColor(color, UIControlState.Selected);
		return button;
	}

	public static TUIButton WithTextColorSelected<TUIButton>(this TUIButton button, int color) where TUIButton : UIButton
	{
		return button.WithTextColorSelected(color.ColorFromHex());
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

	public static TUIButton WithSystemFont<TUIButton>(this TUIButton button, float size, UIFontWeight weight = UIFontWeight.Regular) where TUIButton : UIButton
	{
		button.Font = UIFont.SystemFontOfSize(size, weight);
		return button;
	}

	public static TUIButton WithItalicSystemFont<TUIButton>(this TUIButton button, float size) where TUIButton : UIButton
	{
		button.Font = UIFont.ItalicSystemFontOfSize(size);
		return button;
	}

	/// <summary>
	/// Obsolète, use <see cref="EventsExtensions.TouchUpInsideSubscription(UIButton, EventHandler, bool)
	/// </summary>
	[Obsolete("Use EventsExtensions.TouchUpInsideSubscription")]
	public static TUIButton OnClick<TUIButton>(this TUIButton button, Action action) where TUIButton : UIButton
	{
		button.TouchUpInside += (sender, e) => action?.Invoke();
		return button;
	}

	/// <summary>
	/// Obsolète, use <see cref="EventsExtensions.TouchUpInsideSubscription(UIButton, EventHandler, bool)
	/// </summary>
	[Obsolete("Use EventsExtensions.TouchUpInsideSubscription")]
	public static TUIButton OnClick<TUIButton>(this TUIButton button, Action<TUIButton, EventArgs> action) where TUIButton : UIButton
	{
		button.TouchUpInside += (sender, e) => action?.Invoke((TUIButton)sender, e);
		return button;
	}

	public static TUIButton OnClick<TUIButton>(this TUIButton button, Action action, out Action unregisterAction) where TUIButton : UIButton
	{
		EventHandler onTouchUpInside = (sender, e) => action?.Invoke();
		button.TouchUpInside += onTouchUpInside;
		unregisterAction = () => { button.TouchUpInside -= onTouchUpInside; };
		return button;
	}

	public static TUIButton WithBackgroundColor<TUIButton>(this TUIButton button, int backgroundColor, UIControlState forState) where TUIButton : UIButton
	{
		return button.WithBackgroundColor(backgroundColor.ColorFromHex(), forState);
	}

	public static TUIButton WithBackgroundColor<TUIButton>(this TUIButton button, uint backgroundColor, UIControlState forState) where TUIButton : UIButton
	{
		return button.WithBackgroundColor(backgroundColor.ColorFromHex(), forState);
	}

	public static TUIButton WithBackgroundColor<TUIButton>(this TUIButton button, UIColor backgroundColor, UIControlState forState) where TUIButton : UIButton
	{
		UIImage backgroundImage;
		UIGraphics.BeginImageContext(new CGSize(1f, 1f));
		try
		{
			var context = UIGraphics.GetCurrentContext();
			context.SetFillColor(backgroundColor.CGColor);
			context.FillRect(new CGRect(0, 0, 1, 1));
			backgroundImage = UIGraphics.GetImageFromCurrentImageContext();
		}
		finally
		{
			UIGraphics.EndImageContext();
		}

		button.SetBackgroundImage(backgroundImage, forState);
		return button;
	}

	#endregion

	#region UIControl

	public static UIControl CreateControl(this object _)
	{
		return new UIControl();
	}

	#endregion UIControl

	#region UITableViewCell

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TCell WithSelectedBackground<TCell>(this TCell cell, int color) where TCell : UITableViewCell
	{
		return cell.WithBackgroundColor(color.ColorFromHex());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TCell WithSelectedBackground<TCell>(this TCell cell, UIColor color) where TCell : UITableViewCell
	{
		cell.SelectedBackgroundView = new UIView().WithBackgroundColor(color);
		return cell;
	}

	#endregion UITableViewCell

	#region UIView

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CreateSeparator(this object _)
	{
		return new UIView();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CreateSeparator(this object _, int backgroundColor)
	{
		return new UIView().WithBackgroundColor(backgroundColor.ColorFromHex());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CreateSeparator(this object _, uint backgroundColor)
	{
		return new UIView().WithBackgroundColor(backgroundColor.ColorFromHex());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CreateSeparator(this object _, UIColor backgroundColor)
	{
		return new UIView().WithBackgroundColor(backgroundColor);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIView CreateView(this object _)
	{
		return new UIView();
	}

	public static TUIView WithContentMode<TUIView>(this TUIView view, UIViewContentMode mode) where TUIView : UIView
	{
		view.ContentMode = mode;
		return view;
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

	public static void FadeTo58Percent<TView>(this TView view) where TView : UIView
	{
		view.Alpha = 0.58f;
	}

	public static void FadeTo100Percent<TView>(this TView view) where TView : UIView
	{
		view.Alpha = 1f;
	}

	public static TView ScaleTo<TView>(this TView view, float ratio, HorizontalAnchor horizontalAnchor = HorizontalAnchor.Center, VerticalAnchor verticalAnchor = VerticalAnchor.Center) where TView : UIView
	{
		view.Transform = GetScaleTransform(view, ratio, horizontalAnchor, verticalAnchor);
		return view;
	}

	public static CGAffineTransform GetScaleTransform(this UIView view, float ratio, HorizontalAnchor horizontalAnchor = HorizontalAnchor.Center, VerticalAnchor verticalAnchor = VerticalAnchor.Center)
	{
		var transform = CGAffineTransform.MakeScale(ratio, ratio);
		switch (horizontalAnchor)
		{
			case HorizontalAnchor.Left:
				transform = transform * CGAffineTransform.MakeTranslation(-view.Frame.Width * (1 - ratio) / 2f, 0f);
				break;

			case HorizontalAnchor.Right:
				transform = transform * CGAffineTransform.MakeTranslation(view.Frame.Width * (1 - ratio) / 2f, 0f);
				break;

			case HorizontalAnchor.Center:
			default:
				break;
		}
		switch (verticalAnchor)
		{
			case VerticalAnchor.Top:
				transform = transform * CGAffineTransform.MakeTranslation(0f, -view.Frame.Width * (1 - ratio));
				break;

			case VerticalAnchor.Bottom:
				transform = transform * CGAffineTransform.MakeTranslation(0f, view.Frame.Width * (1 - ratio));
				break;

			case VerticalAnchor.Center:
			default:
				break;
		}
		return transform;
	}

	public static TView ScaleDown<TView>(this TView view) where TView : UIView
	{
		view.Transform = CGAffineTransform.MakeScale(OnTouchTransformer.DEFAULT_SCALE_DOWN_RATIO, OnTouchTransformer.DEFAULT_SCALE_DOWN_RATIO);
		return view;
	}

	#endregion

	#region ScrollView

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static NestedScrollView CreateVerticalNestedScroll(this object _)
	{
		return new NestedScrollView
		{
			AlwaysBounceHorizontal = false,
			AlwaysBounceVertical = true,
			Bounces = true,
			BouncesZoom = false,
			ShowsVerticalScrollIndicator = true,
			ShowsHorizontalScrollIndicator = false
		}.WithContentInsetAdjustementBehavior(UIScrollViewContentInsetAdjustmentBehavior.Never);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView CreateVerticalScroll(this object _)
	{
		return new UIScrollView()
		{
			AlwaysBounceHorizontal = false,
			AlwaysBounceVertical = true,
			Bounces = true,
			BouncesZoom = false,
			ShowsVerticalScrollIndicator = true,
			ShowsHorizontalScrollIndicator = false
		}.WithContentInsetAdjustementBehavior(UIScrollViewContentInsetAdjustmentBehavior.Never);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIScrollView CreateHorizontalScroll(this object _)
	{
		return new UIScrollView()
		{
			AlwaysBounceHorizontal = false,
			AlwaysBounceVertical = false,
			Bounces = false,
			BouncesZoom = false,
			ShowsVerticalScrollIndicator = false,
			ShowsHorizontalScrollIndicator = false
		}.WithContentInsetAdjustementBehavior(UIScrollViewContentInsetAdjustmentBehavior.Never);
	}

	public static TUIScrollView WithContentInset<TUIScrollView>(this TUIScrollView view, UIEdgeInsets inset) where TUIScrollView : UIScrollView
	{
		view.ContentInset = inset;
		return view;
	}

	public static TUIScrollView WithContentInset<TUIScrollView>(this TUIScrollView view, float top = 0, float left = 0, float bottom = 0, float right = 0) where TUIScrollView : UIScrollView
	{
		return view.WithContentInset(new UIEdgeInsets(top, left, bottom, right));
	}

	public static TUIScrollView WithContentInset<TUIScrollView>(this TUIScrollView view, nfloat top = default, nfloat left = default, nfloat bottom = default, nfloat right = default) where TUIScrollView : UIScrollView
	{
		return view.WithContentInset(new UIEdgeInsets(top, left, bottom, right));
	}

	public static TUIScrollView WithContentInsetAdjustementBehavior<TUIScrollView>(this TUIScrollView view, UIScrollViewContentInsetAdjustmentBehavior behavior) where TUIScrollView : UIScrollView
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UILabel CreateLabel(this object _)
	{
		return new UILabel();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UILabel CreatePaddingLabel(this object _, UIEdgeInsets inset)
	{
		return new UIPaddingLabel()
		{
			ContentInset = inset
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UILabel CreatePaddingLabel(this object _, float top, float left, float bottom, float right)
	{
		return new UIPaddingLabel()
		{
			ContentInset = new UIEdgeInsets(top, left, bottom, right)
		};
	}

	public static UILabel WithText(this UILabel label, string text)
	{
		label.Text = text;
		return label;
	}

	public static UILabel WithText(this UILabel label, NSAttributedString text)
	{
		label.AttributedText = text;
		return label;
	}
	
	public static UILabel WithTextColor(this UILabel label, int color)
	{
		return WithTextColor(label, color.ColorFromHex());
	}

	public static UILabel WithTextColor(this UILabel label, uint color)
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

	public static UILabel WithAdjustsFontSizeToFit(this UILabel label, nfloat minimumScaleFactor)
	{
		label.AdjustsFontSizeToFitWidth = true;
		label.MinimumScaleFactor = minimumScaleFactor;
		return label;
	}

	public static UILabel WithTextWrapping(this UILabel label, int maxLine = 0)
	{
		label.Lines = maxLine;
		label.LineBreakMode = UILineBreakMode.WordWrap;
		return label;
	}

	public static UILabel WithEllipsis(this UILabel label, int maxLine = 0)
	{
		label.Lines = maxLine;
		label.LineBreakMode = UILineBreakMode.TailTruncation;
		return label;
	}

	public static UILabel WithSystemFont(this UILabel label, float size, UIFontWeight weight = UIFontWeight.Regular)
	{
		label.Font = UIFont.SystemFontOfSize(size, weight);
		return label;
	}

	public static UILabel WithItalicSystemFont(this UILabel label, float size)
	{
		label.Font = UIFont.ItalicSystemFontOfSize(size);
		return label;
	}

	#endregion UILabel

	#region UITextField

	/// <summary>
	/// Workaround for num-pad text field that does not allows next button.
	/// https://stackoverflow.com/a/45395904/1584823
	/// </summary>
	public static IDisposable AddDoneToolbar(this UITextField textField, EventHandler onDoneEventHandler)
	{
		Xmf2Disposable disposable = new Xmf2Disposable();

		UIToolbar uiToolBar = new UIToolbar()
		{
			BarStyle = UIBarStyle.Default,
			Items = new UIBarButtonItem[]
			{
				new UIBarButtonItem(UIBarButtonSystemItem.Done, onDoneEventHandler).DisposeWith(disposable)
			}
		}.DisposeWith(disposable);
		uiToolBar.SizeToFit();
		textField.InputAccessoryView = uiToolBar;

		return disposable;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UITextField CreateTextField(this object _)
	{
		return new UITextField();
	}

	public static UITextField WithLeftPadding(this UITextField input, int leftPadding)
	{
		input.LeftView = new UIView(new CGRect(0, 0, leftPadding, 5));
		input.LeftViewMode = UITextFieldViewMode.Always;
		return input;
	}

	public static UITextField AsPasswordField(this UITextField input)
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)
		   && (UITextContentType.Password != null))
		{
			input.TextContentType = UITextContentType.Password;
			input.AutocorrectionType = UITextAutocorrectionType.Yes;
		}
		else
		{
			input.AutocorrectionType = UITextAutocorrectionType.No;
		}
		input.KeyboardType = UIKeyboardType.Default;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		input.SecureTextEntry = true;
		return input;
	}

	public static UITextField AsPinField(this UITextField input)
	{
		input.AutocorrectionType = UITextAutocorrectionType.No;
		input.KeyboardType = UIKeyboardType.NumberPad;
		input.SpellCheckingType = UITextSpellCheckingType.No;
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

	public static UITextField AsMyUsernameField(this UITextField input)
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)
		   && (UITextContentType.Username != null))
		{
			input.TextContentType = UITextContentType.Username;
			input.AutocorrectionType = UITextAutocorrectionType.Yes;
		}
		else
		{
			input.AutocorrectionType = UITextAutocorrectionType.No;
		}
		input.KeyboardType = UIKeyboardType.Default;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField AsMyFamilyNameField(this UITextField input)
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)
		   && (UITextContentType.FamilyName != null))
		{
			input.TextContentType = UITextContentType.FamilyName;
			input.AutocorrectionType = UITextAutocorrectionType.Yes;
		}
		else
		{
			input.AutocorrectionType = UITextAutocorrectionType.No;
		}
		input.KeyboardType = UIKeyboardType.Default;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField AsMyGivenNameField(this UITextField input)
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)
		   && (UITextContentType.GivenName != null))
		{
			input.TextContentType = UITextContentType.GivenName;
			input.AutocorrectionType = UITextAutocorrectionType.Yes;
		}
		else
		{
			input.AutocorrectionType = UITextAutocorrectionType.No;
		}
		input.KeyboardType = UIKeyboardType.Default;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField AsUsernameField(this UITextField input)
	{
		input.KeyboardType = UIKeyboardType.Default;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocorrectionType = UITextAutocorrectionType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField AsMyEmailField(this UITextField input)
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)
		   && (UITextContentType.EmailAddress != null))
		{
			input.TextContentType = UITextContentType.EmailAddress;
			input.AutocorrectionType = UITextAutocorrectionType.Yes;
		}
		else
		{
			input.AutocorrectionType = UITextAutocorrectionType.No;
		}
		input.KeyboardType = UIKeyboardType.EmailAddress;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField AsEmailField(this UITextField input)
	{
		input.KeyboardType = UIKeyboardType.EmailAddress;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocorrectionType = UITextAutocorrectionType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField AsNumpadField(this UITextField input)
	{
		input.KeyboardType = UIKeyboardType.NumberPad;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocorrectionType = UITextAutocorrectionType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField AsMyPhonepadField(this UITextField input)
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0)
		   && (UITextContentType.TelephoneNumber != null))
		{
			input.TextContentType = UITextContentType.TelephoneNumber;
			input.AutocorrectionType = UITextAutocorrectionType.Yes;
		}
		else
		{
			input.AutocorrectionType = UITextAutocorrectionType.No;
		}
		input.KeyboardType = UIKeyboardType.PhonePad;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField AsPhonepadField(this UITextField input)
	{
		input.KeyboardType = UIKeyboardType.PhonePad;
		input.SpellCheckingType = UITextSpellCheckingType.No;
		input.AutocorrectionType = UITextAutocorrectionType.No;
		input.AutocapitalizationType = UITextAutocapitalizationType.None;
		return input;
	}

	public static UITextField WithTextColor(this UITextField input, int color)
	{
		return input.WithTextColor(color.ColorFromHex());
	}

	public static UITextField WithTextColor(this UITextField input, uint color)
	{
		return input.WithTextColor(color.ColorFromHex());
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

	public static UITextField WithSystemFont(this UITextField input, float size, UIFontWeight weight = UIFontWeight.Regular)
	{
		input.Font = UIFont.SystemFontOfSize(size, weight);
		return input;
	}

	public static UITextField WithPlaceholder(this UITextField input, string placeholder)
	{
		input.Placeholder = placeholder;
		return input;
	}

	public static UITextField WithPlaceholderTextColor(this UITextField input, int color)
	{
		return input.WithPlaceholderTextColor(color.ColorFromHex());
	}

	public static UITextField WithPlaceholderTextColor(this UITextField input, uint color)
	{
		return input.WithPlaceholderTextColor(color.ColorFromHex());
	}

	public static UITextField WithPlaceholderTextColor(this UITextField input, UIColor color)
	{
		input.AttributedPlaceholder = new NSAttributedString(input.Placeholder ?? string.Empty, input.Font, color);
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

	public static UITextField OnReturnNextResponder(this UITextField input, UIResponder nextReponder, Action action = null)
	{
		input.ShouldReturn += (textField) =>
		{
			action?.Invoke();
			nextReponder?.BecomeFirstResponder();
			return false;
		};
		return input;
	}

	public static UITextField WithReturnKey(this UITextField input, UIReturnKeyType returnKeyType)
	{
		input.ReturnKeyType = returnKeyType;
		return input;
	}

	public static UITextField WithBorderStyle(this UITextField input, UITextBorderStyle style)
	{
		input.BorderStyle = style;
		return input;
	}

	#endregion

	#region UISearchBar

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UISearchBar CreateSearchBar(this object _)
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UITextView CreateTextView(this object _)
	{
		return new UITextView();
	}

	public static UITextView WithSystemFont(this UITextView input, float fontSize, UIFontWeight weight)
	{
		input.Font = UIFont.SystemFontOfSize(fontSize, weight);
		return input;
	}

	public static UITextView WithText(this UITextView input, string text)
	{
		input.Text = text;
		return input;
	}

	public static UITextView WithTextColor(this UITextView input, int color)
	{
		return WithTextColor(input, color.ColorFromHex());
	}

	public static UITextView WithTextColor(this UITextView input, uint color)
	{
		return WithTextColor(input, color.ColorFromHex());
	}

	public static UITextView WithTextColor(this UITextView input, UIColor color)
	{
		input.TextColor = color;
		return input;
	}

	#endregion UITextView

	#region UIImageView

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIImageView CreateImageView(this object _)
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
		if (string.IsNullOrEmpty(imageName))
		{
			view.Image = null;
		}
		else
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
		}
		return view;
	}

	public static UIImageView WithoutImage(this UIImageView view)
	{
		view.Image = null;
		return view;
	}

	public static UIImageView UniformToFit(this UIImageView view)
	{
		view.ContentMode = UIViewContentMode.ScaleAspectFit;
		return view;
	}

	public static UIImageView UniformToFill(this UIImageView view)
	{
		view.ContentMode = UIViewContentMode.ScaleAspectFill;
		return view;
	}

	#endregion

	#region UIControl

	public static TUIControl WithAlignment<TUIControl>(this TUIControl control, UIControlContentHorizontalAlignment horizontalAlignment) where TUIControl : UIControl
	{
		control.HorizontalAlignment = horizontalAlignment;
		return control;
	}

	#endregion UIControl

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

	public static TView WithBackgroundColor<TView>(this TView view, uint color) where TView : UIView
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
	public static TView WithDraftBackground<TView>(this TView view, uint color) where TView : UIView
	{
#if DEBUG
		view.BackgroundColor = color.ColorFromHex();
#endif
		return view;
	}

	[Obsolete("Cette méthode ne doit être utilisée qu'en développement. Pour définir le fond d'une View utilisez WithBackgroundColor")]
	public static TView WithDraftBackground<TView>(this TView view, UIColor color = null, bool addAlpha = true) where TView : UIView
	{
#if DEBUG
		var lcolor = (color ?? UIColor.Orange);
		view.BackgroundColor = addAlpha
			? lcolor.ColorWithAlpha(0.25f)
			: lcolor;
#endif
		return view;
	}

	public static TView WithBackgroundColor<TView>(this TView view, UIColor color) where TView : UIView
	{
		view.BackgroundColor = color;
		return view;
	}

	public static TView WithBorder<TView>(this TView view, uint borderColor, float size) where TView : UIView
	{
		return view.WithBorder(borderColor.ColorFromHex(), size);
	}

	public static TView WithBorder<TView>(this TView view, UIColor borderColor, float size) where TView : UIView
	{
		view.Layer.BorderColor = borderColor.CGColor;
		view.Layer.BorderWidth = size;
		return view;
	}

	public static TView WithSketchShadow<TView>(this TView view, uint shadowColor, float x = 0, float y = 0, float blur = 4, float spread = 0) where TView : UIView
	{
		//Reference : https://stackoverflow.com/a/48489506/1479638
		UIColor color = shadowColor.ColorFromHex();
		view.Layer.ShadowColor = color.ColorWithAlpha(1).CGColor;
		view.Layer.ShadowOpacity = (float)color.CGColor.Alpha;
		view.Layer.ShadowOffset = new CGSize(x, y);
		view.Layer.ShadowRadius = blur / 2f;
		/*
		if (spread == 0)
		{
			view.Layer.ShadowPath = null;
		}
		else
		{
			view.Layer.ShadowPath = UIBezierPath.FromRect(view.Layer.Bounds.Inset(-spread, -spread)).CGPath;
		}
		*/
		return view;
	}

	public static TView WithShadow<TView>(this TView view, uint shadowColor, float xOffset, float yOffset, float radius = 8f) where TView : UIView
	{
		var color = shadowColor.ColorFromHex();
		view.Layer.ShadowColor = color.CGColor;

		view.Layer.ShadowOpacity = (float)color.CGColor.Alpha;
		view.Layer.ShadowRadius = radius;
		view.Layer.ShadowOffset = new CGSize(xOffset, yOffset);

		return view;
	}

	public static TView WithShadow<TView>(this TView view, UIColor shadowColor, float xOffset, float yOffset, float radius = 8f, float opacity = 1f) where TView : UIView
	{
		view.Layer.ShadowColor = shadowColor.CGColor;
		view.Layer.ShadowOpacity = opacity;
		view.Layer.ShadowRadius = radius;
		view.Layer.ShadowOffset = new CGSize(xOffset, yOffset);

		return view;
	}

	public static TView WithClipping<TView>(this TView view) where TView : UIView
	{
		view.ClipsToBounds = true;
		return view;
	}

	public static TView WithClipping<TView>(this TView view, bool clip) where TView : UIView
	{
		view.ClipsToBounds = clip;
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TView WithCornerRadius<TView>(this TView view, float size) where TView : UIView
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

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIDatePicker CreateDatePicker(this object _)
	{
		return new UIDatePicker();
	}

	public static UIDatePicker WithMode(this UIDatePicker view, UIDatePickerMode pickerMode)
	{
		view.Mode = pickerMode;
		return view;
	}

	#endregion UIDatePicker

	#region UITableView

	public static UITableView CreateTableView(this object _)
	{
		return _.CreateTableView(frame: null);
	}

	public static UITableView CreateTableView(this object _, CGRect? frame)
	{
		var tableView = frame.HasValue
			? new UITableView(frame.Value)
			: new UITableView();
		return tableView.WithoutAutomaticEstimatedHeights();
	}

	/// <see cref="https://stackoverflow.com/a/46257601/1584823"/>
	public static TUITableView WithoutAutomaticEstimatedHeights<TUITableView>(this TUITableView view) where TUITableView : UITableView
	{
		if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
		{
			if (view.EstimatedRowHeight == UITableView.AutomaticDimension)
			{
				view.EstimatedRowHeight = 0f;
			}

			if (view.EstimatedSectionHeaderHeight == UITableView.AutomaticDimension)
			{
				view.EstimatedSectionHeaderHeight = 0f;
			}

			if (view.EstimatedSectionFooterHeight == UITableView.AutomaticDimension)
			{
				view.EstimatedSectionFooterHeight = 0f;
			}
		}

		return view;
	}

	#endregion UITableView

	#region UILinearLayout

	public static UILinearLayout CreateVerticalLinearLayout(this object _)
	{
		return new UILinearLayout(UILinearLayout.LayoutOrientation.Vertical);
	}

	#endregion

	public static VisibilityToggleContainer CreateVisibilityToggleContainer(this object _)
	{
		return new VisibilityToggleContainer();
	}
}