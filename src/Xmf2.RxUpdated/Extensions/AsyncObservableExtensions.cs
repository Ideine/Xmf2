using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace System
{
	public static class AsyncObservableExtensions
	{
		public static Task<TResult> WaitForOneAsync<TResult>(this IObservable<TResult> source) => Task.Run(() => source.FirstOrDefaultAsync().Wait());
	}
}
