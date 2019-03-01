using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public abstract class UIBaseHighlightButton : UIButton
	{
		public override bool Highlighted
		{
			get => base.Highlighted;
			set
			{
				if (Highlighted == value)
				{
					return;
				}

				base.Highlighted = value;
				if (value)
				{
					OnHighlighted();
				}
				else
				{
					OnUnhighlighted();
				}
			}
		}

		public override bool Selected 
		{
			get => base.Selected;
			set
			{
				if (Selected == value)
				{
					return;
				}

				base.Selected = value;
				if (value)
				{
					OnSelected();
				}
				else
				{
					OnUnselected();
				}
			}
		}

		protected abstract void OnHighlighted();
		protected abstract void OnUnhighlighted();

		protected abstract void OnSelected();
		protected abstract void OnUnselected();
	}
}