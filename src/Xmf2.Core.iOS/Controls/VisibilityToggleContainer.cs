using System;
using UIKit;
using Xmf2.Core.Subscriptions;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Core.iOS.Controls
{
	public class VisibilityToggleContainer : UIView
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();

		private UIEdgeInsets _insets = UIEdgeInsets.Zero;

		private UIView _child;
		private NSLayoutConstraint _topConstraint;
		private NSLayoutConstraint _bottomConstraint;
		private NSLayoutConstraint _leftConstraint;
		private NSLayoutConstraint _rightConstraint;
		private NSLayoutConstraint _emptyHeightConstraint;
		private NSLayoutConstraint _emptyWidthConstraint;

		public override bool Hidden
		{
			get => base.Hidden;
			set
			{
				base.Hidden = value;
				ShowChildView(!value, _child);
			}
		}

		public bool Visible
		{
			get => !base.Hidden;
			set
			{
				base.Hidden = !value;
				ShowChildView(value, _child);
			}
		}

		public VisibilityToggleContainer()
		{

		}
		public VisibilityToggleContainer(UIView child)
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
			_topConstraint 	  = NSLayoutConstraint.Create(this, Top, 	Equal, _child, Top	 , 1, -_insets.Top).DisposeWith(_disposable);
			_bottomConstraint = NSLayoutConstraint.Create(this, Bottom, Equal, _child, Bottom, 1,  _insets.Bottom).DisposeWith(_disposable);
			_leftConstraint   = NSLayoutConstraint.Create(this, Left, 	Equal, _child, Left	 , 1, -_insets.Left).DisposeWith(_disposable);
			_rightConstraint  = NSLayoutConstraint.Create(this, Right, 	Equal, _child, Right , 1,  _insets.Right).DisposeWith(_disposable);

			_topConstraint	 .SetIdentifier($"{nameof(VisibilityToggleContainer)}.{nameof(_topConstraint)}");
			_bottomConstraint.SetIdentifier($"{nameof(VisibilityToggleContainer)}.{nameof(_bottomConstraint)}");
			_leftConstraint	 .SetIdentifier($"{nameof(VisibilityToggleContainer)}.{nameof(_leftConstraint)}");
			_rightConstraint .SetIdentifier($"{nameof(VisibilityToggleContainer)}.{nameof(_rightConstraint)}");

			ShowChildView(this.Visible, _child);
		}

		public VisibilityToggleContainer WithChild(UIView child)
		{
			SetChild(child);
			return this;
		}
		public VisibilityToggleContainer WithEmptyHeightConstraint(NSLayoutConstraint customHeightConstraint = null)
		{
			_emptyHeightConstraint?.Disable();
			_emptyHeightConstraint = customHeightConstraint
								  ?? NSLayoutConstraint.Create(this, Height, LessThanOrEqual, 1f, 0f).DisposeWith(_disposable).WithAutomaticIdentifier();
			return this;
		}
		public VisibilityToggleContainer WithEmptyWidthConstraint(NSLayoutConstraint customWidthConstraint = null)
		{
			_emptyWidthConstraint?.Disable();
			_emptyWidthConstraint = customWidthConstraint
								  ?? NSLayoutConstraint.Create(this, Width, LessThanOrEqual, 1f, 0f).DisposeWith(_disposable).WithAutomaticIdentifier();
			return this;
		}
		public VisibilityToggleContainer WithContentInsets(nfloat top, nfloat left, nfloat bottom, nfloat right)
		{
			return WithContentInsets(new UIEdgeInsets(top, left, bottom, right));
		}
		public VisibilityToggleContainer WithContentInsets(UIEdgeInsets insets)
		{
			_insets = insets;
			if (_topConstraint 	  != null) { _topConstraint.Constant 	= -insets.Top; }
			if (_bottomConstraint != null) { _bottomConstraint.Constant =  insets.Bottom; }
			if (_leftConstraint   != null) { _leftConstraint.Constant 	= -insets.Left; }
			if (_rightConstraint  != null) { _rightConstraint.Constant 	=  insets.Right; }
			return this;
		}

		private void ShowChildView(bool visible, UIView child)
		{
			if (visible && child != null)
			{
				AddSubview(child);
				if (_emptyHeightConstraint != null)
				{
					RemoveConstraint(_emptyHeightConstraint);
				}
				if (_emptyWidthConstraint != null)
				{
					RemoveConstraint(_emptyWidthConstraint);
				}
				AddConstraints(new[] { _topConstraint, _bottomConstraint, _leftConstraint, _rightConstraint });
			}
			else
			{
				this.EnsureRemove(_topConstraint, _bottomConstraint, _leftConstraint, _rightConstraint);
				if (_emptyHeightConstraint != null)
				{
					AddConstraint(_emptyHeightConstraint);
				}
				if (_emptyWidthConstraint != null)
				{
					AddConstraint(_emptyWidthConstraint);
				}
				child?.RemoveFromSuperview();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_child = null;
				_topConstraint = null;
				_bottomConstraint = null;
				_leftConstraint = null;
				_rightConstraint = null;
				_emptyHeightConstraint = null;
				_emptyWidthConstraint = null;
				_disposable.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
