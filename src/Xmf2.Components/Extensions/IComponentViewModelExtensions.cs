using System.Threading.Tasks;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Extensions
{
	public static class ComponentViewModelExtensions
	{
		public static async Task<TComponentViewModel> WaitInitialize<TComponentViewModel>(this TComponentViewModel viewModel)
			where TComponentViewModel : IComponentViewModel
		{
			viewModel.Lifecycle.Initialize();
			await viewModel.Lifecycle.WaitForInitialization();
			return viewModel;
		}
	}
}