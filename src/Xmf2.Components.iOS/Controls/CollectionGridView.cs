using System;
using UIKit;
using Xmf2.Core.LinearLists;
using Xmf2.Core.Subscriptions;
using Xmf2.Components.iOS.Views;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.iOS.CollectionView;
using Foundation;
using Xmf2.Core.iOS.Controls;
using CoreGraphics;

namespace Xmf2.Components.iOS.Controls
{
	public abstract class CollectionGridView<TComponentView> : BaseComponentView<ListViewState> where TComponentView : IComponentView
	{
		protected UICollectionView GridView;

		private CollectionViewItemSource<CollectionViewItemCell> _source;
		private UICollectionViewFlowLayout _layout;

		protected virtual UIColor BackgroundColor { get; } = UIColor.White;

		protected virtual float MinimumLineSpacing => 8;

		protected virtual float MinimumColumnSpacing => 0;

		protected virtual UIEdgeInsets SectionInset => new UIEdgeInsets(12, 0, 12, 0);

		protected virtual int NbColumns => 2;

		protected virtual NSString CellIdentifier => CollectionViewItemCell.NsCellIdentifier;

		protected abstract float EstimatedCellHeight { get; }

		protected float TotalRowSpacing => MinimumColumnSpacing * (NbColumns - 1);

		public CollectionGridView(IServiceLocator services) : base(services)
		{
			var totalWidthForCells = GetCollectionViewFrame().Width
									   - TotalRowSpacing
									   - SectionInset.Left
									   - SectionInset.Right;
			float cellWidth = (float)Math.Floor(totalWidthForCells / NbColumns);

			_layout = CreateCollectionViewFlowLayout().DisposeViewWith(Disposables);

			var itemSize = GetCellSize(forWidth: cellWidth);
			if (itemSize != null)
			{
				_layout.ItemSize = itemSize.Value;
				_layout.MinimumInteritemSpacing = 0f;//cette taille sera induite par la largeur de l'item size.
			}
			else
			{
				_layout.EstimatedItemSize = new CGSize(cellWidth, EstimatedCellHeight);
				_layout.MinimumInteritemSpacing = MinimumColumnSpacing;
			}

			GridView = new DynamicCollectionView(GetCollectionViewFrame(), _layout)
			{
				Bounces = true,
				ScrollEnabled = true,
				PagingEnabled = false,
				BackgroundColor = BackgroundColor
			}.DisposeViewWith(Disposables);

			_source = CreateSource(GridView).DisposeWith(Disposables);

			GridView.Source = _source;
			GridView.RegisterClassForCell(typeof(CollectionViewItemCell), CellIdentifier);
		}

		#region Virtual methods

		protected abstract IComponentView ViewFactory(IServiceLocator s);

		protected virtual UICollectionViewFlowLayout CreateCollectionViewFlowLayout()
		{
			return new FlowLayout
			{
				MinimumLineSpacing = MinimumLineSpacing,
				ScrollDirection = UICollectionViewScrollDirection.Vertical,
				SectionInset = SectionInset
			};
		}

		/// <summary>
		/// Called during the constructor.
		/// Don't try to access to any elements !
		/// </summary>
		/// <returns>The frame for the collection view</returns>
		protected abstract CGRect GetCollectionViewFrame();

		/// <summary>
		/// WARNING, be careful called from consructor.
		/// </summary>
		protected virtual CGSize? GetCellSize(float forWidth) => null;

		protected virtual CollectionViewItemSource<CollectionViewItemCell> CreateSource(UICollectionView listView)
		{
			return new CollectionViewItemSource<CollectionViewItemCell>(
				listView,
                id => ViewFactory(Services).DisposeViewWith(Disposables),
				CollectionViewItemCell.NsCellIdentifier).DisposeWith(Disposables);
		}

		#endregion

		protected override UIView RenderView() => GridView;

		protected override void OnStateUpdate(ListViewState state)
		{
			base.OnStateUpdate(state);
			_source.ItemSource = state.Items;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				GridView = null;
				_source = null;
			}
			base.Dispose(disposing);
		}


		#region Nested class

		private class FlowLayout : UICollectionViewFlowLayout
		{
			private CGRect _lastFrame = new CGRect(0, 0, 0, 0);

			public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
			{
				if (AreTheSame(newBounds, _lastFrame))
				{
					return false;
				}
				_lastFrame = newBounds;
				return true;
			}

			public override bool ShouldInvalidateLayout(UICollectionViewLayoutAttributes preferredAttributes, UICollectionViewLayoutAttributes originalAttributes)
			{
				return base.ShouldInvalidateLayout(preferredAttributes, originalAttributes);
			}

			private static bool AreTheSame(CGRect frame1, CGRect frame2)
			{
				return frame1.X != frame2.X
					|| frame1.Y != frame2.Y
					|| frame1.Width != frame2.Width
					|| frame1.Height != frame2.Height;
			}
		}

		#endregion
	}
}

