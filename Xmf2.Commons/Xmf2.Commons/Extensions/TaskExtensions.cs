using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Xmf2.Commons.Extensions
{
    public static class TaskExtensions
	{
		public static ConfiguredTaskAwaitable Forget(this Task task)
		{
			return task.ConfigureAwait(false);
		}

		public static ConfiguredTaskAwaitable<T> Forget<T>(this Task<T> task)
		{
			return task.ConfigureAwait(false);
		}
	}
}
