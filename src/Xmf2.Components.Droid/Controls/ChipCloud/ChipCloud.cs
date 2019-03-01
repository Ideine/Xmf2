using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Xmf2.Components.Droid.LinearList;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.Controls.ChipCloud
{
	public class ChipCloud : FlowLayout
	{
		protected Xmf2Disposable Disposable = new Xmf2Disposable();

		private ChipCloudAdapter _adapter;
		private EventSubscriber<ChipCloudAdapter> _subscriber = null;

		public ChipCloudAdapter Adapter
		{
			get => _adapter;
			set
			{
				if (_adapter != value && value != null)
				{
					if (_adapter != null)
					{
						_adapter.ItemSourceChanged -= ItemSourceChanged;
					}
					_subscriber?.Dispose();
					_subscriber = null;
					_subscriber = new EventSubscriber<ChipCloudAdapter>(
						value,
						v => v.ItemSourceChanged += ItemSourceChanged,
						v => v.ItemSourceChanged -= ItemSourceChanged
					).DisposeWith(Disposable);

					_adapter = value;
				}
			}
		}

		protected ChipCloud(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public ChipCloud(Context context) : base(context) { }

		public ChipCloud(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public ChipCloud(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		private void ItemSourceChanged(List<Remove> removeList, List<Move> moveList, List<Add> addList)
		{
			View[] childs = Enumerable.Range(0, ChildCount).Select(GetChildAt).ToArray();

			foreach (var remove in removeList.OrderByDescending(x => x.OldPos))
			{
				RemoveViewAt(remove.OldPos);
			}

			foreach (var move in moveList)
			{
				var view = childs[move.OldPos];
				RemoveView(view);
				AddView(view, move.NewPos);
			}

			foreach (var add in addList)
			{
				AddView(Adapter.GetView(add.NewPos, null, this).DisposeViewWith(Disposable), add.NewPos);
			}

			Adapter.RefreshAllStates();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Disposable?.Dispose();
				Disposable = null;
				Adapter = null;
			}

			base.Dispose(disposing);
		}
	}
}
