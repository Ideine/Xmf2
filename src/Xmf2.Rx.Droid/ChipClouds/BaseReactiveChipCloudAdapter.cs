using System;
using Android.Content;
using Android.Views;
using ReactiveUI;

namespace Xmf2.Rx.Droid.ChipClouds
{
	public class BaseReactiveChipCloudAdapter<ItemData, ViewHolder> : ChipCloudAdapter where ViewHolder : ChipCloudViewHolder
	{
		private readonly LayoutInflater _layoutInflater;

		public BaseReactiveChipCloudAdapter(Context context) : base(context)
		{
			_layoutInflater = LayoutInflater.From(Context);
		}

		public override ChipCloudViewHolder OnCreateViewHolder(ViewGroup parent, int position)
		{
			var view = _layoutInflater.Inflate(ItemTemplate, parent, false);
			var viewHolder = Activator.CreateInstance(typeof(ViewHolder), view) as ViewHolder;
			return viewHolder;
		}

		public override void OnBindViewHolder(ChipCloudViewHolder holder, int position)
		{
			var viewFor = holder as IViewFor;
			if (viewFor != null)
			{
				var item = ItemAt(position);
				viewFor.ViewModel = item;
			}
		}
	}
}
