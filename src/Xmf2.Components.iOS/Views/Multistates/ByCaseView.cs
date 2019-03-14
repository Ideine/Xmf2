using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.ViewModels.Multistates;
using Xmf2.Core.iOS.Controls;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.iOS.Views.Multistates
{
	public class ByCaseView<TCaseEnum> : BaseComponentView<ByCaseViewState<TCaseEnum>>
	{
		private Dictionary<TCaseEnum, IComponentView> _componentByCase;
		private UIByCaseView<TCaseEnum> _uiByCaseView;

		public ByCaseView(IServiceLocator services, Dictionary<TCaseEnum, IComponentView> componentByCase) : base(services)
		{
			_componentByCase = componentByCase;

			var viewByCase = new Dictionary<TCaseEnum, UIView>(
				collection: componentByCase.Select(kvp => new KeyValuePair<TCaseEnum, UIView>(kvp.Key, kvp.Value.View)),
				comparer: componentByCase.Comparer
			);

			_uiByCaseView = new UIByCaseView<TCaseEnum>(viewByCase).DisposeViewWith(Disposables);
		}

		protected override UIView RenderView() => _uiByCaseView;

		protected override void OnStateUpdate(ByCaseViewState<TCaseEnum> state)
		{
			base.OnStateUpdate(state);
			_componentByCase[state.Case].SetState(state.State);
			_uiByCaseView.WithCase(state.Case);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_componentByCase = null;
				_uiByCaseView = null;
			}
			base.Dispose(disposing);
		}
	}
}
