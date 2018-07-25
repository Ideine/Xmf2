using System;
using UIKit;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Commons.iOS.Controls
{
	public class VisibilityToggleContainer : UIView
	{
		private UIView _child;
		private NSLayoutConstraint[] _constraints;
		private NSLayoutConstraint _emptyHeightConstraint;

		public bool Visible
		{
			get => _child.Superview != null;
			set => ShowChildView(value);
		}

		public VisibilityToggleContainer(bool useEmptyHeightConstraint = false)
		{
			if (useEmptyHeightConstraint)
			{
				_emptyHeightConstraint = HeightAnchor.ConstraintEqualTo(0);
				AddConstraint(_emptyHeightConstraint);
			}
		}

		public VisibilityToggleContainer(UIView child, bool useEmptyHeightConstraint = false) : this(useEmptyHeightConstraint)
		{
			SetChild(child);
		}

		public void SetChild(UIView child)
		{
			if(_child != null)
			{
				throw new InvalidOperationException("Child has already been set");
			}
			_child = child;
			_child.TranslatesAutoresizingMaskIntoConstraints = false;
			
			_constraints = new[]
			{
				NSLayoutConstraint.Create(this, Width, Equal, child, Width, 1, 0),
				NSLayoutConstraint.Create(this, CenterX, Equal, _child, CenterX, 1, 0),
				NSLayoutConstraint.Create(this, Height, Equal, _child, Height, 1, 0),
				NSLayoutConstraint.Create(this, CenterY, Equal, _child, CenterY, 1, 0)
			};
		}

        public VisibilityToggleContainer WithChild(UIView child)
        {
            SetChild(child);
            return this;
        }

		private void ShowChildView(bool value)
		{
			if (Visible == value)
			{
				return;
			}

			if (value)
			{
				AddSubview(_child);
				if (_emptyHeightConstraint != null)
				{
					RemoveConstraint(_emptyHeightConstraint);
				}
				AddConstraints(_constraints);
			}
			else
			{
				RemoveConstraints(_constraints);
				if (_emptyHeightConstraint != null)
				{
					AddConstraint(_emptyHeightConstraint);
				}
				_child.RemoveFromSuperview();
			}
		}
	}
}