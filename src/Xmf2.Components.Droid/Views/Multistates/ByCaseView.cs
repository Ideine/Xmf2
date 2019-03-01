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
		private Dictionary<TCaseEnum, IComponentView> _componentByCase;
		private Dictionary<IComponentView, View> _viewForComponent;

		private ViewGroup _container;
		private FrameLayout.LayoutParams _childLayoutParams;

		public ByCaseView(IServiceLocator services, Dictionary<TCaseEnum, IComponentView> componentByCase) : base(services)
		{
			_componentByCase = componentByCase;
			_viewForComponent = new Dictionary<IComponentView, View>();
			_childLayoutParams = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MatchParent, FrameLayout.LayoutParams.WrapContent).DisposeWith(Disposables);
		}

		protected override View RenderView()
		{
			_container = new FrameLayout(Context);
			return _container;
		}

		protected override void OnStateUpdate(ByCaseViewState<TCaseEnum> byCaseState)
		{
			base.OnStateUpdate(byCaseState);
			IComponentView componentView = _componentByCase[byCaseState.Case];

			if (_viewForComponent.TryGetValue(componentView, out var viewForComponent))
			{
				if (_container.ChildCount == 1 && _container.GetChildAt(0) != viewForComponent)
				{
					SetCurrentView(componentView);
				}
			}
			else
			{
				SetCurrentView(componentView);
			}
			componentView.SetState(byCaseState.State);
		}

		#region Render IComponentView

		private void SetCurrentView(IComponentView componentView)
		{
			_container.RemoveAllViews();
			var view = GetViewForComponent(componentView).DisposeViewWith(Disposables);
			_container.AddView(view, _childLayoutParams);
		}

		private View GetViewForComponent(IComponentView componentView)
		{
			if (_viewForComponent.TryGetValue(componentView, out var view))
			{
				return view;
			}
			else
			{
				View newView = componentView.View(_container);
				_viewForComponent.Add(componentView, newView);
				return newView;
			}
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_componentByCase = null;
				_viewForComponent = null;
				_container = null;
				_childLayoutParams = null;
			}
			base.Dispose(disposing);
		}
	}
}
