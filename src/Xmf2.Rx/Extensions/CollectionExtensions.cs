using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ReactiveUI;

namespace Xmf2.Rx.Extensions
{
	public static class CollectionExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ReactiveList<TSource> ToReactiveList<TSource>(this IEnumerable<TSource> source)
		{
			return new ReactiveList<TSource>(source);
		}
	}
}