using Splat;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
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

		public static IDisposable SubscribeAndDispose<T>(this IObservable<T> observable, Action<T> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(x =>
			{
				onNext(x);
				disposable.Dispose();
			}).DisposeWith(disposable);
			return disposable;
		}

		public static void SubscribeAndDispose<T1, T2>(this IObservable<Tuple<T1, T2>> observable, Action<T1, T2> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(tuple =>
			{
				onNext(tuple.Item1, tuple.Item2);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static void SubscribeAndDispose<T1, T2, T3>(this IObservable<Tuple<T1, T2, T3>> observable, Action<T1, T2, T3> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(tuple =>
			{
				onNext(tuple.Item1, tuple.Item2, tuple.Item3);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static void SubscribeAndDispose<T1, T2, T3, T4>(this IObservable<Tuple<T1, T2, T3, T4>> observable, Action<T1, T2, T3, T4> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(tuple =>
			{
				onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static void SubscribeAndDispose<T1, T2, T3, T4, T5>(this IObservable<Tuple<T1, T2, T3, T4, T5>> observable, Action<T1, T2, T3, T4, T5> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(tuple =>
			{
				onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static void SubscribeAndDispose<T, R>(this IObservable<T> observable, Func<T, R> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(x =>
			{
				onNext(x);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static void SubscribeAndDispose<T1, T2, R>(this IObservable<Tuple<T1, T2>> observable, Func<T1, T2, R> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(tuple =>
			{
				onNext(tuple.Item1, tuple.Item2);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static void SubscribeAndDispose<T1, T2, T3, R>(this IObservable<Tuple<T1, T2, T3>> observable, Func<T1, T2, T3, R> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(tuple =>
			{
				onNext(tuple.Item1, tuple.Item2, tuple.Item3);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static void SubscribeAndDispose<T1, T2, T3, T4, R>(this IObservable<Tuple<T1, T2, T3, T4>> observable, Func<T1, T2, T3, T4, R> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(tuple =>
			{
				onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static void SubscribeAndDispose<T1, T2, T3, T4, T5, R>(this IObservable<Tuple<T1, T2, T3, T4, T5>> observable, Func<T1, T2, T3, T4, T5, R> onNext)
		{
			CompositeDisposable disposable = new CompositeDisposable();
			observable.Subscribe(tuple =>
			{
				onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
				disposable.Dispose();
			}).DisposeWith(disposable);
		}

		public static IObservable<T> WithErrorHandling<T>(this IObservable<T> observable, CustomErrorHandler customHandler = null)
		{
			IErrorHandler errorHandler = Locator.Current.GetService<IErrorHandler>();

			return errorHandler. Execute(observable, customHandler)
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

		public static IObservable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(this IObservable<Tuple<T1, T2, T3, T4, T5, T6>> source, Func<T1, T2, T3, T4, T5, T6, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
		}

		public static IObservable<TSource> StartWithDefault<TSource>(this IObservable<TSource> source)
		{
			return source.StartWith(default(TSource));
		}

		#region AsArray

		public static IObservable<T[]> AsArray<T>(this IObservable<Tuple<T, T>> source)
		{
			return source.Select(tuple => new T[] {tuple.Item1, tuple.Item2});
		}

		public static IObservable<T[]> AsArray<T>(this IObservable<Tuple<T, T, T>> source)
		{
			return source.Select(tuple => new T[] {tuple.Item1, tuple.Item2, tuple.Item3});
		}

		public static IObservable<T[]> AsArray<T>(this IObservable<Tuple<T, T, T, T>> source)
		{
			return source.Select(tuple => new T[] {tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4});
		}

		public static IObservable<T[]> AsArray<T>(this IObservable<Tuple<T, T, T, T, T>> source)
		{
			return source.Select(tuple => new T[] {tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5});
		}

		public static IObservable<T[]> AsArray<T>(this IObservable<Tuple<T, T, T, T, T, T>> source)
		{
			return source.Select(tuple => new T[] {tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6});
		}

		public static IObservable<T[]> AsArray<T>(this IObservable<Tuple<T, T, T, T, T, T, T>> source)
		{
			return source.Select(tuple => new T[] {tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7});
		}

		#endregion AsArray

		#region Tuple Subscribe

		public static IDisposable Subscribe<T1, T2>(this IObservable<Tuple<T1, T2>> source, Action<T1, T2> onNext)
		{
			return source.Select(NullToTupleWithDefault).Subscribe(tuple => onNext(tuple.Item1, tuple.Item2));
		}

		public static IDisposable Subscribe<T1, T2, T3>(this IObservable<Tuple<T1, T2, T3>> source, Action<T1, T2, T3> onNext)
		{
			return source.Select(NullToTupleWithDefault).Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3));
		}

		public static IDisposable Subscribe<T1, T2, T3, T4>(this IObservable<Tuple<T1, T2, T3, T4>> source, Action<T1, T2, T3, T4> onNext)
		{
			return source.Select(NullToTupleWithDefault).Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
		}

		public static IDisposable Subscribe<T1, T2, T3, T4, T5>(this IObservable<Tuple<T1, T2, T3, T4, T5>> source, Action<T1, T2, T3, T4, T5> onNext)
		{
			return source.Select(NullToTupleWithDefault).Subscribe(tuple => onNext(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5));
		}

		private static Tuple<T1, T2> NullToTupleWithDefault<T1, T2>(Tuple<T1, T2> tuple) => tuple ?? new Tuple<T1, T2>(default(T1), default(T2));
		private static Tuple<T1, T2, T3> NullToTupleWithDefault<T1, T2, T3>(Tuple<T1, T2, T3> tuple) => tuple ?? new Tuple<T1, T2, T3>(default(T1), default(T2), default(T3));
		private static Tuple<T1, T2, T3, T4> NullToTupleWithDefault<T1, T2, T3, T4>(Tuple<T1, T2, T3, T4> tuple) => tuple ?? new Tuple<T1, T2, T3, T4>(default(T1), default(T2), default(T3), default(T4));
		private static Tuple<T1, T2, T3, T4, T5> NullToTupleWithDefault<T1, T2, T3, T4, T5>(Tuple<T1, T2, T3, T4, T5> tuple) => tuple ?? new Tuple<T1, T2, T3, T4, T5>(default(T1), default(T2), default(T3), default(T4), default(T5));

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

		public static IObservable<TResult> Select<T1, T2, T3, T4, T5, T6, TResult>(this IObservable<(T1, T2, T3, T4, T5, T6)> source, Func<T1, T2, T3, T4, T5, T6, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6));
		}

		public static IObservable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this IObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> source, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10));
		}
		public static IObservable<TResult> Select<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this IObservable<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> source, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> selector)
		{
			return source.Select(tuple => selector(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8, tuple.Item9, tuple.Item10, tuple.Item11));
		}

		#endregion

		public static IObservable<T> TriggeredAlsoOn<T, T1>(this IObservable<T> mainObservable, IObservable<T1> observable1)
		{
			return Observable.CombineLatest(mainObservable, observable1, (mainObserved, _) => mainObserved);
		}

		public static IObservable<T> TriggeredAlsoOn<T, T1, T2>(
			this IObservable<T> mainObservable,
			IObservable<T1> observable1,
			IObservable<T2> observable2)
		{
			return Observable.CombineLatest(mainObservable, observable1, observable2, (mainObserved, _1, _2) => mainObserved);
		}

		public static IObservable<T> TriggeredAlsoOn<T, T2, T3, T4>(
			this IObservable<T> mainObservable,
			IObservable<T2> observable1,
			IObservable<T3> observable2,
			IObservable<T4> observable3)
		{
			return Observable.CombineLatest(mainObservable, observable1, observable2, observable3, (mainObserved, _1, _2, _3) => mainObserved);
		}

		public static IObservable<T> XmfDistinctUntilChanged<T>(this IObservable<T> observable, IEqualityComparer<T> pEqualityComparer = null)
		{
			bool isFirst = true;
			var previous = default(T);
			var equalityComparer = pEqualityComparer ?? EqualityComparer<T>.Default;

			return observable.Where(next =>
			{
				if (isFirst)
				{
					isFirst = false;
					previous = next;
					return true;
				}
				else if (!equalityComparer.Equals(previous, next))
				{
					previous = next;
					return true;
				}
				else
				{
					return false;
				}
			});
		}
	}
}