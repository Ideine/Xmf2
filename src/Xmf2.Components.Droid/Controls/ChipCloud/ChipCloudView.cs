using Android.Graphics;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Droid.Views;
using Xmf2.Components.Interfaces;
using Xmf2.Core.LinearLists;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.Controls.ChipCloud
{
	public abstract class ChipCloudView<TCellComponent> : BaseComponentView<IListViewState> where TCellComponent : IComponentView
	{
		protected ChipCloud ChipCloud;
		protected ChipCloudAdapter Adapter;
		protected virtual Color BackgroundColor => Color.White;

		protected ChipCloudView(IServiceLocator services) : base(services)
		{
			Adapter = new ChipCloudAdapter(Context, Factory).DisposeWith(Disposables);
		}

		protected abstract IComponentView Factory(string itemId);

		protected override View RenderView()
		{
			ChipCloud = new ChipCloud(Context).DisposeViewWith(Disposables);
			ChipCloud.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent).DisposeWith(Disposables);
			ChipCloud.SetBackgroundColor(BackgroundColor);
			ChipCloud.Adapter = Adapter;
			return ChipCloud;
		}

		protected override void OnStateUpdate(IListViewState state)
		{
			base.OnStateUpdate(state);
			Adapter.ItemSource = state.Items;
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				ChipCloud = null;
				Adapter = null;
			}
			base.Dispose(disposing);
		}
	}
}
