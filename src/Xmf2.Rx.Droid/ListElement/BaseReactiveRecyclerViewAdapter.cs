using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Android.Content;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using ReactiveUI;
using Object = Java.Lang.Object;

namespace Xmf2.Rx.Droid.ListElement
{
    public class BaseReactiveRecyclerViewAdapter<TItemData, TViewHolder> : RecyclerView.Adapter where TViewHolder : RecyclerView.ViewHolder, IRecyclerViewViewHolder
    {
        public int ItemTemplate { get; set; }

        public ICommand ItemClick { get; set; }

        public ICommand ItemLongClick { get; set; }

        protected readonly Context Context;

        private ObservableCollection<TItemData> _itemsSource;
        public ObservableCollection<TItemData> ItemsSource
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

        public override int ItemCount => ItemsSource?.Count ?? 0;

        private object ItemAt(int position)
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

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged();
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

        public override void OnViewAttachedToWindow(Object holder)
        {
            base.OnViewAttachedToWindow(holder);
            (holder as IRecyclerViewViewHolder)?.OnViewAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Object holder)
        {
            (holder as IRecyclerViewViewHolder)?.OnViewDetachedFromWindow();
            base.OnViewDetachedFromWindow(holder);
        }

        public override void OnViewRecycled(Object holder)
        {
            (holder as IRecyclerViewViewHolder)?.OnViewRecycled();
            base.OnViewRecycled(holder);
        }
    }
}
