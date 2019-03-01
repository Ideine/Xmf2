using System;
using System.Linq;
using Android.Views;
using Android.Runtime;
using Xmf2.Core.Subscriptions;
using Android.Support.V7.Util;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Droid.List;
using Xmf2.Components.Droid.Interfaces;

namespace Xmf2.Components.Droid.RecyclerList
{
	public class CommonAdapter : RecyclerView.Adapter
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();

		private Func<string, IComponentView> _componentCreatorMap;

		private static IEntityViewState[] _defaultState = new IEntityViewState[0];

		private IReadOnlyList<IEntityViewState> _itemSource;
		public IReadOnlyList<IEntityViewState> ItemSource
		{
			get => _itemSource;
			set
			{
				using (var callback = new DiffList(_itemSource?.ToArray() ?? _defaultState, value?.ToArray() ?? _defaultState))
				{
					using (var result = DiffUtil.CalculateDiff(callback))
					{
						_itemSource = value;
						NotifyDataSetChanged();
					}
				}
			}
		}

		protected CommonAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public CommonAdapter(Func<string, IComponentView> componentCreator)
		{
			_componentCreatorMap = componentCreator;
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			if (holder is CellViewHolder cellViewHolder)
			{
				int pos = position;
				if (pos < ItemSource.Count)
				{
					cellViewHolder.SetState(ItemSource[pos]);
				}
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var index = viewType;
			IComponentView component = _componentCreatorMap(Guid.NewGuid().ToString()).DisposeViewWith(_disposable);
			return new CellViewHolder(component, component.View(parent)).DisposeViewWith(_disposable);
		}

		public override int ItemCount => ItemSource?.Count ?? 0;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_componentCreatorMap = null;
				_disposable.Dispose();
			}

			base.Dispose(disposing);
		}

		#region View Holder Lifecycle

		public override void OnViewAttachedToWindow(Java.Lang.Object holder)
		{
			base.OnViewAttachedToWindow(holder);
			if (holder is CellViewHolder commonViewHolder)
			{
				commonViewHolder.OnViewAttachedToWindow();
			}
		}

		public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
		{
			if (holder is CellViewHolder commonViewHolder)
			{
				commonViewHolder.OnViewDetachedFromWindow();
			}

			base.OnViewDetachedFromWindow(holder);
		}

		public override void OnViewRecycled(Java.Lang.Object holder)
		{
			if (holder is CellViewHolder commonViewHolder)
			{
				commonViewHolder.OnViewRecycled();
			}

			base.OnViewRecycled(holder);
		}

		#endregion
	}
}