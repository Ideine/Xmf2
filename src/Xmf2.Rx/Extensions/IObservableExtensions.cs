using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Splat;
using Xmf2.Commons.Errors;

namespace System
{
	public static class IObservableExtensions
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

		public static IDisposable SubscribeWithErrorHandling<T>(this IObservable<T> observable, Action<T> onNext, CustomErrorHandler customHandler = null)
		{
			IErrorHandler errorHandler = Locator.Current.GetService<IErrorHandler>();

			return errorHandler.Execute(observable, customHandler)
				.Catch<T, Exception>(ex => Observable.Return(default(T)))
				.Subscribe(onNext);

		}

		public static IObservable<T> ToObservableForBinding<T>(this IObservable<T> observable)
		{
			var connectable = observable.Replay(1);

			connectable.Connect();

			return connectable;
		}

		public static IObservable<TResult> Select<T1, T2, TResult>(this IObservable<Tuple<T1, T2>> source, Func<T1, T2, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2));
		}
		public static IObservable<TResult> Select<T1, T2, T3, TResult>(this IObservable<Tuple<T1, T2, T3>> source, Func<T1, T2, T3, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3));
		}
		public static IObservable<TResult> Select<T1, T2, T3, T4, TResult>(this IObservable<Tuple<T1, T2, T3, T4>> source, Func<T1, T2, T3, T4, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
		}
		public static IObservable<TResult> Select<T1, T2, T3, T4, T5, TResult>(this IObservable<Tuple<T1, T2, T3, T4, T5>> source, Func<T1, T2, T3, T4, T5, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
		}

		public static IObservable<TSource> StartWithDefault<TSource>(this IObservable<TSource> source)
		{
			return source.StartWith(default(TSource));
		}

		#region Tuple Subscribe
		public static IDisposable Subscribe<T1, T2>(this IObservable<Tuple<T1, T2>> source, Action<T1, T2> onNext)
		{
			return source.Subscribe(tuple => onNext(tuple.Item1, tuple.Item2));
		}
		public static IDisposable Subscribe<T1, T2, T3>(this IObservable<Tuple<T1, T2, T3>> source, Action<T1, T2, T3> onNext)
		{
			return source.Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3));
		}
		public static IDisposable Subscribe<T1, T2, T3, T4>(this IObservable<Tuple<T1, T2, T3, T4>> source, Action<T1, T2, T3, T4> onNext)
		{
			return source.Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
		}
		public static IDisposable Subscribe<T1, T2, T3, T4, T5>(this IObservable<Tuple<T1, T2, T3, T4, T5>> source, Action<T1, T2, T3, T4, T5> onNext)
		{
			return source.Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
		}
		#endregion

		#region ValueTuple Subscribe
		public static IDisposable Subscribe<T1, T2>(this IObservable<(T1, T2)> source, Action<T1, T2> onNext)
		{
			return source.Subscribe(tuple => onNext(tuple.Item1, tuple.Item2));
		}
		public static IDisposable Subscribe<T1, T2, T3>(this IObservable<(T1, T2, T3)> source, Action<T1, T2, T3> onNext)
		{
			return source.Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3));
		}
		public static IDisposable Subscribe<T1, T2, T3, T4>(this IObservable<(T1, T2, T3, T4)> source, Action<T1, T2, T3, T4> onNext)
		{
			return source.Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
		}
		public static IDisposable Subscribe<T1, T2, T3, T4, T5>(this IObservable<(T1, T2, T3, T4, T5)> source, Action<T1, T2, T3, T4, T5> onNext)
		{
			return source.Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
		}
		#endregion

		#region ValueTuple Select
		public static IObservable<TResult> Select<T1, T2, TResult>(this IObservable<(T1, T2)> source, Func<T1, T2, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2));
		}
		public static IObservable<TResult> Select<T1, T2, T3, TResult>(this IObservable<(T1, T2, T3)> source, Func<T1, T2, T3, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3));
		}
		public static IObservable<TResult> Select<T1, T2, T3, T4, TResult>(this IObservable<(T1, T2, T3, T4)> source, Func<T1, T2, T3, T4, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
		}
		public static IObservable<TResult> Select<T1, T2, T3, T4, T5, TResult>(this IObservable<(T1, T2, T3, T4, T5)> source, Func<T1, T2, T3, T4, T5, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
		}
		#endregion
	}
}