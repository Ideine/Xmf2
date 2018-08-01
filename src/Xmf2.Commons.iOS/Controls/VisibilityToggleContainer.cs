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

		public void SetChild(UIView child, UIEdgeInsets? pInsets = null)
		{
			if(_child != null)
			{
				throw new InvalidOperationException("Child has already been set");
			}
			_child = child;
			_child.TranslatesAutoresizingMaskIntoConstraints = false;

			var insets = pInsets.HasValue ? pInsets.Value : UIEdgeInsets.Zero;

			_constraints = new[]
			{
				NSLayoutConstraint.Create(this, Left, 	Equal, _child, Left	  , 1f, -insets.Left),
				NSLayoutConstraint.Create(this, Right, 	Equal, _child, Right  , 1f,  insets.Right),
				NSLayoutConstraint.Create(this, Top, 	Equal, _child, Top	  , 1f, -insets.Top),
				NSLayoutConstraint.Create(this, Bottom, Equal, _child, Bottom , 1f,  insets.Bottom)
			};
		}

		public VisibilityToggleContainer WithChild(UIView child, UIEdgeInsets? pInsets = null)
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