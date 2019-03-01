using System;
using System.Collections.Generic;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Helpers
{
	public static class ListHelper
	{
		public static bool HasChange(IReadOnlyList<IEntityViewState> source, IReadOnlyList<IEntityViewState> newSource)
		{
			if (source == null ^ newSource == null)
			{
				return true;
			}

			if (source == null)
			{
				return false;
			}

			if (source.Count != newSource.Count)
			{
				return true;
			}

			for (int i = 0; i < source.Count; i++)
			{
				if (source[i].Id != newSource[i].Id)
				{
					return true;
				}
			}

			return false;
		}
	}
}
