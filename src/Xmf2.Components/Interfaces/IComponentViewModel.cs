using System;

namespace Xmf2.Components.Interfaces
{
	public interface IComponentViewModel : IDisposable
	{
		IViewState ViewState();

		IServiceLocator Services { get; }

		ILifecycleManager Lifecycle { get; }
	}
}