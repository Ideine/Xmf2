using System;
using System.Collections.Generic;

namespace Xmf2.Commons.Extensions
{
	public static class EnumerableExtensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> lst)
        {
            if (lst == null)
                return new T[0];
            else
                return lst;
        }

		public static IEnumerable<T> Append<T>(this IEnumerable<T> lst, T obj)
		{
			foreach (var o in lst)
				yield return o;
			yield return obj;
		}
    }
}
