#if NET7_0_OR_GREATER
using System.Runtime.InteropServices;
using ObjCRuntime;
#else
using NFloat = System.nfloat;
#endif
using CoreGraphics;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UIRightImageButton : UIButton
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

			NFloat x = rect.X + TitleEdgeInsets.Left;

			return new CGRect(
				x,
				rect.Y + TitleEdgeInsets.Top,
				imageRect.X - ImageEdgeInsets.Left - TitleEdgeInsets.Right - x,
				rect.Height - TitleEdgeInsets.Top - TitleEdgeInsets.Bottom
			);
		}

		public override CGRect ImageRectForContentRect(CGRect rect)
		{
			NFloat width = 0;
			NFloat height = 0;
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