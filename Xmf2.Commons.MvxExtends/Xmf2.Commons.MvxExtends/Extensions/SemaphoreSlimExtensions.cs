using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xmf2.Commons.MvxExtends.Extensions
{
	public static class SemaphoreSlimExtensions
	{
		private class SemaphoreSlimLock : IDisposable
		{
			private readonly SemaphoreSlim _semaphore;

			public SemaphoreSlimLock(SemaphoreSlim semaphore)
			{
				_semaphore = semaphore;
			}

			public void Dispose()
			{
				_semaphore.Release();
			}
		}

		public static IDisposable Lock(this SemaphoreSlim semaphore)
		{
			semaphore.Wait();
			return new SemaphoreSlimLock(semaphore);
		}

		public static async Task<IDisposable> LockAsync(this SemaphoreSlim semaphore)
		{
			await semaphore.WaitAsync().DontStickOnThread();
			return new SemaphoreSlimLock(semaphore);
		}
	}
}
