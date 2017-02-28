using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xmf2.Commons.MvxExtends.Extensions
{
	public static class TaskExtensions
	{
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
