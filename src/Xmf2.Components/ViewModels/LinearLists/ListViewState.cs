using Xmf2.Components.Interfaces;
using System.Collections.Generic;

namespace Xmf2.Core.LinearLists
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
