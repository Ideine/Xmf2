using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;

namespace Xmf2.Rx.Droid.ChipClouds
{
	public abstract class ChipCloudAdapter : ChipCloudObsevable, IDisposable
	{
		private readonly List<ChipCloudObserver> _observers = new List<ChipCloudObserver>();

		#region Properties

		public Context Context { get; set; }

		public int ItemTemplate { get; set; }

		private List<object> _itemSource;
		public List<object> ItemSource
		{
			get => _itemSource;
			set
			{
				if (!Equals(_itemSource, value))
				{
					_itemSource = value;
				}

				if (value != null)
				{
					NotifyUpdate();
				}
			}
		}

		public int Count => ItemSource == null ? 0 : ItemSource.Count;

		#endregion

		public ChipCloudAdapter(Context context)
		{
			Context = context;
		}

		private void NotifyUpdate()
		{
			foreach (var item in _observers)
			{
				item.Update(item, ItemSource);
			}
		}

		public object ItemAt(int position)
		{
			return position < Count ? ItemSource[position] : null;
		}

		public abstract ChipCloudViewHolder OnCreateViewHolder(ViewGroup parent, int position);

		public virtual void OnBindViewHolder(ChipCloudViewHolder holder, int position) { }

		public void DeleteObservers()
		{
			_observers.Clear();
		}

		public void Subscribe(ChipCloudObserver observer)
		{
			if (!_observers.Contains(observer))
			{
				_observers.Add(observer);
			}
		}

		public void Dispose()
		{
			_observers.Clear();
		}
	}
}
