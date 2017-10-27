using CoreGraphics;

namespace Xmf2.Commons.iOS.Controls
{
	public class UIRightImageButton : UIHighlightButton
	{
		public override CGRect TitleRectForContentRect(CGRect rect)
		{
			var titleRect = base.TitleRectForContentRect(rect);
			titleRect.X = base.ImageRectForContentRect(rect).X;
			return titleRect;
		}

		public override CGRect ImageRectForContentRect(CGRect rect)
		{
			var originalImageRect = base.ImageRectForContentRect(rect);
			originalImageRect.X = (originalImageRect.X + base.TitleRectForContentRect(rect).Width);
			return originalImageRect;
		}
	}
}
