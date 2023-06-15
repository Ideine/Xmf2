using Xmf2.Components.Events;

namespace Xmf2.Components.ViewModels.PullToRefresh
{
	public class RefreshedEvent : IEvent
	{
		public bool Refresh { get; }

		public RefreshedEvent(bool refresh)
		{
			Refresh = refresh;
		}
	}
}