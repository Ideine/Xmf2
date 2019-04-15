using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels
{
	public class EmptyViewState : IViewState
	{
		public static readonly EmptyViewState Singleton = new EmptyViewState();
		private EmptyViewState() { }
	}
}
