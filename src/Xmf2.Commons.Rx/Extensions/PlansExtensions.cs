using System;
using System.Reactive.Joins;
using System.Reactive.Linq;

namespace Xmf2.Commons.Rx.Extensions
{
	public static class PlansExtensions
	{
		public static IObservable<TResult> ToObservable<TResult>(this Plan<TResult> source)
		{
			return Observable.When(source);
		}

		public static IObservable<TResult> ToObservable<TResult>(this Plan<IObservable<TResult>> source)
		{
			return Observable.When(source).Merge();
		}
	}
}