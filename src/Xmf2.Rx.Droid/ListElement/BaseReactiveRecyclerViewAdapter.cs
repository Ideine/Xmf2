using System;
using System.Windows.Input;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using ReactiveUI;

namespace Xmf2.Rx.Droid.ListElement
{
	public class BaseReactiveRecyclerViewAdapter<TItemData, TViewHolder> : RecyclerView.Adapter where TViewHolder : RecyclerView.ViewHolder, IRecyclerViewViewHolder
	{
		public int ItemTemplate { get; set; }

		public ICommand ItemClick { get; set; }

		public ICommand ItemLongClick { get; set; }

		protected readonly Context Context;

		private IReadOnlyReactiveList<TItemData> _itemsSource;
		public IReadOnlyReactiveList<TItemData> ItemsSource
		{
			get => _itemsSource;
			set
			{
				if (!Equals(_itemsSource, value))
				{
					_itemsSource = value;
					_itemsSource.CollectionChanged += OnCollectionChanged;
				}
				NotifyDataSetChanged();
			}
		}

		public BaseReactiveRecyclerViewAdapter(Context context)
		{
			Context = context;
		}

		protected BaseReactiveRecyclerViewAdapter(IntPtr javaRef, Android.Runtime.JniHandleOwnership transfer) : base(javaRef, transfer)
		{
		}

		public override int ItemCount => ItemsSource?.Count ?? 0;

		object ItemAt(int position)
		{
			return ItemsSource[position];
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			if (holder is IViewFor viewFor)
			{
				viewFor.ViewModel = ItemAt(position);
			}
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			var view = LayoutInflater.From(Context).Inflate(ItemTemplate, parent, false);
			var viewHolder = Activator.CreateInstance(typeof(TViewHolder), view) as TViewHolder;
			viewHolder.ItemClick = ItemClick;
			viewHolder.ItemLongClick = ItemLongClick;
			return viewHolder;
		}

		private void OnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			new Handler(Looper.MainLooper).Post(() => NotifyDataSetChanged());

		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (ItemsSource != null)
				{
					ItemsSource.CollectionChanged -= OnCollectionChanged;
				}
			}
			base.Dispose(disposing);
		}
		#region View holders Lifecycle

		public override void OnViewAttachedToWindow(Java.Lang.Object holder)
		{
			base.OnViewAttachedToWindow(holder);
			(holder as IRecyclerViewViewHolder)?.OnViewAttachedToWindow();
		}

		public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
		{
			(holder as IRecyclerViewViewHolder)?.OnViewDetachedFromWindow();
			base.OnViewDetachedFromWindow(holder);
		}

		public override void OnViewRecycled(Java.Lang.Object holder)
		{
			(holder as IRecyclerViewViewHolder)?.OnViewRecycled();
			base.OnViewRecycled(holder);
		}

		#endregion
	}
}
