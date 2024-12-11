using System;
using System.Collections;
using System.Collections.Specialized;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Xmf2.Droid.Rx.Views
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
				IAdapterWithChangedEvent existing = _adapter;
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

		private UpdateListViewController _updateListViewController;

		public ILinearItemsLayoutAdapter CustomLayoutAdapter { get; set; }

		#region Constructor

		public LinearListView(Context context) : base(context) => Initialize();

		public LinearListView(Context context, Android.Util.IAttributeSet attrs) : base(context, attrs) => Initialize();

		public LinearListView(Context context, Android.Util.IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) => Initialize();

		protected LinearListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) => Initialize();

		#endregion

		private void Initialize()
		{
			Orientation = Orientation.Vertical;
			_updateListViewController = new UpdateListViewController(this);
		}

		private void AdapterOnDataSetChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
		{
			new Handler(Looper.MainLooper).Post(() =>
			{
				if (CustomLayoutAdapter == null)
				{
					_updateListViewController.UpdateDataSetFromChange(sender, eventArgs);
				}
				else
				{
					CustomLayoutAdapter.UpdateDataSetFromChange(this, Adapter, Adapter.ItemsSource);
				}
			});
		}

		private void OnChildViewRemoved(object sender, ChildViewRemovedEventArgs childViewRemovedEventArgs)
		{
			View boundChild = childViewRemovedEventArgs.Child;
			boundChild?.Dispose();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Adapter?.Dispose();
				Adapter = null;
			}

			base.Dispose(disposing);
		}

		public class UpdateListViewController
		{
			private LinearListView _linearListView;

			public UpdateListViewController(LinearListView viewGroup)
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

			private static void Refill(ViewGroup viewGroup, IAdapter adapter)
			{
				if (viewGroup is null || adapter is null)
				{
					return;
				}

				viewGroup.RemoveAllViews();
				int count = adapter.Count;
				for (int i = 0 ; i < count ; i++)
				{
					viewGroup.AddView(adapter.GetView(i, null, viewGroup));
				}
			}

			private static void Add(ViewGroup viewGroup, IAdapter adapter, int insertionIndex, int count)
			{
				for (int i = 0 ; i < count ; i++)
				{
					viewGroup.AddView(adapter.GetView(insertionIndex + i, null, viewGroup), insertionIndex + i);
				}
			}

			private static void Remove(ViewGroup viewGroup, IAdapter adapter, int removalIndex, int count)
			{
				for (int i = 0 ; i < count ; i++)
				{
					viewGroup.RemoveViewAt(removalIndex + i);
				}
			}

			private void Replace(ViewGroup viewGroup, IAdapter adapter, int startIndex, int count)
			{
				for (int i = 0 ; i < count ; i++)
				{
					viewGroup.RemoveViewAt(startIndex + i);
					viewGroup.AddView(adapter.GetView(startIndex + i, null, viewGroup), startIndex + i);
				}
			}

			#region IDisposable Support

			private bool _disposedValue; // To detect redundant calls

			protected virtual void Dispose(bool disposing)
			{
				if (!_disposedValue)
				{
					if (disposing)
					{
						_linearListView = null;
					}

					_disposedValue = true;
				}
			}

			~UpdateListViewController()
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