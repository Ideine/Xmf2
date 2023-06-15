using Xmf2.Components.Events;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels.PullToRefresh
{
	public class PullToRefreshViewState : IViewState
	{
		public bool IsRefreshing { get; }
		public bool IsEnabled { get; }
		public IEvent Refreshed { get; }

		public PullToRefreshViewState(bool isRefreshing, bool isEnabled, IEvent refreshEvent)
		{
			IsRefreshing = isRefreshing;
			IsEnabled = isEnabled;
			Refreshed = refreshEvent;
		}
	}
}