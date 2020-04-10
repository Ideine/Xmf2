using System;
using CoreGraphics;
using Foundation;
using UIKit;
using Xmf2.iOS.Extensions.Constraints;

namespace Xmf2.Components.iOS.CollectionView
{
	public class CollectionViewItemCell : UICollectionViewCell
	{
		public const string CELL_IDENTIFIER = nameof(CollectionViewItemCell);
		public static readonly NSString NsCellIdentifier = new NSString(CELL_IDENTIFIER);

		private UIView _contentView;
		private float? _width;

		protected CollectionViewItemCell(IntPtr handle) : base(handle) { }

		public void SetContent(UIView contentView)
		{
			_contentView = contentView;

			ContentView.AddSubview(_contentView);

			AutoLayout();
		}

		protected virtual void AutoLayout()
		{
			ContentView.CenterAndFillWidth(_contentView)
				.CenterAndFillHeight(_contentView);
		}

		public void UseWidth(float? cellWidth)
		{
			_width = cellWidth;
		}

		public override UICollectionViewLayoutAttributes PreferredLayoutAttributesFittingAttributes(UICollectionViewLayoutAttributes layoutAttributes)
		{
			UICollectionViewLayoutAttributes autoLayoutAttributes = base.PreferredLayoutAttributesFittingAttributes(layoutAttributes);
			if (_width != null)
			{
				CGSize targetSize = new CGSize(_width.Value, 0);
				var autoLayoutSize = ContentView.SystemLayoutSizeFittingSize(targetSize, (float)UILayoutPriority.Required, (float)UILayoutPriority.DefaultLow);
				CGRect autoLayoutFrame = new CGRect(autoLayoutAttributes.Frame.Location, autoLayoutSize);
				autoLayoutAttributes.Frame = autoLayoutFrame;
			}
			return autoLayoutAttributes;
		}
	}
}