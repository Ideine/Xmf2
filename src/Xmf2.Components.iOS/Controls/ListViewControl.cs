using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Core.iOS.Controls;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.iOS.Controls
{
	public class ListViewControl : UIView
	{
		private Func<IComponentView> _componentViewCreator;
		private readonly Xmf2Disposable _disposables = new Xmf2Disposable();

		public UIScrollView ScrollView { get; private set; }
		public UILinearLayout LinearLayout { get; private set; }
		public List<IComponentView> ChildComponents { get; private set; }

		public ListViewControl(Func<IComponentView> componentViewCreator, bool enableScrollView, UILinearLayout.LayoutOrientation orientation = UILinearLayout.LayoutOrientation.Vertical, Func<UIView> separatorCreator = null)
		{
			_componentViewCreator = componentViewCreator;

			LinearLayout = new UILinearLayout(orientation, separatorCreator).DisposeViewWith(_disposables);
			ChildComponents = new List<IComponentView>();

			if (enableScrollView)
			{
				if (orientation == UILinearLayout.LayoutOrientation.Vertical)
				{
					ScrollView = this.CreateVerticalScroll().DisposeViewWith(_disposables);

					Add(ScrollView);
					ScrollView.Add(LinearLayout);

					this.CenterAndFillWidth(ScrollView)
						.CenterAndFillHeight(ScrollView);

					ScrollView.VerticalScrollContentConstraint(LinearLayout);
				}
				else
				{
					ScrollView = this.CreateHorizontalScroll().DisposeViewWith(_disposables);

					Add(ScrollView);
					ScrollView.Add(LinearLayout);

					this.CenterAndFillWidth(ScrollView)
						.CenterAndFillHeight(ScrollView);

					ScrollView.HorizontalScrollContentConstraint(LinearLayout);
				}
			}
			else
			{
				Add(LinearLayout);
				this.CenterAndFillWidth(LinearLayout)
					.CenterAndFillHeight(LinearLayout);
			}
		}

		public void ReapplyStates(IReadOnlyList<IEntityViewState> items)
		{
			if (ChildComponents.Count != items.Count)
			{
				throw new InvalidOperationException();
			}

			for (int i = 0; i < ChildComponents.Count; i++)
			{
				ChildComponents[i].SetState(items[i]);
			}
		}

		public void Reset(IReadOnlyList<IEntityViewState> entities)
		{
			ChildComponents.Clear();
			LinearLayout.Clear();

			if (entities.Count > 0)
			{
				ChildComponents.AddRange(entities.Select(childState =>
				{
					var childComponent = _componentViewCreator();
					childComponent.SetState(childState);
					return childComponent;
				}));
				LinearLayout.AddSubviews(ChildComponents.Select(c => c.View));
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposables.Dispose();

				LinearLayout = null;
				ChildComponents = null;

				_componentViewCreator = null;
			}

			base.Dispose(disposing);
		}
	}
}