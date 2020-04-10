using System;
using System.Linq;
using Android.Views;
using Android.Widget;
using Xmf2.Core.Subscriptions;
using Xmf2.Components.Interfaces;
using System.Collections.Generic;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.ViewModels.Multistates;

namespace Xmf2.Components.Droid.Views.Multistates
{
	public class ByCaseView<TCaseEnum> : BaseComponentView<ByCaseViewState<TCaseEnum>>
	{
		private ViewGroup _container;
		private FrameLayout.LayoutParams _childLayoutParams;

		private bool _currentCaseOnceSet;
		private TCaseEnum _currentCase;
		private ComponentInfo _currentInfo;
		private Dictionary<TCaseEnum, ComponentInfo> _byCaseInfo;

		public ByCaseView(IServiceLocator services, Dictionary<TCaseEnum, Func<IComponentView>> componentFactoryByCase) : base(services)
		{
			_currentCase = default(TCaseEnum);

			_byCaseInfo = componentFactoryByCase.ToDictionary(
				keySelector: kvp => kvp.Key,
				elementSelector: kvp => new ComponentInfo { ComponentFactory = kvp.Value }.DisposeWith(Disposables),
				comparer: componentFactoryByCase.Comparer
			);

			_childLayoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent).DisposeWith(Disposables);
		}

		protected override View RenderView()
		{
			_container = new FrameLayout(Context);
			return _container;
		}

		protected override void OnStateUpdate(ByCaseViewState<TCaseEnum> state)
		{
			base.OnStateUpdate(state);

			var newCase = state.Case;
			var newInfo = _byCaseInfo[state.Case];

			if (!_currentCaseOnceSet || !_byCaseInfo.Comparer.Equals(_currentCase, newCase))
			{
				_currentInfo?.DisposeComponent();
				SetCurrentView(newInfo);

				_currentInfo = newInfo;
				_currentCase = newCase;
				_currentCaseOnceSet = true;
			}
			newInfo.GetComponent().SetState(state.State);
		}

		#region Render IComponentView

		private void SetCurrentView(ComponentInfo componentInfo)
		{
			_container.RemoveAllViews();
			var view = componentInfo.GetViewForComponent(_container).DisposeViewWith(Disposables);
			_container.AddView(view, _childLayoutParams);
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_byCaseInfo = null;
				_container = null;
				_childLayoutParams = null;
			}
			base.Dispose(disposing);
		}

		#region nested

		public class ComponentInfo : IDisposable
		{
			private bool _disposedValue = false;
			private IComponentView _component;
			private View _view;

			internal Func<IComponentView> ComponentFactory { private get; set; }

			internal IComponentView GetComponent() => _component ?? (_component = ComponentFactory());

			internal View GetViewForComponent(ViewGroup parent)
			{
				if (_view == null)
				{
					View newView = GetComponent().View(parent);
					_view = newView;
				}
				return _view;
			}

			#region Dispose

			internal void DisposeComponent()
			{
				_component?.Dispose();
				_view?.Dispose();
				_view = null;
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

			#endregion
		}

		#endregion
	}
}
