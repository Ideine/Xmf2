using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Xmf2.Core.Subscriptions;
using System.Collections.Generic;
using System.Linq;
using Android.Views;

namespace Xmf2.Components.Droid.LinearList
{
	public class AndroidLinearListView : LinearLayout
	{
		protected Xmf2Disposable Disposable = new Xmf2Disposable();

		private LinearListAdapter _adapter;
		private EventSubscriber<LinearListAdapter> _subscriber;

		public LinearListAdapter Adapter
		{
			get => _adapter;
			set
			{
				if (_adapter != value && value != null)
				{
					if (_adapter != null)
					{
						_adapter.ItemSourceChanged -= ItemSourceChanged;
						_adapter.ItemSourceChanged -= ItemSourceWithSeparatorChanged;
					}
					_subscriber?.Dispose();
					_subscriber = null;

					if (value is IAdapterWithSeparator)
					{
						_subscriber = new EventSubscriber<LinearListAdapter>(
							value,
							v => v.ItemSourceChanged += ItemSourceWithSeparatorChanged,
							v => v.ItemSourceChanged -= ItemSourceWithSeparatorChanged
						).DisposeWith(Disposable);
					}
					else
					{
						_subscriber = new EventSubscriber<LinearListAdapter>(
							value,
							v => v.ItemSourceChanged += ItemSourceChanged,
							v => v.ItemSourceChanged -= ItemSourceChanged
						).DisposeWith(Disposable);
					}

					_adapter = value;
				}
			}
		}

		protected AndroidLinearListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public AndroidLinearListView(Context context) : base(context) { }

		public AndroidLinearListView(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public AndroidLinearListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		private void ItemSourceChanged(List<Remove> removeList, List<Move> moveList, List<Add> addList)
		{
			View[] childs = Enumerable.Range(0, ChildCount).Select(GetChildAt).ToArray();

			foreach (var remove in removeList.OrderByDescending(x => x.OldPos))
			{
				RemoveViewAt(remove.OldPos);
			}

			foreach (var add in addList)
			{
				AddView(Adapter.GetView(add.NewPos, null, this).DisposeViewWith(Disposable), add.NewPos);
			}

			foreach (var move in moveList)
			{
				var view = childs[move.OldPos];
				RemoveView(view);
				AddView(view, move.NewPos);
			}

			Adapter.RefreshAllStates();
		}

		private void ItemSourceWithSeparatorChanged(List<Remove> removeList, List<Move> moveList, List<Add> addList)
		{
			IAdapterWithSeparator adapterWithSeparator = (IAdapterWithSeparator)Adapter;
			View[] childs = Enumerable.Range(0, ChildCount).Select(GetChildAt).ToArray();
			int realItemCount = childs.Length / 2;


			foreach (Remove remove in removeList.OrderByDescending(x => x.OldPos))
			{
				realItemCount--;
				if (remove.OldPos == 0)
				{
					RemoveViewAt(0);
				}
				else
				{
					var cellIndex = remove.OldPos * 2;
					var separatorIndex = cellIndex - 1;

					RemoveViewAt(cellIndex);
					RemoveViewAt(separatorIndex);
				}
			}

			foreach (Move move in moveList)
			{
				View itemView = childs[move.OldPos * 2];
				View separatorView = null;
				RemoveView(itemView);
				if (realItemCount > 1)
				{
					separatorView = move.OldPos == realItemCount - 1 ? childs[move.OldPos * 2 - 1] : childs[move.OldPos * 2 + 1];
					RemoveView(separatorView);
				}
				realItemCount--;

				int addPosition = move.NewPos;
				if (addPosition > realItemCount)
				{
					addPosition = realItemCount;
				}

				AddView(itemView, addPosition * 2);

				if (separatorView != null)
				{
					if (addPosition == realItemCount)
					{
						AddView(separatorView, addPosition * 2);
					}
					else
					{
						AddView(separatorView, addPosition * 2 + 1);
					}
				}

				realItemCount++;
			}

			foreach (var add in addList.OrderBy(x => x.NewPos))
			{
				if (add.NewPos == 0)
				{
					if (realItemCount > 0)
					{
						AddView(adapterWithSeparator.CreateSeparator(), 0);
					}
				}
				else
				{
					AddView(adapterWithSeparator.CreateSeparator(), add.NewPos * 2 - 1);
				}

				AddView(Adapter.GetView(add.NewPos, null, this).DisposeViewWith(Disposable), add.NewPos * 2);
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