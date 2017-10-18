using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xmf2.Commons.Droid.LinearList
{
	public interface IAdapterWithChangedEvent : IAdapter
	{
		event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;

		IReadOnlyList<object> ItemsSource { get; set; }

		int ItemTemplateId { get; set; }

		ICommand ItemClick { get; set; }

		object GetRawItem(int position);
	}

	public class LinearListViewAdapter : BaseAdapter, IAdapterWithChangedEvent
	{
		public Context Context { get; }

		public bool ReloadOnAllItemsSourceSets { get; set; }

		public event EventHandler<NotifyCollectionChangedEventArgs> DataSetChanged;

		public int ItemTemplateId { get; set; }

		public ICommand ItemClick { get; set; }

		private IReadOnlyList<object> _itemsSource;
		public virtual IReadOnlyList<object> ItemsSource
		{
			get => _itemsSource;
			set => SetItemsSource(value);
		}

		public override int Count => _itemsSource.Count;

		public LinearListViewAdapter(Context context)
		{
			Context = context;
		}

		protected LinearListViewAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected virtual void SetItemsSource(IReadOnlyList<object> value)
		{
			if (ReferenceEquals(_itemsSource, value) && !ReloadOnAllItemsSourceSets)
			{
				return;
			}

			_itemsSource = value;

			if (_itemsSource != null && !(_itemsSource is IList))
			{
				Console.WriteLine("You are currently binding to IEnumerable - this can be inefficient, especially for large collections. Binding to IList is more efficient.");
			}

			if (_itemsSource is INotifyCollectionChanged newObservable)
			{
				newObservable.CollectionChanged += OnItemsSourceCollectionChanged;
			}
			NotifyDataSetChanged();
		}

		protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			NotifyDataSetChanged(e);
		}

		public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
		{
			new Handler(Looper.MainLooper).Post(() => DataSetChanged?.Invoke(this, e));
		}

		public override void NotifyDataSetChanged()
		{
			new Handler(Looper.MainLooper).Post(() => DataSetChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)));
		}

		public virtual object GetRawItem(int position) => ItemsSource[position];

		public override Java.Lang.Object GetItem(int position) => null;

		public override long GetItemId(int position) => position;

		#region Create and Bind View

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			if (ItemsSource == null)
			{
				Console.WriteLine("GetView called when ItemsSource is null");
				return null;
			}

			if (convertView == null)
			{
				var layoutInflater = LayoutInflater.From(Context);
				convertView = layoutInflater.Inflate(ItemTemplateId, parent, false);
			}

			if (convertView.Tag == null)
			{
				convertView.Tag = CreateViewHolder(position, convertView);
			}

			BindView(position, convertView.Tag as LinearListViewHolder);

			return convertView;
		}

		protected virtual void BindView(int position, LinearListViewHolder viewHolder)
		{
			var dataContext = GetRawItem(position);
			viewHolder.DataContext = dataContext;
		}

		protected virtual LinearListViewHolder CreateViewHolder(int position, View view)
		{
			return new LinearListViewHolder(view);
		}

		#endregion
	}
}
