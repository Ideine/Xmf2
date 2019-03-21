using System;
using Xmf2.Components.Events;

namespace Xmf2.Components.ViewModels.EndlessScrolls
{
	public class LoadMoreListItemEvent : IEvent
	{
		public Guid CorrelationId { get; }

		public int IndexPage { get; }

		public LoadMoreListItemEvent(int indexPage)
		{
			IndexPage = indexPage;
			CorrelationId = Guid.NewGuid();
		}
	}
}
