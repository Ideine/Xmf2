﻿using System;
using CoreGraphics;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	public class UISwappedImageButton : UIButton
	{
		private static readonly CGAffineTransform _flipTransform = CGAffineTransform.MakeScale(-1.0f, 1.0f);

		public UISwappedImageButton()
		{
			Transform = _flipTransform;
			TitleLabel.Transform = _flipTransform;
			ImageView.Transform = _flipTransform;
		}

		[Obsolete("Use SwappedTitleEdgeInsets instead")]
		public new UIEdgeInsets TitleEdgeInsets
		{
			get => base.TitleEdgeInsets;
			set => base.TitleEdgeInsets = value;
		}

		[Obsolete("Use SwappedImageEdgeInsets instead")]
		public new UIEdgeInsets ImageEdgeInsets
		{
			get => base.ImageEdgeInsets;
			set => base.ImageEdgeInsets = value;
		}

		[Obsolete("Use SwappedHorizontalAlignment instead")]
		public new UIControlContentHorizontalAlignment HorizontalAlignment
		{
			get => base.HorizontalAlignment;
			set => base.HorizontalAlignment = value;
		}

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

		private static UIControlContentHorizontalAlignment SwapHorizontally(UIControlContentHorizontalAlignment hAlignement) => hAlignement switch
		{
			UIControlContentHorizontalAlignment.Left => UIControlContentHorizontalAlignment.Right,
			UIControlContentHorizontalAlignment.Right => UIControlContentHorizontalAlignment.Left,
			UIControlContentHorizontalAlignment.Leading => throw new NotImplementedException() //TODO: Implémenter ces cas.
			,
			UIControlContentHorizontalAlignment.Trailing => throw new NotImplementedException() //TODO: Implémenter ces cas.
			,
			_ => hAlignement
		};
	}
}