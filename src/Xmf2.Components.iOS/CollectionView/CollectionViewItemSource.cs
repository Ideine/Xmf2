using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Xmf2.Common.Collections;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Helpers;

namespace Xmf2.Components.iOS.CollectionView
{
	public class CollectionViewItemSource<TCell> : UICollectionViewSource
		where TCell : CollectionViewItemCell
	{
		private readonly NSString _cellIdentifier;
		private readonly float? _cellWidth;

		private UICollectionView _collectionView;

		private Func<string, IComponentView> _componentViewCreator;
		private Dictionary<TCell, IComponentView> _componentViews = new Dictionary<TCell, IComponentView>();

		private IReadOnlyList<IEntityViewState> _itemSource;

		public IReadOnlyList<IEntityViewState> ItemSource
		{
			get => _itemSource;
			set
			{
				bool changed = ListHelper.HasChange(_itemSource, value);

				_itemSource = value;

				if (changed || (_itemSource.Count > 0 && _collectionView.VisibleCells.None()))
				{
					_collectionView.ReloadData();
				}
				else
				{
					ReapplyStates();
				}
			}
		}

		public CollectionViewItemSource(UICollectionView collectionView, Func<string, IComponentView> componentViewCreator, NSString cellIdentifier, float? cellWidth = null)
		{
			_collectionView = collectionView;
			_componentViewCreator = componentViewCreator;
			_cellIdentifier = cellIdentifier;
			_cellWidth = cellWidth;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return ItemSource?.Count ?? 0;
		}

		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
		{
			TCell cell = (TCell)collectionView.DequeueReusableCell(_cellIdentifier, indexPath);
			cell.UseWidth(_cellWidth);

			IEntityViewState childState = ItemSource[indexPath.Row];
			bool cellContentAlreadySet = _componentViews.TryGetValue(cell, out IComponentView childView);
			if (!cellContentAlreadySet)
			{
				childView = _componentViewCreator(childState.Id.ToString());
				_componentViews.Add(cell, childView);
				cell.SetContent(childView.View);
			}

			childView.SetState(childState);
			return cell;
		}

		private void ReapplyStates()
		{
			foreach (var kvp in _componentViews)
			{
				var index = _collectionView.IndexPathForCell(kvp.Key);
				if (index != null)
				{
					kvp.Value.SetState(ItemSource[index.Row]);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_collectionView = null;
				_componentViewCreator = null;

				foreach (IComponentView view in _componentViews.Values.ToArray())
				{
					view.Dispose();
				}
				_componentViews.Clear();
				_componentViews = null;
			}

			base.Dispose(disposing);
		}
	}
}