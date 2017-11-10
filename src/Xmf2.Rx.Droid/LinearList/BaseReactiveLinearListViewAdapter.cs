using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using ReactiveUI;
using Xmf2.Commons.Droid.LinearList;

namespace Xmf2.Rx.Droid.LinearList
{
	public class BaseReactiveLinearListViewAdapter<TItemData, TViewHolder> : LinearListViewAdapter where TItemData : class where TViewHolder : LinearListViewHolder
	{
		public new IReadOnlyList<TItemData> ItemsSource
		{
			get => base.ItemsSource as IReadOnlyList<TItemData>;
			set => base.ItemsSource = value;
		}

		public BaseReactiveLinearListViewAdapter(Context context) : base(context) { }

		protected BaseReactiveLinearListViewAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected override LinearListViewHolder CreateViewHolder(int position, Android.Views.View view)
		{
			var viewHolder = Activator.CreateInstance(typeof(TViewHolder), view) as TViewHolder;
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
