using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using ReactiveUI;

namespace Xmf2.Rx.Droid.LinearListView
{
	public class BaseReactiveLinearListViewAdapter<ItemData, ViewHolder> : LinearListViewAdapter where ItemData : class where ViewHolder : LinearListViewHolder
	{
		public new IReadOnlyList<ItemData> ItemsSource
		{
			get => base.ItemsSource as IReadOnlyList<ItemData>;
			set => base.ItemsSource = value;
		}

		public BaseReactiveLinearListViewAdapter(Context context) : base(context) { }

		protected BaseReactiveLinearListViewAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected override LinearListViewHolder CreateViewHolder(int position, Android.Views.View view)
		{
			var viewHolder = Activator.CreateInstance(typeof(ViewHolder), view) as ViewHolder;
			return viewHolder;
		}

		protected override void BindView(int position, LinearListViewHolder viewHolder)
		{
			if (viewHolder is IViewFor viewFor)
			{
				var item = GetRawItem(position);
				viewFor.ViewModel = item;
				viewHolder.ItemClick = ItemClick;
			}
		}
	}
}
