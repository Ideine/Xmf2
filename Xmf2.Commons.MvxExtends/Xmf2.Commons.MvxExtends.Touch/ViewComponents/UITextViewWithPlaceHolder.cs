using System;
using CoreGraphics;
using UIKit;

namespace Xmf2.Commons.MvxExtends.Touch.ViewComponents
{
	public class UITextViewWithPlaceHolder : UITextView
	{
		private readonly UILabel _placeholderLabel;

		public UITextViewWithPlaceHolder() : base()
		{
			this._placeholderLabel = new UILabel() { BackgroundColor = UIColor.Clear };
			this.PlaceholderColor = UIColor.Gray;
			this.Started += this.OnStarted;
			this.Ended += this.OnEnded;
			this.Add(this._placeholderLabel);
		}

		public string Placeholder { get { return _placeholderLabel.Text; } set { _placeholderLabel.Text = value; } }
		public UIColor PlaceholderColor { get { return _placeholderLabel.TextColor; } set { _placeholderLabel.TextColor = value; } }
		public UIFont PlaceholderFont { get { return _placeholderLabel.Font; } set { _placeholderLabel.Font = value; } }

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			var inset	   = this.TextContainerInset;
			var leftInset  = this.TextContainer.LineFragmentPadding + inset.Left;
			var rightInset = this.TextContainer.LineFragmentPadding + inset.Right;
			var placeHolderMaxSize = new CGSize(width: this.Frame.Width  - (leftInset + rightInset)
											 , height: this.Frame.Height - (inset.Top + inset.Bottom));
			var size = _placeholderLabel.Text.StringSize(_placeholderLabel.Font, placeHolderMaxSize, UILineBreakMode.WordWrap);
			this._placeholderLabel.Frame = new CGRect(new CGPoint(leftInset, inset.Top), size);
		}

		private void OnStarted(object sender, EventArgs e)
		{
			this._placeholderLabel.Hidden = true;
		}

		private void OnEnded(object sender, EventArgs e)
		{
			this._placeholderLabel.Hidden = !string.IsNullOrWhiteSpace(this.Text);
		}
	}
}
