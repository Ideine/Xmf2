#if __ANDROID_29__
using AndroidX.RecyclerView.Widget;
#else
using Android.Support.V7.Util;
using Android.Support.V7.Widget;
#endif

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
			_adapter.NotifyItemRangeChanged(position + _shiftCount, count, payload);
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