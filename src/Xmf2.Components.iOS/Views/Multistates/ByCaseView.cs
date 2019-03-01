using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.ViewModels.Multistates;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.iOS.Views.Multistates
{
	public class ByCaseView<TCaseEnum> : BaseComponentView<ByCaseViewState<TCaseEnum>>
	{
		private Dictionary<TCaseEnum, IComponentView> _componentByCase;
		private Dictionary<UIView, NSLayoutConstraint[]> _constraintsByView;
		private UIView _container;

		public ByCaseView(IServiceLocator services, Dictionary<TCaseEnum, IComponentView> componentByCase) : base(services)
		{
			_componentByCase = componentByCase;
			_container = this.CreateView().DisposeViewWith(Disposables);
			_constraintsByView = new Dictionary<UIView, NSLayoutConstraint[]>();
		}

		protected override UIView RenderView() => _container;

		private NSLayoutConstraint[] GetContraintsFor(UIView view)
		{
			if (_constraintsByView.TryGetValue(view, out var constraints))
			{
				return constraints;
			}
			else
			{
				var newConstraints = new NSLayoutConstraint[]
				{
					NSLayoutConstraint.Create(_container, NSLayoutAttribute.CenterX , NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterX, 1f, 0f).WithAutomaticIdentifier().DisposeWith(Disposables),
					NSLayoutConstraint.Create(_container, NSLayoutAttribute.CenterY , NSLayoutRelation.Equal, view, NSLayoutAttribute.CenterY, 1f, 0f).WithAutomaticIdentifier().DisposeWith(Disposables),
					NSLayoutConstraint.Create(_container, NSLayoutAttribute.Width   , NSLayoutRelation.Equal, view, NSLayoutAttribute.Width  , 1f, 0f).WithAutomaticIdentifier().DisposeWith(Disposables),
					NSLayoutConstraint.Create(_container, NSLayoutAttribute.Height  , NSLayoutRelation.Equal, view, NSLayoutAttribute.Height , 1f, 0f).WithAutomaticIdentifier().DisposeWith(Disposables),
				};
				_constraintsByView.Add(view, newConstraints);
				return newConstraints;
			}
		}

		protected override void OnStateUpdate(ByCaseViewState<TCaseEnum> byCaseState)
		{
			base.OnStateUpdate(byCaseState);
			_componentByCase[byCaseState.Case].SetState(byCaseState.State);
			var view = _componentByCase[byCaseState.Case].View;
			if (_container.Subviews.FirstOrDefault() != view)
			{
				_container.EnsureRemove(_constraintsByView.SelectMany(x => x.Value));
				_container.EnsureRemove(_container.Subviews);
				view.TranslatesAutoresizingMaskIntoConstraints = false;
				_container.AddSubview(view);
				_container.AddConstraints(GetContraintsFor(view));
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_componentByCase = null;
				_constraintsByView = null;
				_container = null;
			}
			base.Dispose(disposing);
		}
	}
}
