using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels.Multistates
{
	public class ByCaseViewState<TCaseEnum> : IViewState
	{
		public TCaseEnum Case { get; }
		public IViewState State { get; }

		public ByCaseViewState(TCaseEnum pCase, IViewState state)
		{
			Case = pCase;
			State = state;
		}
	}
}
