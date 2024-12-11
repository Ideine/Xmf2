using System;
using System.Reactive.Disposables;

namespace Xmf2.Commons.Rx.Extensions
{
	public static class CompositeDisposableExtensions
	{
		public static void Add(this CompositeDisposable compositeDisposable, params IDisposable[] disposables)
		{
			foreach (IDisposable disposable in disposables)
			{
				compositeDisposable.Add(disposable);
			}
		}
	}
}