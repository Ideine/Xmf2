using Xmf2.Components.Interfaces;
using Xmf2.Components.Events;

namespace Xmf2.Components.ViewModels.EndlessScrolls
{
	public interface IEndlessListViewState : IListViewState
	{
		//TODO: mco, nouvelle interface proposée :
		//	bool HaveMoreItemsAvailable { get; }
		//	IEvent NewLoadNextPageRequestedEvent();

		//from Idelink
		int IndexPage { get; }
		int TotalCount { get; }
		IEvent LoadNextPageEvent { get; }
	}

	public class EndlessListViewState : IViewState
	{
		//public int IndexPage { get; }

		//public int TotalCount { get; }

		//public IEvent LoadNextPageEvent { get; }

		//public EndlessListViewState(int page, int totalCount, IEvent loadNextPageEvent)
		//{
		//	IndexPage = page;
		//	TotalCount = totalCount;
		//	LoadNextPageEvent = loadNextPageEvent;
		//}
	}
}