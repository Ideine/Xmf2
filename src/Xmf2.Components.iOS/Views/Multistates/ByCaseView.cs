using System;
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
		private Dictionary<TCaseEnum, ComponentInfo> _byCaseInfo;
		private UIByCaseView<TCaseEnum> _uiByCaseView;
		private ComponentInfo _currentInfo;
		private bool _aggressiveViewDispose;

		public ByCaseView(IServiceLocator services, Dictionary<TCaseEnum, Func<IComponentView>> componentFactoryByCase) : base(services)
		{
			_byCaseInfo = componentFactoryByCase.ToDictionary(
				keySelector: kvp => kvp.Key,
				elementSelector: kvp => new ComponentInfo { ComponentFactory = kvp.Value }.DisposeWith(Disposables),
				comparer: componentFactoryByCase.Comparer
			);

			Dictionary<TCaseEnum, Func<UIView>> viewFactoryByCase = _byCaseInfo.ToDictionary(
				keySelector: kvp => kvp.Key,
				elementSelector: kvp => kvp.Value.GetViewFactory(),
				comparer: componentFactoryByCase.Comparer
			);
			_uiByCaseView = new UIByCaseView<TCaseEnum>(viewFactoryByCase).DisposeViewWith(Disposables);
		}

		protected override UIView RenderView() => _uiByCaseView;

		protected override void OnStateUpdate(ByCaseViewState<TCaseEnum> state)
		{
			base.OnStateUpdate(state);

			ComponentInfo newInfo = _byCaseInfo[state.Case];
			_byCaseInfo[state.Case].GetComponent().SetState(state.State);
			_uiByCaseView.WithCase(state.Case);

			if (   newInfo != _currentInfo
				&& _aggressiveViewDispose)
			{
				_currentInfo?.DisposeComponent();
			}
			_currentInfo = newInfo;
		}

		public ByCaseView<TCaseEnum> WithAggressiveViewDispose(bool aggressive = true)
		{
			_uiByCaseView.WithAggressiveViewDispose(aggressive);
			_aggressiveViewDispose = aggressive;
			return this;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_byCaseInfo = null;
				_uiByCaseView = null;
			}
			base.Dispose(disposing);
		}

		#region Nested Types

		private class ComponentInfo : IDisposable
		{
			private bool _disposedValue = false;
			private IComponentView _component;

			public Func<IComponentView> ComponentFactory { private get; set; }

			public IComponentView GetComponent() => _component ?? (_component = ComponentFactory());
			public Func<UIView> GetViewFactory() => () => GetComponent().View;

			internal void DisposeComponent()
			{
				_component?.Dispose();
				_component = null;
			}

			public void Dispose() => Dispose(true);
			protected virtual void Dispose(bool disposing)
			{
				if (!_disposedValue)
				{
					if (disposing)
					{
						ComponentFactory = null;
						DisposeComponent();
					}
					_disposedValue = true;
				}
			}
		}

		#endregion Nested Types
	}
}
