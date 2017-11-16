using System;
using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public class UIHighlightButton : UIButton
	{
		private UIColor _oldBackgroundColor;

		public UIColor HighlightColor { get; set; } = 0xa1aeb3.ColorFromHex();

		public Action ToHighlightedAnimation { get; set; }
		public Action FromHighlightedAnimation { get; set; }

		public override bool Highlighted
		{
			get
			{
				return base.Highlighted;
			}
			set
			{
				if (base.Highlighted != value)
				{
					if (value)
					{
						_oldBackgroundColor = BackgroundColor;
						Animate(0.2, ToHighlightedAnimation ?? ToHighlightedDefaultAnimation, ActionHelper.NoOp);
					}
					else
					{
						Animate(0.2, FromHighlightedAnimation ?? FromHighlightedDefaultAnimation, ActionHelper.NoOp);
					}
					base.Highlighted = value;
				}
			}
		}

		private void ToHighlightedDefaultAnimation()
		{
			BackgroundColor = HighlightColor;
		}
		private void FromHighlightedDefaultAnimation()
		{
			BackgroundColor = _oldBackgroundColor;
		}
	}
}
