using UIKit;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.Views;

namespace Xmf2.Components.iOS.Views
{
	public abstract class BaseComponentView<TViewState> : BaseCoreComponentView<TViewState>, IComponentView where TViewState : class, IViewState
	{
		private UIView _view;

		public UIView View => _view ?? (_view = RenderView());

		protected BaseComponentView(IServiceLocator services) : base(services)
		{

		}

		/// <summary>
		/// Called only once time
		/// </summary>
		/// <returns>The view.</returns>
		protected abstract UIView RenderView();

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_view = null;
			}
			base.Dispose(disposing);
		}
	}
}