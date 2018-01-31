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
		private readonly LayoutInflater _layoutInflater;

		public BaseReactiveChipCloudAdapter(Context context) : base(context)
		{
			_layoutInflater = LayoutInflater.From(Context);
		}

		public new List<ItemData> ItemSource
		{
			get => base.ItemSource as List<ItemData>;
			set => base.ItemSource = value.Select(x => x as object).ToList();
		}

		public override ChipCloudViewHolder OnCreateViewHolder(ViewGroup parent, int position)
		{
			var view = _layoutInflater.Inflate(ItemTemplate, parent, false);
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
