using UIKit;
using Xmf2.Core.Helpers;

namespace Xmf2.Core.iOS.Controls
{
	public class UIBackgroundHighlightButton : UIBaseHighlightButton, IUIBackgroundHighlight
	{
		private UIColor _previousBackgroundColor;

		public UIColor BackgroundHightlightedColor { get; set; }

		protected override void OnHighlighted()
		{
			_previousBackgroundColor = BackgroundColor;
			TransitionBackgroundColor(BackgroundHightlightedColor);
		}

		protected override void OnUnhighlighted()
		{
			TransitionBackgroundColor(_previousBackgroundColor);
		}

		protected override void OnUnselected() { }
		protected override void OnSelected() { }

		protected void TransitionBackgroundColor(UIColor to)
		{
			Animate(0.15, () =>
			{
				this.WithBackgroundColor(to);
			}, ActionHelper.NoOp);
		}
	}
}