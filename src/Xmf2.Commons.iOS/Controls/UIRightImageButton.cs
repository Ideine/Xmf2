using CoreGraphics;

namespace Xmf2.Commons.iOS.Controls
{
	public class UIRightImageButton : UIHighlightButton
	{
		public override CGRect TitleRectForContentRect(CGRect rect)
		{
			var contentRect = base.TitleRectForContentRect(rect);
			contentRect.X = TitleEdgeInsets.Left;
			return contentRect;
		}

		public override CGRect ImageRectForContentRect(CGRect rect)
		{
			var contentRect = base.ImageRectForContentRect(rect);
			contentRect.X = rect.Width - ImageEdgeInsets.Right - CurrentImage.Size.Width;
			return contentRect;
		}
	}
}
