using System;
using UIKit;
using Xmf2.Core.LinearLists;
using Xmf2.Core.Subscriptions;
using Xmf2.Components.iOS.Views;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.ChipCloud;
using Xmf2.Components.iOS.Interfaces;

namespace Xmf2.Components.iOS.Controls
{
	public class ChipCloudComponentView<TComponentView> : BaseComponentView<ListViewState> where TComponentView : IComponentView
	{
		protected ChipCloudView GroupView;

		private ChipCloudItemSource _source;

		private Func<IServiceLocator, IComponentView> _factory;

		protected virtual UIColor BackgroundColor { get; } = UIColor.White;

		protected virtual int ItemHorizontalMargin { get; }

		protected virtual int ItemVerticalMargin { get; }

		public ChipCloudComponentView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services)
		{
			_factory = factory;

			GroupView = new ChipCloudView
			{
				ItemHorizontalMargin = ItemHorizontalMargin,
				ItemVerticalMargin = ItemVerticalMargin
			}.DisposeViewWith(Disposables);

			_source = CreateSource(GroupView, _factory);

			GroupView.Source = _source;
		}

		protected virtual ChipCloudItemSource CreateSource(ChipCloudView groupView, Func<IServiceLocator, IComponentView> factory)
		{
			return new ChipCloudItemSource(groupView, s => factory(Services)).DisposeWith(Disposables);
		}

		protected override UIView RenderView()
		{
			return GroupView;
		}

		protected override void OnStateUpdate(ListViewState state)
		{
			base.OnStateUpdate(state);
			_source.ItemSource = state.Items;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				GroupView = null;
				_source = null;
				_factory = null;
			}
			base.Dispose(disposing);
		}
	}
}

