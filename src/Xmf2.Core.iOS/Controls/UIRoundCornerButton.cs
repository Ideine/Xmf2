using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UIRoundCornerButton : UIButton
	{
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			Layer.CornerRadius = Bounds.Size.Width / 2f;//Button will appear round if Width/Height ratio is 1/1.
			Layer.MasksToBounds = true;
		}
	}
}
