using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
	public class UIEdgeableLabel : UILabel
	{
		public UIEdgeableLabel() : base() { }
		public UIEdgeableLabel(NSCoder coder) : base(coder) { }
		public UIEdgeableLabel(CGRect frame) : base(frame) { }
		protected UIEdgeableLabel(NSObjectFlag t) : base(t) { }

		private UIEdgeInsets _edgeInsets = UIEdgeInsets.Zero;
		public UIEdgeInsets EdgeInsets
		{
			get { return _edgeInsets; }
			set
			{
				_edgeInsets = value;
				this.InvalidateIntrinsicContentSize();
			}
		}

		public override CGRect TextRectForBounds(CGRect bounds, nint numberOfLines)
		{
			var rect = base.TextRectForBounds(EdgeInsets.InsetRect(bounds), numberOfLines);
			return new CGRect(x: rect.X - EdgeInsets.Left,
							  y: rect.Y - EdgeInsets.Top,
							  width: rect.Width + EdgeInsets.Left + EdgeInsets.Right,
							  height: rect.Height + EdgeInsets.Top + EdgeInsets.Bottom);
		}

		public override void DrawText(CGRect rect)
		{
			base.DrawText(this.EdgeInsets.InsetRect(rect));
		}

		public UIEdgeableLabel WithEdgeInsets(nfloat top, nfloat left, nfloat bottom, nfloat right)
		{
			this.EdgeInsets = new UIEdgeInsets(top, left, bottom, right);
			return this;
		}
	}
}
