using System.Collections.Generic;

namespace Xmf2.Commons.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> lst)
		{
			return lst == null ? new T[0] : lst;
		}

	}
}
