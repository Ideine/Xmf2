using System;
using UIKit;

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
						UIView.Animate(0.2, () => BackgroundColor = HighlightColor, ActionHelper.NoOp);
					}
					else
					{
						UIView.Animate(0.2, () => BackgroundColor = _oldBackgroundColor, ActionHelper.NoOp);
					}

					base.Highlighted = value;
				}
			}
		}
	}
}
