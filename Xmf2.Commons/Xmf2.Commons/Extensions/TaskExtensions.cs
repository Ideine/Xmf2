using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xmf2.Commons.Extensions
{
	public static class TaskExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredTaskAwaitable Forget(this Task task)
		{
			return task.ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredTaskAwaitable<T> Forget<T>(this Task<T> task)
		{
			return task.ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredTaskAwaitable DontStickOnThread(this Task task)
		{
			return task.ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredTaskAwaitable<T> DontStickOnThread<T>(this Task<T> task)
		{
			return task.ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredTaskAwaitable StickOnThread(this Task task)
		{
			return task.ConfigureAwait(true);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ConfiguredTaskAwaitable<T> StickOnThread<T>(this Task<T> task)
		{
			return task.ConfigureAwait(true);
		}
	}
}
