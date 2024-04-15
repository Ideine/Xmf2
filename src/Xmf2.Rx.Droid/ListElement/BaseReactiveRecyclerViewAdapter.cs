using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows.Input;
using Android.Content;
using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using ReactiveUI;

namespace Xmf2.Rx.Droid.ListElement
{
	public class BaseReactiveRecyclerViewAdapter<TItemData, TViewHolder> : RecyclerView.Adapter where TViewHolder : RecyclerView.ViewHolder, IRecyclerViewViewHolder
	{
		protected CompositeDisposable UiDispo = new CompositeDisposable();

        public int ItemTemplate { get; set; }

		public ICommand ItemClick { get; set; }

		public ICommand ItemLongClick { get; set; }

		protected readonly Context Context;

        private BindingList<TItemData> itemsSource;
        public BindingList<TItemData> ItemsSource
        {
            get => itemsSource;
            set
            {
                if (!Equals(itemsSource, value))
                {
                    itemsSource = value;
                    itemsSource.ListChanged += ListChanged;
                }
                NotifyDataSetChanged();
            }
        }

        private void ListChanged(object sender, ListChangedEventArgs e)
        {
            new Handler(Looper.MainLooper).Post(() =>
            {
                try
                {
                    switch (e.ListChangedType)
                    {
                        case ListChangedType.ItemAdded:
                            NotifyItemInserted(e.NewIndex);
                            break;
                        //case ListChangedType.ItemMoved:
                        //    NotifyItemMoved(e.OldIndex, e.NewIndex);
                        //    break;
                        //case ListChangedType.ItemDeleted:
                        //    NotifyItemRemoved(e.OldIndex);
                        //    break;
                        case ListChangedType.ItemChanged:
                            NotifyItemChanged(e.NewIndex);
                            break;
                        default:
                            NotifyDataSetChanged();
                            break;
                    }
                }
                catch (Exception)
                {
                    //Mostly occurs in default case when execute NotifyDataSetChanged();
                    //It was caused by calling manual Dispose on Adapter when setting new adapter to RecyclerView.
                    //Or Adapter instance was Disposed but not unhooked from Data Source events, and in event method was code working with context.
                }
            });
        }

        public BaseReactiveRecyclerViewAdapter(Context context)
		{
			Context = context;
		}

		protected BaseReactiveRecyclerViewAdapter(IntPtr javaRef, Android.Runtime.JniHandleOwnership transfer) : base(javaRef, transfer) { }

		public override int ItemCount => ItemsSource?.Count ?? 0;

		object ItemAt(int position)
		{
            return ItemsSource.ElementAt(position);
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
			View view;
			using (var inflater = LayoutInflater.From(Context))
			{
				view = inflater.Inflate(ItemTemplate, parent, false);
			}
			var viewHolder = Activator.CreateInstance(typeof(TViewHolder), view) as TViewHolder;
			viewHolder.ItemClick = ItemClick;
			viewHolder.ItemLongClick = ItemLongClick;
			return viewHolder.DisposeWith(UiDispo);
		}

        //public virtual void OnCollectionChanged(IChangeSet<TItemData> changeSet)
        //{
        //	new Handler(Looper.MainLooper).Post(() =>
        //	{
        //		try
        //		{
        //			int i = 0;
        //			foreach (var change in changeSet)
        //			{
        //                      switch (change.Reason)
        //                      {
        //                          case ListChangeReason.Add:
        //                              NotifyItemInserted(i);
        //                              break;
        //                          //case ListChangeReason.Moved:
        //                          //    NotifyItemMoved(change.OldStartingIndex, e.NewStartingIndex);
        //                          //    break;
        //                          case ListChangeReason.Remove:
        //                              //if (e.OldItems.Count > 1)
        //                              //{
        //                              //    NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
        //                              //}
        //                              //else
        //                              //{
        //                              //NotifyItemRemoved(i);
        //                              //}
        //                              break;
        //                          default:
        //                              NotifyDataSetChanged();
        //                              break;
        //                      }
        //				i++;
        //                  }
        //              }
        //		catch (Exception)
        //		{
        //			//Mostly occurs in default case when execute NotifyDataSetChanged();
        //			//It was caused by calling manual Dispose on Adapter when setting new adapter to RecyclerView.
        //			//Or Adapter instance was Disposed but not unhooked from Data Source events, and in event method was code working with context.
        //		}
        //	});
        //}


        //private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //	OnCollectionChanged(sender, e);
        //}

        protected virtual void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            new Handler(Looper.MainLooper).Post(() =>
            {
                try
                {
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            NotifyItemInserted(e.NewStartingIndex);
                            break;
                        case NotifyCollectionChangedAction.Move:
                            NotifyItemMoved(e.OldStartingIndex, e.NewStartingIndex);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            if (e.OldItems.Count > 1)
                            {
                                NotifyItemRangeRemoved(e.OldStartingIndex, e.OldItems.Count);
                            }
                            else
                            {
                                NotifyItemRemoved(e.OldStartingIndex);
                            }
                            break;
                        default:
                            NotifyDataSetChanged();
                            break;
                    }
                }
                catch (Exception)
                {
                    //Mostly occurs in default case when execute NotifyDataSetChanged();
                    //It was caused by calling manual Dispose on Adapter when setting new adapter to RecyclerView.
                    //Or Adapter instance was Disposed but not unhooked from Data Source events, and in event method was code working with context.
                }
            });
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(sender, e);
        }

        protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (ItemsSource != null)
				{
                    ItemsSource.ListChanged -= ListChanged;
				}
				UiDispo?.Dispose();
				UiDispo = null;
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
