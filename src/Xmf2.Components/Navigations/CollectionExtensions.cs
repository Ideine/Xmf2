using System;
using System.Collections.Generic;

namespace Xmf2.Components.Navigations
{
	public static class CollectionExtensions
	{
		public static List<T> Sublist<T>(this IList<T> source, int start, int count)
		{
			List<T> result = new List<T>(count);
			int end = start + count;
			for (int i = start; i < end; ++i)
			{
				result.Add(source[i]);
			}

			return result;
		}

		public static List<TResult> ConvertAll<TSource, TResult>(this IReadOnlyList<TSource> source, Func<TSource, TResult> mapper)
		{
			List<TResult> result = new List<TResult>(source.Count);
			for (var i = 0; i < source.Count; i++)
			{
				result.Add(mapper(source[i]));
			}

			return result;
		}
	}
}