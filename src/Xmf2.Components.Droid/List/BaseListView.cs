using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Droid.Views;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.List
{
	public class BaseListView<TViewState> : BaseComponentView<TViewState>, IListView where TViewState : class, IViewState
	{
		private Dictionary<IComponentView, RecyclerView.OnScrollListener> _listenerList = new();

		private Dictionary<IComponentView, EventSubscriber<RecyclerView>> _helperList = new();

		protected RecyclerView RecyclerView;

		protected virtual Color BackgroundColor => Color.White;

		protected CommonAdapter Adapter { get; set; }

		protected virtual int LayoutResId { get; } = Resource.Layout.ListView;

		protected BaseListView(IServiceLocator services) : base(services) { }

		protected override View RenderView()
		{
			var root = Inflate(LayoutResId);

			RecyclerView = root.FindViewById<RecyclerView>(Resource.Id.RecyclerView).DisposeViewWith(Disposables);
			((SimpleItemAnimator)RecyclerView.GetItemAnimator()).SupportsChangeAnimations = false;
			RecyclerView.SetBackgroundColor(BackgroundColor);

			RecyclerView.SetAdapter(Adapter);

			RecyclerView.SetLayoutManager(GetLayoutManager(RecyclerView.Context).DisposeWith(Disposables));

			foreach (var entry in _listenerList)
			{
				var subscriber = CreateSubscriber(RecyclerView, entry.Value);
				_helperList[entry.Key] = subscriber;
			}

			_listenerList.Clear();

			foreach (var helper in _helperList.Values)
			{
				helper.Subscribe();
			}

			return root;
		}

		protected virtual RecyclerView.LayoutManager GetLayoutManager(Context context)
		{
			return new LinearLayoutManager(context, LinearLayoutManager.Vertical, false);
		}

		#region IListView

		public bool TryUpdateHeader(IComponentView component, IViewState viewState)
		{
			return Adapter.TryUpdateHeader(component, viewState);
		}

		public bool TryUpdateFooter(IComponentView component, IViewState viewState)
		{
			return Adapter.TryUpdateFooter(component, viewState);
		}

		public bool TryAddHeader(IComponentView header, int? height = null, int? index = null)
		{
			return Adapter.TryAddHeader(header, height, index);
		}

		private EventSubscriber<RecyclerView> CreateSubscriber(RecyclerView view, RecyclerView.OnScrollListener helper)
		{
			return new EventSubscriber<RecyclerView>(
				obj: view,
				subscribe: recycler => recycler.AddOnScrollListener(helper),
				unsubscribe: recycler => recycler.RemoveOnScrollListener(helper),
				autoSubscribe: false
			).DisposeEventWith(Disposables);
		}

		public bool TryAddParallaxHeader(IComponentView header, int? height)
		{
			var parallaxHelper = new ParallaxRecyclerViewHelper().DisposeWith(Disposables);

			if (RecyclerView != null)
			{
				var subscriber = CreateSubscriber(RecyclerView, parallaxHelper);
				_helperList[header] = subscriber;
				subscriber.Subscribe();
			}
			else
			{
				_listenerList[header] = parallaxHelper;
			}

			return Adapter.TryAddParallaxHeader(header, parallaxHelper, height);
		}

		public bool TryAddStickyHeader(IComponentView component, View componentView, out StickyRecyclerHelper helper, int offset = 0, bool autoActivate = true)
		{
			var stickyView = new StickyView(componentView).DisposeWith(Disposables);
			var stickyHelper = new StickyRecyclerHelper(componentView, stickyView, offset, autoActivate).DisposeWith(Disposables);

			if (RecyclerView != null)
			{
				var subscriber = CreateSubscriber(RecyclerView, stickyHelper);
				_helperList[component] = subscriber;
				subscriber.Subscribe();
			}
			else
			{
				_listenerList[component] = stickyHelper;
			}

			helper = stickyHelper;
			return Adapter.TryAddStickyHeader(component, stickyView);
		}

		public bool TryRemoveHeader(IComponentView header)
		{
			var res = Adapter.TryRemoveHeader(header);

			if (_helperList.TryGetValue(header, out var subscriber))
			{
				_helperList.Remove(header);
				subscriber.Unsubscribe();
			}

			return res;
		}

		public bool TryAddFooter(IComponentView footer)
		{
			return Adapter.TryAddFooter(footer);
		}

		public bool TryRemoveFooter(IComponentView footer)
		{
			return Adapter.TryRemoveFooter(footer);
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_helperList = null;
				_listenerList = null;
				RecyclerView = null;
				Adapter = null;
			}

			base.Dispose(disposing);
		}
	}
}