using Xmf2.Components.Interfaces;
using System.Collections.Generic;

namespace Xmf2.Components.ViewModels.Multistates
{
	public class ByCaseViewModel<TCaseEnum> : BaseComponentViewModel
	{
		private Dictionary<TCaseEnum, IComponentViewModel> _componentByCase;

		public TCaseEnum Case { get; set; }

		public ByCaseViewModel(IServiceLocator services, Dictionary<TCaseEnum, IComponentViewModel> componentByCase) : base(services)
		{
			_componentByCase = componentByCase;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_componentByCase = null;
			}
			base.Dispose(disposing);
		}

		protected override IViewState NewState()
		{
			var caseViewState = _componentByCase[Case].ViewState();
			return new ByCaseViewState<TCaseEnum>(Case, caseViewState);
		}
	}
}
