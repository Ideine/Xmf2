using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Linq
{
	public static class IEnumerableExtensions
	{
		public static async Task<IEnumerable<T>> Where<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
		{
			var itemTaskList = source.Select(item => new
			{
				Item = item,
				KeepItem = predicate.Invoke(item)
			});
			await Task.WhenAll(itemTaskList.Select(x => x.KeepItem));
			return itemTaskList.Where(x => x.KeepItem.Result)
				.Select(x => x.Item);
		}
	}
}