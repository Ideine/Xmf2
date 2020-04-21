﻿using System;
using System.Linq;
using Android.Views;
using Xmf2.Core.Subscriptions;
#if __ANDROID_29__
using AndroidX.RecyclerView.Widget;
#else
using Android.Support.V7.Widget;
#endif
using Xmf2.Components.Interfaces;
using Xmf2.Components.Droid.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.ViewModels.LinearLists;

namespace Xmf2.Components.Droid.List
{
	public abstract class RecyclerItemsView : BaseComponentView<ListViewState>
	{
		protected RecyclerView RecyclerView { get; private set; }

		private CommonAdapter _adapter;

		private Func<IServiceLocator, IComponentView> _factory;

		public RecyclerItemsView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services)
		{
			_factory = factory;
			_adapter = CreateAdapter().DisposeViewWith(Disposables);
		}

		protected override View RenderView()
		{
			RecyclerView = new RecyclerView(Context).DisposeViewWith(Disposables);
			RecyclerView.SetAdapter(_adapter);
			SetLayoutManager();
			OnDesignView();
			return RecyclerView;
		}

		protected abstract void SetLayoutManager();

		protected virtual CommonAdapter CreateAdapter()
		{
			return new CommonAdapter(typeof(IEntityViewState), id => _factory(Services)).DisposeWith(Disposables);
		}

		protected virtual void OnDesignView() { }

		protected override void OnStateUpdate(ListViewState state)
		{
			if (CurrentState != null && state.Items.SequenceEqual(CurrentState.Items))
			{
				return;
			}

			base.OnStateUpdate(state);
			_adapter.ItemSource = state.Items;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_factory = null;
				RecyclerView = null;
				_adapter = null;
			}

			base.Dispose(disposing);
		}
	}
}