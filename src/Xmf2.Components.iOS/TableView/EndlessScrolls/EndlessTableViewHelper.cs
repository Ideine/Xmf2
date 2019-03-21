using System;
using UIKit;
using Xmf2.Components.Events;
using Xmf2.Components.Interfaces;
using Xmf2.Components.ViewModels.EndlessScrolls;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.iOS.EndlessScrolls.TableView
{
	public class EndlessTableViewHelper : UITableViewDelegate
	{
		private Xmf2Disposable _disposable = new Xmf2Disposable();
		private IEventBus _eventBus;
		private UITableView _listView;

		private nfloat _oldScrollY = 0;

		private int _totalCount;
		private bool _isLoading;
		private LoadMoreListItemEvent _loadNextPageEvent;

		public int VisibleThreshold { get; set; } = 2;

		public EndlessTableViewHelper(UITableView listView, IEventBus eventBus)
		{
			_eventBus = eventBus;
			_listView = listView;

			new EventSubscriber<UITableView>
			(
				_listView,
				list => list.Delegate = this,
				list => list.Delegate = null
			).DisposeEventWith(_disposable);
		}

		public override void Scrolled(UIScrollView scrollView)
		{
			nfloat scrollY = scrollView.ContentOffset.Y;

			if (scrollY >= _oldScrollY)
			{
				OnScrolled(_listView, scrollView.ContentOffset.X, scrollY);
			}

			_oldScrollY = scrollY;
		}

		public void OnScrolled(UITableView listView, nfloat scrollX, nfloat scrollY)
		{
			int totalItemCountInDataSet = (int)listView.NumberOfRowsInSection(0);
			int lastVisibleItemPosition = FindLastVisibleItemPosition(listView);
			int visibleItemCount = listView.VisibleCells?.Length ?? 0;

			if (!_isLoading &&
				totalItemCountInDataSet < _totalCount &&
				(totalItemCountInDataSet - visibleItemCount) <= (lastVisibleItemPosition + VisibleThreshold) &&
				_loadNextPageEvent != null &&
				_eventCorrelationId != _loadNextPageEvent.CorrelationId
			)
			{
				_isLoading = true;
				_eventCorrelationId = _loadNextPageEvent.CorrelationId;
				_eventBus.Publish(_loadNextPageEvent);
			}
		}

		private int FindLastVisibleItemPosition(UITableView listView)
		{
			var visibleCellIndexes = listView.IndexPathsForVisibleRows;
			if (visibleCellIndexes != null && visibleCellIndexes.Length > 0)
			{
				return visibleCellIndexes[visibleCellIndexes.Length - 1].Row;
			}

			return 0;
		}

		private int FindFirstVisibleItemPosition(UITableView listView)
		{
			return listView.IndexPathsForVisibleRows?[0].Row ?? 0;
		}

		private int _currentPageIndex;
		private Guid? _eventCorrelationId;

		public void SetState(IViewState state)
		{
			if (state is IEndlessListViewState endlessListViewState)
			{
				_loadNextPageEvent = endlessListViewState.LoadNextPageEvent as LoadMoreListItemEvent;
				if (_isLoading && (_totalCount != endlessListViewState.TotalCount || _currentPageIndex != endlessListViewState.IndexPage))
				{
					_isLoading = false;
				}

				_totalCount = endlessListViewState.TotalCount;
				_currentPageIndex = endlessListViewState.IndexPage;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable?.Dispose();
				_disposable = null;
				_eventBus = null;
				_listView = null;
			}

			base.Dispose(disposing);
		}
	}
}
