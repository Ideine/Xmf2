using System;
using CoreGraphics;
using UIKit;

namespace Xmf2.Core.iOS.Controls
{
	/// <summary>
	/// UICollectionView with dynamic size
	/// From : Habitat
	/// </summary>
	public class DynamicCollectionView : UICollectionView
	{
		protected internal DynamicCollectionView(IntPtr handle) : base(handle) { }

		public DynamicCollectionView(CGRect frame, UICollectionViewLayout layout) : base(frame, layout) { }

		public override CGSize IntrinsicContentSize => ContentSize;

		public override void LayoutSubviews()
		{
			if (Bounds.Size != IntrinsicContentSize)
			{
				InvalidateIntrinsicContentSize();
			}
			
			try
			{
				base.LayoutSubviews();
			}
			catch(Exception ex)
			{
				//hidden on purpose, crash only seems to happen on iPhone X, waiting for a workaround
				System.Diagnostics.Debug.WriteLine($"DynamicCollectionView.base.LayoutSubviews crash : {ex}");
			}
		}
	}
}