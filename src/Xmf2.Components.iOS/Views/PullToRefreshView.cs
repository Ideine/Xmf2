using System;
using UIKit;
using Xmf2.Components.Interfaces;
using Xmf2.Components.ViewModels.PullToRefresh;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.iOS.Views
{
	public class PullToRefreshView : BaseComponentView<PullToRefreshViewState>
	{
		private UIRefreshControl _refreshControl;

		public PullToRefreshView(IServiceLocator services) : base(services)
		{
			_refreshControl = new UIRefreshControl().DisposeViewWith(Disposables);

			new EventSubscriber<UIRefreshControl>
			(
				_refreshControl,
				refreshControl => refreshControl.ValueChanged += OnRefreshValueChanged,
				refreshControl => refreshControl.ValueChanged -= OnRefreshValueChanged
			).DisposeBindingWith(Disposables);
		}

		protected override UIView RenderView()
		{
			return _refreshControl;
		}

		private void OnRefreshValueChanged(object sender, EventArgs e)
		{
			if (CurrentState == null)
			{
				return;
			}

			if (_refreshControl.Refreshing != CurrentState.IsRefreshing)
			{
				EventBus.Publish(CurrentState.Refreshed);
			}
		}

		protected override void OnStateUpdate(PullToRefreshViewState state)
		{
			base.OnStateUpdate(state);
			_refreshControl.Enabled = state.IsEnabled;
			//TODO VJU ?: revoir pour faire du beginrefreshing au premier chargement
			if (!state.IsRefreshing)
			{
				_refreshControl.EndRefreshing();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_refreshControl = null;
			}

			base.Dispose(disposing);
		}
	}
}