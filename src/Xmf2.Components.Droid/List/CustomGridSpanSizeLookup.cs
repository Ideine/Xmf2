using System;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;

namespace Xmf2.Components.Droid.List
{
	public class CustomGridSpanSizeLookup : GridLayoutManager.SpanSizeLookup
	{
		private CommonAdapter _adapter;
		private readonly int _nbCol;

		public CustomGridSpanSizeLookup(CommonAdapter adapter, int nbCol)
		{
			_adapter = adapter;
			_nbCol = nbCol;
		}

		protected CustomGridSpanSizeLookup(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override int GetSpanSize(int position)
		{
			if (_adapter.GetItemViewType(position) < CommonAdapter.HEADER_START_TYPE_INDEX)
			{
				//Cell
				return 1;
			}
			else
			{
				//Header & Footer
				return _nbCol;
			}
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
