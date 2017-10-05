using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Splat;
using Xmf2.Commons.Errors;

namespace System
{
	public static class ObservableExtensions
	{
		public static IDisposable SubscribeAsync<T>(this IObservable<T> observable, Func<T, Task> onNext)
		{
			return observable.SelectMany(async item =>
			{
				await onNext(item);
				return Unit.Default;
			}).Subscribe();
		}

		public static IDisposable SubscribeAsync<T>(this IObservable<T> observable, Func<Task> onNext)
		{
			return observable.SelectMany(async _ =>
			{
				await onNext();
				return Unit.Default;
			}).Subscribe();
		}

		public static void SubscribeAndDispose<T>(this IObservable<T> observable, Action<T> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();

			observable.Subscribe(x =>
			{
				onNext(x);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

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

		public static IObservable<T> ToObservableForBinding<T>(this IObservable<T> observable)
		{
			var connectable = observable.Replay(1);

			connectable.Connect();

			return connectable;
		}
	}
}