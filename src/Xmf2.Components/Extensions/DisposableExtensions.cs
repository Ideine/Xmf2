using System;
using System.Threading.Tasks;

namespace Xmf2.Components.Extensions
{
	public static class DisposableExtensions
	{
		public static void DispatchSafeDispose(this IDisposable disposable)
		{
			Task.Run(() => disposable.SafeDispose());
		}

		public static void SafeDispose(this IDisposable disposable)
		{
			try
			{
				Console.WriteLine($"disposing: {disposable.GetType().Name}");
				disposable.Dispose();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}