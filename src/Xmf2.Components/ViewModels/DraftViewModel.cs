using System;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels
{
	[Obsolete("Only use while in development")]
	public class DraftViewModel : BaseComponentViewModel
	{
		public DraftViewModel(IServiceLocator services) : base(services) { }
		protected override IViewState NewState() => new DraftViewState();
	}
}
