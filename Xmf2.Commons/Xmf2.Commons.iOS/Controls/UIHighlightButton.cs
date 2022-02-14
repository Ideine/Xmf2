using UIKit;
using Xmf2.Commons.iOS.Extensions;
using Xmf2.iOS.Extensions.Extensions;

namespace Xmf2.Commons.iOS.Controls
{
	public class UIHighlightButton : UIButton
	{
		private UIColor _oldBackgroundColor;

		public UIColor HighlightColor { get; set; } = 0xa1aeb3.ColorFromHex();

		public override bool Highlighted
		{
			get => base.Highlighted;
			set
			{
				if (base.Highlighted != value)
				{
					if (value)
					{
						_oldBackgroundColor = BackgroundColor;
						Animate(0.2, () => BackgroundColor = HighlightColor, ActionHelper.NoOp);
					}
					else
					{
						Animate(0.2, () => BackgroundColor = _oldBackgroundColor, ActionHelper.NoOp);
					}

					base.Highlighted = value;
				}
			}
		}
	}
}