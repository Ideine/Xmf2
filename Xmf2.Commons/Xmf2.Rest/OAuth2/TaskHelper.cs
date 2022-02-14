using System.Threading.Tasks;

namespace Xmf2.Rest.OAuth2
{
	public static class TaskHelper
	{
		public static Task CompletedTask { get; } = Task.FromResult<object>(null);
	}
}