﻿using AndroidX.RecyclerView.Widget;

namespace Xmf2.Components.Droid.List
{
	public class CustomDiffDispatcher : Java.Lang.Object, IListUpdateCallback
	{
		private readonly int _shiftCount;
		private RecyclerView.Adapter _adapter;

		public CustomDiffDispatcher(int shiftCount, RecyclerView.Adapter adapter)
		{
			_shiftCount = shiftCount;
			_adapter = adapter;
		}

		public void OnChanged(int position, int count, Java.Lang.Object payload)
		{
			if (count == 1)
			{
				_adapter.NotifyItemChanged(position + _shiftCount, payload);
			}
			else
			{
				_adapter.NotifyItemRangeChanged(position + _shiftCount, count, payload);
			}
		}

		public void OnInserted(int position, int count)
		{
			_adapter.NotifyItemRangeInserted(position + _shiftCount, count);
		}

		public void OnMoved(int fromPosition, int toPosition)
		{
			_adapter.NotifyItemMoved(fromPosition + _shiftCount, toPosition + _shiftCount);
		}

		public void OnRemoved(int position, int count)
		{
			_adapter.NotifyItemRangeRemoved(position + _shiftCount, count);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_adapter = null;
			}

			base.Dispose(disposing);
		}
	}
}