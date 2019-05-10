using System;
using UIKit;
using static UIKit.NSLayoutAttribute;
using static UIKit.NSLayoutRelation;

namespace Xmf2.Core.iOS.Controls
{
	public class UIFilledScrollView : UIScrollView
	{
		private bool _constraintsSet = false;
		private UIView _child;
		private NSLayoutConstraint _leftConstraint;
		private NSLayoutConstraint _rightConstraint;
		private NSLayoutConstraint _topConstraint;
		private NSLayoutConstraint _bottomConstraint;
		private NSLayoutConstraint _heighConstraint;
		private NSLayoutConstraint _centerXConstraint;

		public UIFilledScrollView()
		{
			this.TranslatesAutoresizingMaskIntoConstraints = false;
		}

		public override void SafeAreaInsetsDidChange()
		{
#pragma warning disable XI0002 //pas de warning sur le call de la base, car cette méthode ne sera de toute façon appelée que si on ...
			//...est sur la plateforme qui le supporte.
			base.SafeAreaInsetsDidChange();
			this.SetNeedsUpdateConstraints();
#pragma warning restore XI0002
		}

		public override void UpdateConstraints()
		{
			base.UpdateConstraints();
			if (_child != null)
			{
				var safeAreaInsets = UIDevice.CurrentDevice.CheckSystemVersion(11, 0)	? this.SafeAreaInsets
																						: UIEdgeInsets.Zero;
				if (!_constraintsSet)
				{
					_leftConstraint 	= NSLayoutConstraint.Create(this, Left, 	Equal, _child	, Left	, 1f, safeAreaInsets.Left);
					_rightConstraint 	= NSLayoutConstraint.Create(this, Right, 	Equal, _child	, Right	, 1f, safeAreaInsets.Right);
					_topConstraint		= NSLayoutConstraint.Create(this, Top, 		Equal, _child	, Top	, 1f, safeAreaInsets.Top);
					_bottomConstraint 	= NSLayoutConstraint.Create(this, Bottom,	Equal, _child	, Bottom, 1f, safeAreaInsets.Bottom);
					_heighConstraint 	= NSLayoutConstraint.Create(this, Height,	LessThanOrEqual	, _child, Height, 1f, 0f);
					_centerXConstraint 	= NSLayoutConstraint.Create(this, CenterX,	Equal, _child	, CenterX, 1f, 0f);

					_leftConstraint		.WithIdentifier($"{nameof(UIFilledScrollView)}-Left");
					_rightConstraint	.WithIdentifier($"{nameof(UIFilledScrollView)}-Right");
					_topConstraint		.WithIdentifier($"{nameof(UIFilledScrollView)}-Top");
					_bottomConstraint	.WithIdentifier($"{nameof(UIFilledScrollView)}-Bottom");
					_heighConstraint	.WithIdentifier($"{nameof(UIFilledScrollView)}-Height");
					_centerXConstraint	.WithIdentifier($"{nameof(UIFilledScrollView)}-CenterX");

					this.AddConstraints(
						_leftConstraint,
						_rightConstraint,
						_topConstraint,
						_bottomConstraint,
						_heighConstraint,
						_centerXConstraint
					);
					_constraintsSet = true;
				}
				else
				{
					_leftConstraint.Constant = safeAreaInsets.Left;
					_rightConstraint.Constant = safeAreaInsets.Right;
					_topConstraint.Constant = safeAreaInsets.Top;
					_bottomConstraint.Constant = safeAreaInsets.Bottom;
				}
			}
		}

		[Obsolete("Use WithContent instead", error: true)]
		public void WithSubviews(params UIView[] _discarded) { }

		public UIFilledScrollView WithContent(UIView child)
		{
			if (_child != null)
			{
				throw new InvalidOperationException("Child has already been set");
			}
			_child = child;
			_child.TranslatesAutoresizingMaskIntoConstraints = false;
			this.AddSubview(_child);
			return this;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.EnsureRemove(_leftConstraint, _rightConstraint, _topConstraint, _bottomConstraint, _heighConstraint, _centerXConstraint);
				_leftConstraint		?.Dispose();
				_rightConstraint	?.Dispose();
				_topConstraint		?.Dispose();
				_bottomConstraint	?.Dispose();
				_heighConstraint	?.Dispose();
				_centerXConstraint	?.Dispose();

				_leftConstraint 	= null;
				_rightConstraint	= null;
				_topConstraint 		= null;
				_bottomConstraint	= null;
				_heighConstraint 	= null;
				_centerXConstraint 	= null;
			}
			base.Dispose(disposing);
		}
	}
}
