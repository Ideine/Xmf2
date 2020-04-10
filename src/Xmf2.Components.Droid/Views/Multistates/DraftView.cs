using Android.Views;
using Android.Widget;
using Xmf2.Components.Interfaces;
using Xmf2.Components.ViewModels;

namespace Xmf2.Components.Droid.Views.Multistates
{
	public class DraftView : BaseComponentView<DraftViewState>
	{
		public DraftView(IServiceLocator services) : base(services) { }

		protected override View RenderView() => new FrameLayout(Context);
	}
}
