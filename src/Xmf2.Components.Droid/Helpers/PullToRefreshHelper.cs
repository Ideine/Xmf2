using System;
using AndroidX.SwipeRefreshLayout.Widget;
using Xmf2.Components.Events;
using Xmf2.Components.Interfaces;
using Xmf2.Components.ViewModels.PullToRefresh;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.Helpers
{
	public class PullToRefreshHelper : IDisposable
	{
		private Xmf2Disposable _disposable = new();

		private SwipeRefreshLayout _swipeRefreshLayout;
		private PullToRefreshViewState _currentState;
		private IEventBus _eventBus;

		public PullToRefreshHelper(SwipeRefreshLayout swipeRefreshLayout, IEventBus eventBus)
		{
			_swipeRefreshLayout = swipeRefreshLayout;
			_eventBus = eventBus;

			new EventSubscriber<SwipeRefreshLayout>(
				_swipeRefreshLayout,
				swipeLayout => swipeLayout.Refresh += OnRefresh,
				swipeLayout => swipeLayout.Refresh -= OnRefresh
			).DisposeEventWith(_disposable);
		}

		private void OnRefresh(object sender, EventArgs e)
		{
			if (_currentState == null)
			{
				return;
			}

			if (_swipeRefreshLayout.Refreshing != _currentState.IsRefreshing)
			{
				_eventBus.Publish(_currentState.Refreshed);
			}
		}

		public void SetState(IViewState state)
		{
			if (state is PullToRefreshViewState pullToRefreshViewState)
			{
				_currentState = pullToRefreshViewState;
				_swipeRefreshLayout.Refreshing = pullToRefreshViewState.IsRefreshing;
				_swipeRefreshLayout.Enabled = pullToRefreshViewState.IsEnabled;
			}
		}

		public void Dispose()
		{
			_swipeRefreshLayout = null;
			_currentState = null;
			_eventBus = null;

			_disposable?.Dispose();
			_disposable = null;
		}
	}
}