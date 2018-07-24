using System;
using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public class UIHighlightButton : UIButton
	{
		private UIColor _oldBackgroundColor;

		public UIColor HighlightColor { get; set; } = 0xa1aeb3.ColorFromHex();

		public Action<UIHighlightButton> ToHighlightedAnimation { get; set; }
		public Action<UIHighlightButton> FromHighlightedAnimation { get; set; }

		public Action<UIHighlightButton> FromSelectedAnimation { get; set; }
		public Action<UIHighlightButton> ToSelectedAnimation { get; set; }

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
						Animate(0.2, () =>
						{
							if (ToHighlightedAnimation == null)
							{
								ToHighlightedDefaultAnimation();
							}
							else
							{
								ToHighlightedAnimation(this);
							}
						}, ActionHelper.NoOp);
					}
					else
					{
						Animate(0.2, () =>
						{
							if (FromHighlightedAnimation == null)
							{
								FromHighlightedDefaultAnimation();
							}
							else
							{
								FromHighlightedAnimation(this);
							}
						}, ActionHelper.NoOp);
					}
					base.Highlighted = value;
				}
			}
		}

		public override bool Selected
		{
			get => base.Selected;
			set
			{
				if (base.Selected != value)
				{
					if (value)
					{
						Animate(0.2, () => ToSelectedAnimation?.Invoke(this), ActionHelper.NoOp);
					}
					else
					{
						Animate(0.2, () => FromSelectedAnimation?.Invoke(this), ActionHelper.NoOp);
					}
					base.Selected = value;
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
