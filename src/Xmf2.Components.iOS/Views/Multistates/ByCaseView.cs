using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.ViewModels.Multistates;
using Xmf2.Core.iOS.Extensions;
using Xmf2.Core.Subscriptions;
using Xmf2.iOS.Extensions.Constraints;
using Xmf2.iOS.Extensions.Extensions;

namespace Xmf2.Components.iOS.Views.Multistates
{
	public class ByCaseView<TCaseEnum> : BaseComponentView<ByCaseViewState<TCaseEnum>>
	{
		private bool _currentCaseOnceSet = false;
		private TCaseEnum _currentCase;
		private Dictionary<TCaseEnum, ComponentInfo> _byCaseInfo;
		private ComponentInfo _currentInfo;
		private bool _aggressiveViewDispose = false;
		private UIView _container;

		public ByCaseView(IServiceLocator services, Dictionary<TCaseEnum, Func<IComponentView>> componentFactoryByCase) : base(services)
		{
			_currentCase = default(TCaseEnum);
			_byCaseInfo = componentFactoryByCase.ToDictionary(
				keySelector: kvp => kvp.Key,
				elementSelector: kvp => new ComponentInfo { ComponentFactory = kvp.Value }.DisposeWith(Disposables),
				comparer: componentFactoryByCase.Comparer
			);
			_container = this.CreateView().DisposeViewWith(Disposables);
		}

		protected override UIView RenderView() => _container;

		protected override void OnStateUpdate(ByCaseViewState<TCaseEnum> state)
		{
			base.OnStateUpdate(state);

			var newCase = state.Case;
			var newInfo = _byCaseInfo[state.Case];

			if (	!_currentCaseOnceSet
				||	!_byCaseInfo.Comparer.Equals(_currentCase, newCase))
			{
				if (_currentInfo != null)
				{
					if (_currentInfo.TryGetExistingConstraints(out var constraints))
					{
						_container.EnsureRemove(constraints);
					}
					_container.EnsureRemove(_container.Subviews);
				}
				if (_aggressiveViewDispose)
				{
					_currentInfo?.DisposeConstraints();
					_currentInfo?.DisposeComponent();
				}
				var newView = newInfo.GetComponent().View;
				newView.TranslatesAutoresizingMaskIntoConstraints = false;
				_container.AddSubview(newView);
				_container.AddConstraints(newInfo.GetOrCreateConstraints(parentView: _container));
				_currentInfo = newInfo;
				_currentCase = newCase;
				_currentCaseOnceSet = true;
			}
			newInfo.GetComponent().SetState(state.State);
		}

		public ByCaseView<TCaseEnum> WithAggressiveViewDispose(bool aggressive = true)
		{
			_aggressiveViewDispose = aggressive;
			return this;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_byCaseInfo = null;
			}
			base.Dispose(disposing);
		}

		#region Nested Types

		private class ComponentInfo : IDisposable
		{
			private bool _disposedValue = false;
			private IComponentView _component;
			private NSLayoutConstraint[] _viewConstraints;

			internal Func<IComponentView> ComponentFactory { private get; set; }
			internal IComponentView GetComponent() => _component ?? (_component = ComponentFactory());

			internal bool TryGetExistingConstraints(out NSLayoutConstraint[] constraints)
			{
				constraints = _viewConstraints;
				return (constraints != null);
			}
			internal NSLayoutConstraint[] GetOrCreateConstraints(UIView parentView)
			{
				if (_viewConstraints is null)
				{
					var view = GetComponent().View;
					_viewConstraints = new NSLayoutConstraint[]
					{
						NSLayoutConstraint.Create(parentView, NSLayoutAttribute.CenterX , NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f).WithAutomaticIdentifier(),
						NSLayoutConstraint.Create(parentView, NSLayoutAttribute.CenterY , NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f).WithAutomaticIdentifier(),
						NSLayoutConstraint.Create(parentView, NSLayoutAttribute.Width   , NSLayoutRelation.Equal, view, NSLayoutAttribute.Width  , 1f, 0f).WithAutomaticIdentifier(),
						NSLayoutConstraint.Create(parentView, NSLayoutAttribute.Height  , NSLayoutRelation.Equal, view, NSLayoutAttribute.Height , 1f, 0f).WithAutomaticIdentifier(),
					};
				}
				return _viewConstraints;
			}

			internal void DisposeComponent()
			{
				_component?.Dispose();
				_component = null;
			}
			internal void DisposeConstraints()
			{
				if (_viewConstraints is null)
				{
					return;
				}
				for (int i = 0; i < _viewConstraints.Length; i++)
				{
					_viewConstraints[i].Dispose();
				}
				_viewConstraints = null;
			}

			public void Dispose() => Dispose(true);
			protected virtual void Dispose(bool disposing)
			{
				if (!_disposedValue)
				{
					if (disposing)
					{
						ComponentFactory = null;
						DisposeConstraints();
						DisposeComponent();
					}
					_disposedValue = true;
				}
			}
		}
		public Func<UIView> NewViewFactory { private get; set; }

		#endregion Nested Types
	}
}
