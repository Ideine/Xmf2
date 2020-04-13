using System.Collections.Generic;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels.LinearLists
{
	public class ListViewState : IListViewState
	{
		public IReadOnlyList<IEntityViewState> Items { get; }

		public ListViewState(IReadOnlyList<IEntityViewState> items)
		{
			Items = items;
		}
	}
}