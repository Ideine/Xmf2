using System;
using CoreGraphics;
using UIKit;
using Xmf2.Core.iOS.Controls;

namespace Xmf2.Commons.iOS.Controls
{
	public class UIRightImageButton : UIBackgroundHighlightButton
	{
		public override CGRect ContentRectForBounds(CGRect rect)
		{
			UIEdgeInsets insets = ContentEdgeInsets;

			return new CGRect(
				rect.X + insets.Left,
				rect.Y + insets.Top,
				rect.Width - (insets.Left + insets.Right),
				rect.Height - (insets.Top + insets.Bottom)
			);
		}

		public override CGRect TitleRectForContentRect(CGRect rect)
		{
			CGRect imageRect = ImageRectForContentRect(rect);

			nfloat x = rect.X + TitleEdgeInsets.Left;

			return new CGRect(
				x,
				rect.Y + TitleEdgeInsets.Top,
				imageRect.X - ImageEdgeInsets.Left - TitleEdgeInsets.Right - x,
				rect.Height - TitleEdgeInsets.Top - TitleEdgeInsets.Bottom
			);
		}

		public override CGRect ImageRectForContentRect(CGRect rect)
		{
			nfloat width = 0;
			nfloat height = 0;
			if (CurrentImage != null)
			{
				width = CurrentImage.Size.Width;
				height = CurrentImage.Size.Height;
			}

			return new CGRect(
				rect.Right - width - ImageEdgeInsets.Right,
				rect.Top + rect.Height / 2 - height / 2,
				width,
				height
			);
		}
	}
}