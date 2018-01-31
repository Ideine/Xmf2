using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using ReactiveUI;
using Xmf2.Commons.Droid.ChipClouds;

namespace Xmf2.Rx.Droid.ChipClouds
{
	public class BaseReactiveChipCloudAdapter<ItemData, ViewHolder> : ChipCloudAdapter where ViewHolder : ChipCloudViewHolder
	{
		public BaseReactiveChipCloudAdapter(Context context) : base(context) { }

		public new List<ItemData> ItemSource
		{
			get => base.ItemSource as List<ItemData>;
			set => base.ItemSource = value.Select(x => x as object).ToList();
		}

		public override ChipCloudViewHolder OnCreateViewHolder(ViewGroup parent, int position)
		{
			View view;
			using (var inflater = LayoutInflater.FromContext(Context))
			{
				view = inflater.Inflate(ItemTemplate, parent, false);
			}
			var viewHolder = Activator.CreateInstance(typeof(ViewHolder), view) as ViewHolder;
			return viewHolder;

		}

		public override void OnBindViewHolder(ChipCloudViewHolder holder, int position)
		{
			if (holder is IViewFor viewFor)
			{
				var item = ItemAt(position);
				viewFor.ViewModel = item;
			}
		}
	}
}
