using System.Collections.Generic;
using ReactiveUI;

namespace Xmf2.Commons.Rx.Extensions
{
	public static class ListExtensions
	{
		public static ReactiveList<T> ToReactiveList<T>(this IEnumerable<T> src) => new ReactiveList<T>(src);
	}
}