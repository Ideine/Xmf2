using System;
using Android.Content;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Views;
using Xmf2.Core.Subscriptions;
using Xmf2.Components.Droid.Controls;

namespace Xmf2.Components.Droid.Views
{
	public abstract class BaseComponentView<TViewState> : BaseCoreComponentView<TViewState>, IComponentView where TViewState : class, IViewState
	{
		private View _view;
		private ViewGroup _parent;

		protected Context Context => _parent?.Context;

		private LayoutInflater _inflater;
		private LayoutInflater Inflater => _inflater ?? (_inflater = Services.Resolve<ILayoutInflaterResolver>().Inflater().DisposeLayoutHolderWith(Disposables));

		protected BaseComponentView(IServiceLocator services) : base(services) { }

		public View View(ViewGroup parent)
		{
			if (_view != null)
			{
				return _view;
			}

			_parent = parent;

			_view = RenderView();
			return _view;
		}

		protected abstract View RenderView();

		/// <summary>
		/// Don't need to DisposeView at the end, already done ! 
		/// </summary>
		protected View Inflate(int layoutId)
		{
			if (_parent == null)
			{
				Console.WriteLine($"Take care, you are inflating view in type {GetType()} without parent");
			}

			return Inflater.Inflate(layoutId, _parent, false).DisposeViewWith(Disposables);
		}

		protected View Mount(View root, int id, IComponentView component)
		{
			return root.FindViewById<ComponentStub>(id)
					   .DisposeViewWith(Disposables)
					   .SetComponent(component)
					   .DisposeViewWith(Disposables);
		}

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