using CoreGraphics;
using System;
using UIKit;

namespace Xmf2.Commons.iOS.Controls
{
	public class UISwappedImageButton : UIHighlightButton
	{
		private static readonly CGAffineTransform _flipTransform = CGAffineTransform.MakeScale(-1.0f, 1.0f);
		public UISwappedImageButton() : base()
		{
			this.Transform = _flipTransform;
			this.TitleLabel.Transform = _flipTransform;
			this.ImageView.Transform = _flipTransform;
		}

		[Obsolete("Use SwappedTitleEdgeInsets instead")]
		public new UIEdgeInsets TitleEdgeInsets { get => base.TitleEdgeInsets; set => base.TitleEdgeInsets = value; }

		[Obsolete("Use SwappedImageEdgeInsets instead")]
		public new UIEdgeInsets ImageEdgeInsets { get => base.ImageEdgeInsets; set => base.ImageEdgeInsets = value; }

		[Obsolete("Use SwappedHorizontalAlignment instead")]
		public new UIControlContentHorizontalAlignment HorizontalAlignment { get => base.HorizontalAlignment; set => base.HorizontalAlignment = value; }

		public virtual UIEdgeInsets SwappedTitleEdgeInsets
		{
			get => SwapHorizontally(base.TitleEdgeInsets);
			set => base.TitleEdgeInsets = SwapHorizontally(value);
		}
		public virtual UIEdgeInsets SwappedImageEdgeInsets
		{
			get => SwapHorizontally(base.ImageEdgeInsets);
			set => base.ImageEdgeInsets = SwapHorizontally(value);
		}

		public virtual UIControlContentHorizontalAlignment SwappedHorizontalAlignment
		{
			get => SwapHorizontally(base.HorizontalAlignment);
			set => base.HorizontalAlignment = SwapHorizontally(value);
		}

		private static UIEdgeInsets SwapHorizontally(UIEdgeInsets inset)
		{
			return new UIEdgeInsets(inset.Top, inset.Right, inset.Bottom, inset.Left);
		}
		private static UIControlContentHorizontalAlignment SwapHorizontally(UIControlContentHorizontalAlignment hAlignement)
		{
			switch (hAlignement)
			{
				case UIControlContentHorizontalAlignment.Left:	return UIControlContentHorizontalAlignment.Right;
				case UIControlContentHorizontalAlignment.Right:	return UIControlContentHorizontalAlignment.Left;

				case UIControlContentHorizontalAlignment.Leading:
				case UIControlContentHorizontalAlignment.Trailing:
					throw new NotImplementedException();//TODO: Implémenter ces cas.

				default:
				case UIControlContentHorizontalAlignment.Center:
				case UIControlContentHorizontalAlignment.Fill:
					return hAlignement;
			}
		}
	}
}
