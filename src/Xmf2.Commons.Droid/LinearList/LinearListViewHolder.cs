using System;
using System.Windows.Input;
using Android.Content;
using Android.Views;

namespace Xmf2.Commons.Droid.LinearList
{
	public class LinearListViewHolder : Java.Lang.Object, IDisposable
	{
		public Context Context { get; }

		public View ItemView { get; }

		public virtual bool HasBottomSeparator { get; set; }

		public virtual int ItemPosition { get; set; }

		private bool _isAttachedToWindow;
		private bool _clickOverloaded;
		private ICommand _itemClick;
		private object _dataContext;

		public ICommand ItemClick
		{
			get => this._itemClick;
			set
			{
				this._itemClick = value;
				if (this._itemClick != null)
				{
					this.EnsureClickOverloaded();
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
					this._dataContext = value;
				}
			}
		}

		private void EnsureClickOverloaded()
		{
			if (this._clickOverloaded)
			{
				return;
			}
			this._clickOverloaded = true;
			this.ItemView.Click += OnClickOnItemView;
		}

		public LinearListViewHolder(View itemView)
		{
			ItemView = itemView;
			Context = ItemView.Context;

			ItemView.ViewAttachedToWindow += ItemViewAttachedToWindow;
			ItemView.ViewDetachedFromWindow += ItemViewDetachedFromWindow;
		}

		protected LinearListViewHolder(IntPtr javaRef, Android.Runtime.JniHandleOwnership transfer) : base(javaRef, transfer) { }

		public virtual void OnClickOnItemView(object sender, EventArgs e)
		{
			ItemClick?.TryExecute(GetItemClickParameter());
		}

		protected virtual object GetItemClickParameter() => DataContext;

		public void ItemViewAttachedToWindow(object sender, View.ViewAttachedToWindowEventArgs e)
		{
			OnAttachedToWindow();
		}

		public void ItemViewDetachedFromWindow(object sender, View.ViewDetachedFromWindowEventArgs e)
		{
			OnAttachedToWindow();
		}

		protected virtual void OnAttachedToWindow()
		{
			this._isAttachedToWindow = true;
		}

		protected virtual void OnDetachedFromWindow()
		{
			this._isAttachedToWindow = false;
		}

		protected override void Dispose(bool disposing)
		{
			if (ItemView != null)
			{
				ItemView.ViewAttachedToWindow -= ItemViewAttachedToWindow;
				ItemView.ViewDetachedFromWindow -= ItemViewDetachedFromWindow;
				ItemView.Click -= OnClickOnItemView;
			}
			base.Dispose(disposing);
		}
	}
}
