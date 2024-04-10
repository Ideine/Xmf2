using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace => Extension class
namespace ReactiveUI
{
	public static class BindingExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IObservable<T> OnMainThread<T>(this IObservable<T> source)
		{
			return source.ObserveOn(RxApp.MainThreadScheduler);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IObservable<T> OnTaskThread<T>(this IObservable<T> source)
		{
			return source.ObserveOn(RxApp.TaskpoolScheduler);
		}
	}
}