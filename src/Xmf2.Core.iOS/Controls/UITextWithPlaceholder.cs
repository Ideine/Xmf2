#if NET7_0_OR_GREATER
using System.Runtime.InteropServices;
using ObjCRuntime;
#else
using NFloat = System.nfloat;
#endif
using System;
using CoreGraphics;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UITextViewWithPlaceHolder : UITextView
	{
		private readonly UILabel _placeholderLabel;

		public UITextViewWithPlaceHolder()
		{
			_placeholderLabel = new UILabel()
			{
				BackgroundColor = UIColor.Clear,
				LineBreakMode = UILineBreakMode.WordWrap,
				TextAlignment = UITextAlignment.Natural,
				Lines = 0
			};
			PlaceholderColor = UIColor.Gray;
			Started += OnStarted;
			Ended += OnEnded;
			Add(_placeholderLabel);
		}

		public UILabel Placeholder => _placeholderLabel;
		public string PlaceholderText
		{
			get => _placeholderLabel.Text;
			set
			{
				_placeholderLabel.Text = value;
				DrawPlaceholder();
			}
		}
		public override string Text
		{
			set
			{
				base.Text = value;
				UpdatePlaceHolderVisibility();
			}
		}
		public UIColor PlaceholderColor
		{
			get => _placeholderLabel.TextColor;
			set => _placeholderLabel.TextColor = value;
		}

		public UIFont PlaceholderFont
		{
			get => _placeholderLabel.Font;
			set => _placeholderLabel.Font = value;
		}

		public override void Draw(CGRect rect)
		{
			base.Draw(rect);
			DrawPlaceholder();
		}

		private void DrawPlaceholder()
		{
			UIEdgeInsets inset = TextContainerInset;
			NFloat leftInset = TextContainer.LineFragmentPadding + inset.Left;
			NFloat rightInset = TextContainer.LineFragmentPadding + inset.Right;
			var placeHolderMaxSize = new CGSize(width: Frame.Width - (leftInset + rightInset)
				, height: Frame.Height - (inset.Top + inset.Bottom));
			_placeholderLabel.Frame = new CGRect(new CGPoint(leftInset, inset.Top), placeHolderMaxSize);
			_placeholderLabel.SizeToFit();
		}

		private void OnStarted(object sender, EventArgs e)
		{
			_placeholderLabel.Hidden = true;
		}

		private void OnEnded(object sender, EventArgs e)
		{
			UpdatePlaceHolderVisibility();
		}

		private void UpdatePlaceHolderVisibility()
		{
			_placeholderLabel.Hidden = !string.IsNullOrWhiteSpace(Text);
		}

		public void AutoHeight()
		{
			NFloat fixedWidth = Frame.Size.Width;
			CGSize newSize = SizeThatFits(new CGSize(width: fixedWidth, height: NFloat.MaxValue));
			CGRect newFrame = Frame;
			newFrame.Size = new CGSize(width: Math.Max(newSize.Width, fixedWidth), height: newSize.Height);
			Frame = newFrame;
		}
	}
}