using Android.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Droid.List
{
	public interface IListView : IComponentView
	{
		bool TryAddHeader(IComponentView header, int? height = null, int? index = null);
		//TODO RAJOUTER INDEX SUR PARALLAX OU STICKY SI BESOIN
		bool TryAddParallaxHeader(IComponentView header, int? height = null);
		bool TryAddStickyHeader(IComponentView component, View componentView, out StickyRecyclerHelper helper, int offset = 0, bool autoActivate = true);
		bool TryRemoveHeader(IComponentView component);

		bool TryAddFooter(IComponentView footer);
		bool TryRemoveFooter(IComponentView footer);

		bool TryUpdateHeader(IComponentView component, IViewState viewState);
		bool TryUpdateFooter(IComponentView component, IViewState viewState);
	}
}
