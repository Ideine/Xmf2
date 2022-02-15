using UIKit;
using Xmf2.Commons.iOS.Controls;
using Xmf2.iOS.Extensions.Extensions;

namespace Xmf2.Commons.iOS.Extensions
{
	public static class CreatorExtensions
	{
		public static UIHighlightButton CreateHighlightButton(this object parent)
		{
			return new UIHighlightButton();
		}


		public static TButton WithHighlightBackgroundColor<TButton>(this TButton button, int color)
			where TButton : UIHighlightButton
		{
			button.HighlightColor = color.ColorFromHex();
			return button;
		}

		public static TButton WithHighlightBackgroundColor<TButton>(this TButton button, UIColor color)
			where TButton : UIHighlightButton
		{
			button.HighlightColor = color;
			return button;
		}
	}
}