using System;
using Xmf2.NavigationGraph.Core.Interfaces;

namespace Xmf2.Components.Interfaces
{
	public interface IComponentViewModel : IViewModel
	{
		IViewState ViewState();

		IServiceLocator Services { get; }

		ILifecycleManager Lifecycle { get; }
	}
}