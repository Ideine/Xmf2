using UIKit;
using Xmf2.Core.Helpers;

namespace Xmf2.Core.iOS.Controls
{
	public class UIBackgroundHighlightSelectedButton : UIBaseHighlightSelectedButton, IUIBackgroundHighlight, IUIBackgroundSelected
	{
		private UIColor _normalBackgroundColor;
		
		public UIColor BackgroundHightlightedColor { get; set; }
		public UIColor BackgroundSelectedColor { get; set; }

		protected override void OnNormal()
		{
			TransitionBackgroundColor(_normalBackgroundColor);
		}

		protected override void OnHighlighted()
		{
			SaveNormalBackground();
			TransitionBackgroundColor(BackgroundHightlightedColor);
		}

		protected override void OnSelected()
		{
			SaveNormalBackground();
			TransitionBackgroundColor(BackgroundSelectedColor);
		}

		private void SaveNormalBackground()
		{
			if (_normalBackgroundColor == null)
			{
				_normalBackgroundColor = BackgroundColor;
			}
		}

		protected void TransitionBackgroundColor(UIColor to)
		{
			Animate(0.15, () =>
			{
				this.WithBackgroundColor(to);
			}, ActionHelper.NoOp);
		}
	}
}