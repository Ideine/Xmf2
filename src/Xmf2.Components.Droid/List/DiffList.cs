using System;
using Android.Runtime;
#if __ANDROID_29__
using AndroidX.RecyclerView.Widget;
#else
using Android.Support.V7.Util;
#endif
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Droid.List
{
	public class DiffList : DiffUtil.Callback
	{
		private IEntityViewState[] _oldList;
		private IEntityViewState[] _newList;

		public DiffList(IEntityViewState[] oldList, IEntityViewState[] newList)
		{
			_oldList = oldList;
			_newList = newList;
		}

		protected DiffList(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override bool AreContentsTheSame(int oldItemPosition, int newItemPosition)
		{
			return false;
		}

		public override bool AreItemsTheSame(int oldItemPosition, int newItemPosition)
		{
			return _oldList[oldItemPosition].Id == _newList[newItemPosition].Id;
		}

		public override int NewListSize => _newList?.Length ?? 0;
		public override int OldListSize => _oldList?.Length ?? 0;

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_oldList = null;
				_newList = null;
			}

			base.Dispose(disposing);
		}
	}
}