using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UIBaseHighlightSelectedButton : UIButton
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
				StateUpdate(Selected, value);
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
				StateUpdate(value, Highlighted);
			}
		}

		protected virtual void StateUpdate(bool selected, bool highlighted)
		{
			if (selected)
			{
				OnSelected();
			}
			else if (highlighted)
			{
				OnHighlighted();
			}
			else
			{
				OnNormal();
			}
		}

		protected virtual void OnNormal() {}
		protected virtual void OnHighlighted() {}
		protected virtual void OnSelected() {}
	}
}