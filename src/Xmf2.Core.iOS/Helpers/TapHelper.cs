using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xmf2.Core.iOS.Helpers
{
	public static class TapHelper
	{
		/// <summary>
		/// Method from https://stackoverflow.com/a/28519273
		/// </summary>
		public static bool DidTapAttributedTextInLabel(UITapGestureRecognizer tap, UILabel label, NSRange targetRange)
		{
			NSLayoutManager layoutManager = new();
			NSTextContainer textContainer = new(CGSize.Empty);
			NSTextStorage textStorage = new();
			textStorage.SetString(label.AttributedText);

			// Configure layoutManager and textStorage
			layoutManager.AddTextContainer(textContainer);
			textStorage.AddLayoutManager(layoutManager);

			// Configure textContainer
			textContainer.LineFragmentPadding = 0.0f;
			textContainer.LineBreakMode = label.LineBreakMode;
			textContainer.MaximumNumberOfLines = (nuint)label.Lines;
			CGSize labelSize = label.Bounds.Size;
			textContainer.Size = labelSize;

			// Find the tapped character location and compare it to the specified range
			CGPoint locationOfTouchInLabel = tap.LocationInView(label);
			CGRect textBoundingBox = layoutManager.GetUsedRectForTextContainer(textContainer);
			CGPoint textContainerOffset = new(
				((labelSize.Width - textBoundingBox.Size.Width) * 0.5) - textBoundingBox.Location.X,
				((labelSize.Height - textBoundingBox.Size.Height) * 0.5) - textBoundingBox.Location.Y
			);
			CGPoint locationOfTouchInTextContainer = new(locationOfTouchInLabel.X - textContainerOffset.X, locationOfTouchInLabel.Y - textContainerOffset.Y);
			nint indexOfCharacter = (nint)layoutManager.GetCharacterIndex(locationOfTouchInTextContainer, textContainer, out nfloat _);

			// Xamarin version of NSLocationInRange?
			return indexOfCharacter >= targetRange.Location && indexOfCharacter < targetRange.Location + targetRange.Length;
		}
	}
}