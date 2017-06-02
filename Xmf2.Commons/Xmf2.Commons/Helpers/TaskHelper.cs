using System.Threading.Tasks;

namespace Xmf2.Commons.Helpers
{
	public static class TaskHelper
	{
		public static readonly Task CompletedTask = Task.FromResult<object>(null);
	}
}
