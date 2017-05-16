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
			this._placeholderLabel = new UILabel()
			{
				BackgroundColor = UIColor.Clear,
				LineBreakMode = UILineBreakMode.WordWrap,
				TextAlignment = UITextAlignment.Natural,
				Lines = 0
			};
			this.PlaceholderColor = UIColor.Gray;
			this.Started += this.OnStarted;
			this.Ended += this.OnEnded;
			this.Add(this._placeholderLabel);
		}

		public UILabel Placeholder { get { return _placeholderLabel; } }
		public string PlaceholderText
		{
			get { return _placeholderLabel.Text; }
			set
			{
				_placeholderLabel.Text = value;
				this.DrawPlaceholder();
			}
		}
		public override string Text
		{
			set
			{
				base.Text = value;
				this.UpdatePlaceHolderVisibility();
			}
		}
		public UIColor PlaceholderColor { get { return _placeholderLabel.TextColor; } set { _placeholderLabel.TextColor = value; } }
		public UIFont PlaceholderFont { get { return _placeholderLabel.Font; } set { _placeholderLabel.Font = value; } }

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			this.DrawPlaceholder();
		}

		private void DrawPlaceholder()
		{
			var inset = this.TextContainerInset;
			var leftInset = this.TextContainer.LineFragmentPadding + inset.Left;
			var rightInset = this.TextContainer.LineFragmentPadding + inset.Right;
			var placeHolderMaxSize = new CGSize(width: this.Frame.Width - (leftInset + rightInset)
											 , height: this.Frame.Height - (inset.Top + inset.Bottom));
			this._placeholderLabel.Frame = new CGRect(new CGPoint(leftInset, inset.Top), placeHolderMaxSize);
			this._placeholderLabel.SizeToFit();
		}

		private void OnStarted(object sender, EventArgs e)
		{
			this._placeholderLabel.Hidden = true;
		}

		private void OnEnded(object sender, EventArgs e)
		{
			this.UpdatePlaceHolderVisibility();
		}

		private void UpdatePlaceHolderVisibility()
		{
			this._placeholderLabel.Hidden = !string.IsNullOrWhiteSpace(this.Text);
		}

        public void AutoHeight(){
            nfloat fixedWidth = Frame.Size.Width;
            var newSize=SizeThatFits(new CGSize(width: fixedWidth, height: nfloat.MaxValue));
            var newFrame = Frame;
            newFrame.Size = new CGSize(width: Math.Max(newSize.Width, fixedWidth), height: newSize.Height);
            Frame = newFrame;
        }
	}
}