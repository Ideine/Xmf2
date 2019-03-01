using System.Threading.Tasks;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Helpers;

namespace Xmf2.Components.ViewModels.Operations
{
	internal class DefaultViewModelOperation : ViewModelOperation<Unit>
	{
		public DefaultViewModelOperation(IBusy busy) : base(busy) { }

		protected override Task<Unit> Execute()
		{
			return Task.FromResult(Unit.Default);
		}
	}
}