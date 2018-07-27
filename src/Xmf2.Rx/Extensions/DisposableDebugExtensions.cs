using System.Threading;

namespace System.Reactive.Disposables
{
	public static class DisposableDebugExtensions
	{
		/// <summary>
		/// Write logs when disposer calls dispose.
		/// </summary>
		[Obsolete("Use for debugging purpose only.")]
		public static T DisposeInterceptedWith<T>(this T disposable, CompositeDisposable disposer) where T : IDisposable
		{
#if DEBUG
			new DisposableWrapper<T>(forwardTo: disposable, onDispose: onDispose).DisposeWith(disposer);
			return disposable;
#else
			return disposable.DisposeWith(disposer);
#endif
		}

		private class DisposableWrapper<T> : IDisposable
			where T : IDisposable
		{
			private static int _instanceIndex = -1;

			private int _instance;
			private int _calls = -1;
			private T _wrappedDisposable;

			public DisposableWrapper(T forwardTo)
			{
				_instance = Interlocked.Increment(ref _instanceIndex);

				System.Diagnostics.Debug.WriteLine($"DisposeInterceptedWith<{typeof(T).Name}>() subscription n° {_instance}");

				_wrappedDisposable = forwardTo;
			}

			public void Dispose()
			{
				Interlocked.Increment(ref _calls);
				System.Diagnostics.Debug.WriteLine($"DisposeInterceptedWith<{typeof(T).Name}>() n° {_instance} call {_calls}");
				_wrappedDisposable.Dispose();
			}
		}
	}

}
