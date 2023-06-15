using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels.PullToRefresh
{
	public class PullToRefreshViewModel : BaseComponentViewModel
	{
		public bool IsRefreshing { get; set; }

		public bool IsEnabled { get; set; } = true;

		public PullToRefreshViewModel(IServiceLocator services) : base(services) { }

		protected override IViewState NewState()
		{
			return new PullToRefreshViewState(
				isRefreshing: IsRefreshing,
				isEnabled: IsEnabled,
				refreshEvent: new RefreshedEvent(refresh: true)
			);
		}
	}
}