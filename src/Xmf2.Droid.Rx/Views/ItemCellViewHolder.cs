using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Xmf2.Common.Extensions;

namespace Xmf2.Droid.Rx.Views
{
	public class ItemCellViewHolder : Java.Lang.Object
	{
		public Context Context { get; }

		public View ItemView { get; }

		private bool _clickOverloaded;
		private ICommand _itemClick;
		private object _dataContext;

		public ICommand ItemClick
		{
			get => _itemClick;
			set
			{
				_itemClick = value;
				if (_itemClick != null)
				{
					EnsureClickOverloaded();
				}
			}
		}

		public object DataContext
		{
			get => _dataContext;
			set
			{
				if (value != null)
				{
					_dataContext = value;
				}
			}
		}

		private void EnsureClickOverloaded()
		{
			if (_clickOverloaded)
			{
				return;
			}

			_clickOverloaded = true;
			ItemView.Click += OnClickOnItemView;
		}

		public ItemCellViewHolder(View itemView)
		{
			ItemView = itemView;
			Context = ItemView.Context;
		}

		public ItemCellViewHolder() { }

		protected ItemCellViewHolder(IntPtr javaRef, JniHandleOwnership transfer) : base(javaRef, transfer) { }

		public virtual void OnClickOnItemView(object sender, EventArgs e) => ItemClick?.TryExecute(GetItemClickParameter());

		protected virtual object GetItemClickParameter() => DataContext;

		protected override void Dispose(bool disposing)
		{
			if (ItemView != null)
			{
				ItemView.Click -= OnClickOnItemView;
			}

			base.Dispose(disposing);
		}
	}
}