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
		public override UIEdgeInsets TitleEdgeInsets { get => base.TitleEdgeInsets; set => base.TitleEdgeInsets = value; }

		[Obsolete("Use SwappedImageEdgeInsets instead")]
		public override UIEdgeInsets ImageEdgeInsets { get => base.ImageEdgeInsets; set => base.ImageEdgeInsets = value; }

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

		private static UIEdgeInsets SwapHorizontally(UIEdgeInsets inset)
		{
			return new UIEdgeInsets(inset.Top, inset.Right, inset.Bottom, inset.Left);
		}
	}
}
