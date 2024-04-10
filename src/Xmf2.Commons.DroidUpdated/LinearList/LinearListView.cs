using System;
using System.Collections;
using System.Collections.Specialized;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Xmf2.Commons.Droid.LinearList
{
	public interface ILinearItemsLayoutAdapter
	{
		void UpdateDataSetFromChange(ViewGroup viewGroup, IAdapter adapter, IEnumerable items);
	}

	public class LinearListView : LinearLayout
	{
		public int ItemTemplateId
		{
			get => Adapter.ItemTemplateId;
			set => Adapter.ItemTemplateId = value;
		}

		private IAdapterWithChangedEvent _adapter;
		public IAdapterWithChangedEvent Adapter
		{
			get => _adapter;
			set
			{
				var existing = _adapter;
				if (existing == value)
				{
					return;
				}

				if (existing != null)
				{
					existing.DataSetChanged -= AdapterOnDataSetChanged;
					if (value != null)
					{
						value.ItemsSource = existing.ItemsSource;
						value.ItemTemplateId = existing.ItemTemplateId;
					}
				}

				_adapter = value;

				if (_adapter != null)
				{
					_adapter.DataSetChanged += AdapterOnDataSetChanged;
				}

				if (existing != null)
				{
					existing.ItemsSource = null;
				}
			}
		}

		private LinearItemLayoutAdapter _classicLayoutAdapter;

		public ILinearItemsLayoutAdapter CustomLayoutAdapter { get; set; }

		#region Constructor

		public LinearListView(Context context) : base(context) { Init(); }

		public LinearListView(Context context, IAttributeSet attrs) : base(context, attrs) { Init(); }

		public LinearListView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { Init(); }

		protected LinearListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { Init(); }

		#endregion

		void Init()
		{
			_classicLayoutAdapter = new LinearItemLayoutAdapter(this);
		}

		public void AdapterOnDataSetChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
		{
			if (CustomLayoutAdapter == null)
			{
				_classicLayoutAdapter.UpdateDataSetFromChange(sender, eventArgs);
			}
			else
			{
				CustomLayoutAdapter.UpdateDataSetFromChange(this, Adapter, Adapter.ItemsSource);
			}
		}

		private void OnChildViewRemoved(object sender, ChildViewRemovedEventArgs childViewRemovedEventArgs)
		{
			var boundChild = childViewRemovedEventArgs.Child;
			boundChild?.Dispose();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_adapter != null)
				{
					_adapter.DataSetChanged -= AdapterOnDataSetChanged;
					_adapter.Dispose();
				}
				_adapter = null;
			}
			base.Dispose(disposing);
		}

		public class LinearItemLayoutAdapter : IDisposable
		{
			private LinearListView _linearListView;

			public LinearItemLayoutAdapter(LinearListView viewGroup)
			{
				_linearListView = viewGroup;
			}

			public void UpdateDataSetFromChange(object sender, NotifyCollectionChangedEventArgs eventArgs)
			{
				switch (eventArgs.Action)
				{
					case NotifyCollectionChangedAction.Add:
						Add(_linearListView, _linearListView.Adapter, eventArgs.NewStartingIndex, eventArgs.NewItems.Count);
						break;

					case NotifyCollectionChangedAction.Remove:
						Remove(_linearListView, _linearListView.Adapter, eventArgs.OldStartingIndex, eventArgs.OldItems.Count);
						break;

					case NotifyCollectionChangedAction.Replace:
						if (eventArgs.NewItems.Count != eventArgs.OldItems.Count)
						{
							Refill(_linearListView, _linearListView.Adapter);
						}
						else
						{
							Replace(_linearListView, _linearListView.Adapter, eventArgs.NewStartingIndex, eventArgs.NewItems.Count);
						}
						break;

					case NotifyCollectionChangedAction.Move:
						// move is not implemented - so we call Refill instead
						Refill(_linearListView, _linearListView.Adapter);
						break;

					case NotifyCollectionChangedAction.Reset:
						Refill(_linearListView, _linearListView.Adapter);
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			private void Refill(ViewGroup viewGroup, IAdapter adapter)
			{
				try
				{
					viewGroup.RemoveAllViews();
					var count = adapter.Count;
					for (var i = 0; i < count; i++)
					{
						viewGroup.AddView(adapter.GetView(i, null, viewGroup));
					}
				}
				catch (Exception e)
				{
					//viewGroup can be null or disposed if Refill come after a Dispose
					System.Diagnostics.Debug.WriteLine(e);
				}
			}

			private void Add(ViewGroup viewGroup, IAdapter adapter, int insertionIndex, int count)
			{
				for (var i = 0; i < count; i++)
				{
					viewGroup.AddView(adapter.GetView(insertionIndex + i, null, viewGroup), insertionIndex + i);
				}
			}

			private void Remove(ViewGroup viewGroup, IAdapter adapter, int removalIndex, int count)
			{
				for (var i = 0; i < count; i++)
				{
					viewGroup.RemoveViewAt(removalIndex + i);
				}
			}

			private void Replace(ViewGroup viewGroup, IAdapter adapter, int startIndex, int count)
			{
				for (var i = 0; i < count; i++)
				{
					viewGroup.RemoveViewAt(startIndex + i);
					viewGroup.AddView(adapter.GetView(startIndex + i, null, viewGroup), startIndex + i);
				}
			}

			#region IDisposable Support
			private bool disposedValue = false; // To detect redundant calls

			protected virtual void Dispose(bool disposing)
			{
				if (!disposedValue)
				{
					if (disposing)
					{
						_linearListView = null;
					}
					disposedValue = true;
				}
			}


			~LinearItemLayoutAdapter()
			{
				Dispose(false);
			}


			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			#endregion
		}

	}
}
