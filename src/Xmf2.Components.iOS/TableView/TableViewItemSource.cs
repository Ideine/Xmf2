using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using Xmf2.Components.Helpers;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.iOS.TableView
{
	public class TableViewItemSource<TCell> : UITableViewSource where TCell : TableViewItemCell
	{
		protected readonly NSString _cellIdentifier;

		private UITableView _listView;
		protected Xmf2Disposable Disposables { get; } = new();

		protected readonly Dictionary<Type, Func<IComponentView>> _componentViewCreators;
		protected readonly Dictionary<Type, Dictionary<TCell, IComponentView>> _componentViews = new();
		private readonly Dictionary<Guid, IComponentView> _componentViewsByState = new();
		private readonly Dictionary<IComponentView, Guid> _stateByComponentViews = new();

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
					_listView.ReloadData();
				}
				else
				{
					ReloadState();
				}
			}
		}

		public TableViewItemSource(UITableView listView, Dictionary<Type, Func<IComponentView>> componentViewCreators, NSString cellIdentifier)
		{
			_listView = listView;
			_componentViewCreators = componentViewCreators;
			_cellIdentifier = cellIdentifier;

			foreach (Type type in _componentViewCreators.Keys)
			{
				_componentViews.Add(type, new Dictionary<TCell, IComponentView>());
			}
		}

		public TableViewItemSource(UITableView listView, Func<IComponentView> componentViewCreator, NSString cellIdentifier)
		: this(listView, new Dictionary<Type, Func<IComponentView>> { [typeof(IEntityViewState)] = componentViewCreator }, cellIdentifier)
		{
		}

		public override nint NumberOfSections(UITableView tableView) => 1;

		public override nint RowsInSection(UITableView tableview, nint section) => ItemSource?.Count ?? 0;

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			TCell cell = (TCell)tableView.DequeueReusableCell(_cellIdentifier, indexPath);
			IEntityViewState childState = ItemSource[indexPath.Row];

			Type childType = childState.GetType();
			Type implementationType = _componentViewCreators.Keys.First(x => x.IsAssignableFrom(childType));

			Dictionary<TCell, IComponentView> childViews = _componentViews[implementationType];

			if (childViews.TryGetValue(cell, out IComponentView childComponent))
			{
				if (   _stateByComponentViews.TryGetValue(childComponent, out Guid linkedExistingStateId) && linkedExistingStateId != childState.Id
					&& _componentViewsByState.TryGetValue(linkedExistingStateId, out IComponentView linkedExistingChildView) && linkedExistingChildView == childComponent)
				{
					_componentViewsByState.Remove(linkedExistingStateId);
				}
			}
			else
			{
				childComponent = _componentViewCreators[implementationType]().DisposeWith(Disposables);
				childViews.Add(cell, childComponent);
			}

			ComponentAdditionalTreatment(tableView, indexPath, childComponent);

			_stateByComponentViews[childComponent] = childState.Id;
			_componentViewsByState[childState.Id] = childComponent;

			cell.SetContent(childComponent.View);
			childComponent.SetState(childState);

			return cell;
		}

		protected virtual void ComponentAdditionalTreatment(UITableView tableView, NSIndexPath indexPath, IComponentView childComponent) { }

		private void ReloadState()
		{
			for (int i = 0 ; i < ItemSource.Count ; i++)
			{
				var childState = ItemSource[i];

				if (_componentViewsByState.TryGetValue(childState.Id, out IComponentView childView))
				{
					childView.SetState(childState);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_listView = null;
				_componentViewCreators.Clear();
				Disposables.Dispose();
				_componentViews.Clear();
			}

			base.Dispose(disposing);
		}
	}
}