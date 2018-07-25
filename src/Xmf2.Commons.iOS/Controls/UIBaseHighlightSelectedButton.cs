using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public abstract class UIBaseHighlightSelectedButton : UIButton
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
				StateUpdate();
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
				StateUpdate();
			}
		}

		protected virtual void StateUpdate()
		{
			if (Selected)
			{
				OnSelected();
			}
			else if (Highlighted)
			{
				OnHighlighted();
			}
			else
			{
				OnNormal();
			}
		}

		protected abstract void OnNormal();
		protected abstract void OnHighlighted();
		protected abstract void OnSelected();
	}
}