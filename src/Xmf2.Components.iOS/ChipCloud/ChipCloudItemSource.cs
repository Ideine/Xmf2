using System;
using System.Linq;
using CoreGraphics;
using Xmf2.Components.Interfaces;
using System.Collections.Generic;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.iOS.ChipCloud.Cells;
using Xmf2.Core.Subscriptions;
using Xmf2.Components.Helpers;

namespace Xmf2.Components.iOS.ChipCloud
{
	public class ChipCloudItemSource : Xmf2Disposable, IChipCloudItemSource
	{
		private ChipCloudView _collectionView;

		private Dictionary<Guid, IComponentView> _componentViews = new Dictionary<Guid, IComponentView>();
		private Func<string, IComponentView> _componentViewCreator;

		private IReadOnlyList<IEntityViewState> _itemSource;

		public IReadOnlyList<IEntityViewState> ItemSource
		{
			get => _itemSource;
			set
			{
				bool changed = ListHelper.HasChange(_itemSource, value);

				_itemSource = value;

				if (changed)
				{
					_collectionView.ReloadData();
				}
				else
				{
					ReApplyState();
				}
			}
		}

		public int Count => ItemSource?.Count ?? 0;

		public ChipCloudItemSource(ChipCloudView collectionView, Func<string, IComponentView> componentViewCreator)
		{
			_collectionView = collectionView;
			_componentViewCreator = componentViewCreator;
		}

		public virtual ChipCloudItemCell GetCell(ChipCloudView cloudView, int position)
		{
			ChipCloudItemCell cell = new ChipCloudItemCell();

			IEntityViewState childState = ItemSource[position];
			Guid childId = childState.Id;
			IComponentView childView = null;
			if (!_componentViews.TryGetValue(childId, out childView))
			{
				childView = _componentViewCreator(childState.Id.ToString());
				_componentViews.Add(childId, childView);
			}

			cell.SetContent(childView.View);
			childView.SetState(childState);

			cell.ItemSize = ViewSize(childView);

			return cell;
		}

		private void ReApplyState()
		{
			for (int position = 0; position < Count; position++)
			{
				IEntityViewState childState = ItemSource[position];
				IComponentView childView = null;
				_componentViews.TryGetValue(childState.Id, out childView);
				childView?.SetState(childState);
			}
		}

		protected virtual CGSize ViewSize(IComponentView componentView) => CGSize.Empty;

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
