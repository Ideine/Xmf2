namespace System.Reactive.Disposables
{
	public static class CompositeDisposableExtensions
	{
		public static void Add(this CompositeDisposable compositeDisposable, params IDisposable[] disposables)
		{
			foreach (var disposable in disposables)
			{
				compositeDisposable.Add(disposable);
			}
		}
	}
}
