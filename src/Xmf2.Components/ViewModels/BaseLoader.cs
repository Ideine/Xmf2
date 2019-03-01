using Xmf2.Components.Interfaces;
using Xmf2.Components.Services;

namespace Xmf2.Components.ViewModels
{
	public abstract class BaseLoader : BaseServiceContainer
	{
		protected BaseLoader(IServiceLocator services) : base(services) { }
	}
}