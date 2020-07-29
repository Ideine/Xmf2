using System.Threading.Tasks;

namespace System.Reactive.Linq
{
	public static class AsyncObservableExtensions
	{
		public static Task<TResult> WaitForOneAsync<TResult>(this IObservable<TResult> source) => Task.Run(() => source.FirstOrDefaultAsync().Wait());
	}
}
