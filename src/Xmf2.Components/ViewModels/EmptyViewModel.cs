using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels
{
	public class EmptyViewModel : BaseComponentViewModel
	{
		public EmptyViewModel(IServiceLocator services) : base(services)
		{
		}

		protected override IViewState NewState() => EmptyViewState.Singleton;
	}
}
