using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Splat;
using Xmf2.Commons.Errors;

namespace Xmf2.Commons.Rx.Extensions
{
	public static class AsyncObservableExtensions
	{
		public static Task<TResult> WaitForOneAsync<TResult>(this IObservable<TResult> source) => Task.Run(() => source.FirstOrDefaultAsync().Wait());

		public static IObservable<T> ToObservableForBinding<T>(this IObservable<T> observable)
		{
			IConnectableObservable<T> connectable = observable.Replay(1);
			connectable.Connect();
			return connectable;
		}

		//CODE FROM : https://github.com/dotnet/reactive/issues/395#issuecomment-378855644
		public static IObservable<T> ThrottleFirst<T>(this IObservable<T> source, TimeSpan timespan, IScheduler timeSource)
		{
			return new ThrottleFirstObservable<T>(source, timeSource, timespan);
		}

		public static IDisposable SubscribeAsync<T>(this IObservable<T> observable, Func<T, Task> onNext) => observable.SelectMany(async item =>
		{
			await onNext(item);
			return Unit.Default;
		}).Subscribe();

		public static IDisposable SubscribeAsync<T>(this IObservable<T> observable, Func<Task> onNext) => observable.SelectMany(async _ =>
		{
			await onNext();
			return Unit.Default;
		}).Subscribe();

		public static IObservable<T> WithErrorHandling<T>(this IObservable<T> observable, CustomErrorHandler customHandler = null)
		{
			IErrorHandler errorHandler = Locator.Current.GetService<IErrorHandler>();

			return errorHandler.Execute(observable, customHandler)
				.Catch<T, Exception>(ex => Observable.Return(default(T)));
		}

		public static IDisposable SubscribeWithErrorHandling<T>(this IObservable<T> observable, CustomErrorHandler customHandler = null)
		{
			IErrorHandler errorHandler = Locator.Current.GetService<IErrorHandler>();

			return errorHandler.Execute(observable, customHandler)
				.Catch<T, Exception>(ex => Observable.Return(default(T)))
				.Subscribe();
		}
	}
}