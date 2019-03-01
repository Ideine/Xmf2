using System;
using UIKit;
using Xmf2.Core.iOS.Controls;
using Xmf2.Core.iOS.Extensions;

// ReSharper disable CheckNamespace

public static class CustomControlsCreatorExtensions
{
	public static UIBackgroundHighlightButton CreateHighlightButton(this object _)
	{
		return new UIBackgroundHighlightButton();
	}
	
	public static UIBackgroundHighlightSelectedButton CreateHighlightSelectedButton(this object _)
	{
		return new UIBackgroundHighlightSelectedButton();
	}

	public static UIActionHighlightButton CreateActionHighlightButton(this object _)
	{
		return new UIActionHighlightButton();
	}

	public static TUIHighlightButton WithBackgroundHighlightedColor<TUIHighlightButton>(this TUIHighlightButton button, UIColor backgroundColor) where TUIHighlightButton : IUIBackgroundHighlight
	{
		button.BackgroundHightlightedColor = backgroundColor;
		return button;
	}

	public static TUIHighlightButton WithBackgroundHighlightedColor<TUIHighlightButton>(this TUIHighlightButton button, uint backgroundColor) where TUIHighlightButton : IUIBackgroundHighlight
	{
		button.BackgroundHightlightedColor = backgroundColor.ColorFromHex();
		return button;
	}
	
	public static TUISelectedButton WithBackgroundSelectedColor<TUISelectedButton>(this TUISelectedButton button, UIColor backgroundColor) where TUISelectedButton : IUIBackgroundSelected
	{
		button.BackgroundSelectedColor = backgroundColor;
		return button;
	}

	public static TUISelectedButton WithBackgroundSelectedColor<TUISelectedButton>(this TUISelectedButton button, uint backgroundColor) where TUISelectedButton : IUIBackgroundSelected
	{
		button.BackgroundSelectedColor = backgroundColor.ColorFromHex();
		return button;
	}
}