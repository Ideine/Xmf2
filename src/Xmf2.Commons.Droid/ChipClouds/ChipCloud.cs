using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Xmf2.Commons.Droid.Helpers;

namespace Xmf2.Commons.Droid.ChipClouds
{
	public class ChipCloud : FlowLayout, ChipCloudObserver
	{
		#region Properties

		public int Count => Adapter == null ? 0 : Adapter.Count;

		private ChipCloudAdapter _adapter;
		public ChipCloudAdapter Adapter
		{
			get => _adapter;
			set
			{
				_adapter = value;
				Adapter.DeleteObservers();
				Adapter.Subscribe(this);
				Refresh();
			}
		}

		#endregion

		#region Constructors

		protected ChipCloud(IntPtr handle, JniHandleOwnership transer) : base(handle, transer) { }

		public ChipCloud(Context context) : base(context)
		{
			Initialize();
		}

		public ChipCloud(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();
		}

		public ChipCloud(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Initialize();
		}

		void Initialize()
		{
			MinimumHorizontalSpacing = UIHelper.DpToPx(Context, 8);
			VerticalSpacing = UIHelper.DpToPx(Context, 8);
		}

		#endregion

		void Refresh()
		{
			if (Adapter != null)
			{
				RemoveAllViews();

				for (int position = 0; position < Count; position++)
				{
					var viewHolder = Adapter.OnCreateViewHolder(this, position);
					AddView(viewHolder.ItemView);
					Adapter.OnBindViewHolder(viewHolder, position);
				}
			}
			Invalidate();
		}

		public void Update(ChipCloudObserver observer, object data)
		{
			Refresh();
		}
	}
}
