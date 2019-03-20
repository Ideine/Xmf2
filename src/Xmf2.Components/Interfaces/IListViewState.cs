using System.Collections.Generic;

namespace Xmf2.Components.Interfaces
{
	public interface IListViewState : IViewState
	{
		IReadOnlyList<IEntityViewState> Items { get; }
	}
}