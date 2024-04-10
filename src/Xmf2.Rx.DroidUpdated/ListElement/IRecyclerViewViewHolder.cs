using System;
using System.Windows.Input;

namespace Xmf2.Rx.Droid.ListElement
{
	public interface IRecyclerViewViewHolder
	{
		ICommand ItemClick { get; set; }

		ICommand ItemLongClick { get; set; }

		void OnViewAttachedToWindow();

		void OnViewDetachedFromWindow();

		void OnViewRecycled();
	}
}
