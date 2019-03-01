using System;
using CoreGraphics;

namespace Xmf2.Core.iOS.Controls
{
	public class UILeftImageButton : UIBackgroundHighlightButton
	{
		public UILeftImageButton()
		{
			TitleLabel.AdjustsFontSizeToFitWidth = true;
		}

		public override CGRect ImageRectForContentRect(CGRect rect)
		{
			var imageContentRect = base.ImageRectForContentRect(rect);
			imageContentRect.X = (ImageEdgeInsets.Left + ImageEdgeInsets.Right + CurrentImage.Size.Width) / 2;
			return imageContentRect;
		}

		public override CGRect TitleRectForContentRect(CGRect rect)
		{
			var titleContentRect = base.TitleRectForContentRect(rect);

			if (CurrentImage != null)
			{
				titleContentRect.X = ImageRectForContentRect(titleContentRect).GetMaxX() + ImageEdgeInsets.Right + TitleEdgeInsets.Left;
			}
			return titleContentRect;
		}
	}
}