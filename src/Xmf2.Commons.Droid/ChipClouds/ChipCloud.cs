using System;
using Android.Content;
using Android.Runtime;
using Android.Util;

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
			MinimumHorizontalSpacing = DpToPx(Context, 8);
			VerticalSpacing = DpToPx(Context, 8);
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

		public static int PxToDp(Context context, float pixelValue)
		{
			return (int)((pixelValue) / context.Resources.DisplayMetrics.Density);
		}

		public static int DpToPx(Context context, float dpValue)
		{
			return (int)((dpValue) * context.Resources.DisplayMetrics.Density);
		}

		public static float SpToPx(Context context, float px)
		{
			return context.Resources.DisplayMetrics.ScaledDensity * px;
		}
	}
}
