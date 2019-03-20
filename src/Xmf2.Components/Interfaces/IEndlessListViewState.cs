namespace Xmf2.Components.Interfaces
{
	public interface IEndlessListViewState : IListViewState
	{
		bool HaveMoreItemsAvailable { get; }
		bool IsLoading { get; }
	}
}