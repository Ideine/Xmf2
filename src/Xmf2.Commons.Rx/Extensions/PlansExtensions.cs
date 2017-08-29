using System.Reactive.Joins;

namespace System.Reactive.Linq
{
	public static class PlansExtensions
	{
		public static IObservable<TResult> ToObservable<TResult>(this Plan<TResult> source)
		{
			return Observable.When(source);
		}

		public static IObservable<TResult> ToObservable<TResult>(this Plan<IObservable<TResult>> source)
		{
			return Observable.When(source)
				             .Merge();
		}
	}
}
