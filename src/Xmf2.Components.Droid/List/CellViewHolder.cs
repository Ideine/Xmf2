using System;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Droid.List
{
	public class StickyViewHolder : RecyclerView.ViewHolder
	{
		public StickyViewHolder(View itemView) : base(itemView)
		{
			IsRecyclable = false;
		}

		protected StickyViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }
	}

	public class HeaderViewHolder : CellViewHolder
	{
		protected HeaderViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public HeaderViewHolder(IComponentView component, View view) : base(component, view)
		{
			IsRecyclable = false;
		}
	}

	public class FooterViewHolder : CellViewHolder
	{
		protected FooterViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public FooterViewHolder(IComponentView component, View view) : base(component, view)
		{
			IsRecyclable = false;
		}
	}

	public class CellViewHolder : RecyclerView.ViewHolder
	{
		public IComponentView Component { get; private set; }

		protected CellViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public CellViewHolder(IComponentView component, View view) : base(view)
		{
			Component = component;
		}

		public virtual void OnViewAttachedToWindow() { }

		public virtual void OnViewDetachedFromWindow() { }

		public virtual void OnViewRecycled() { }

		public void SetState(IViewState state)
		{
			Component.SetState(state);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Component = null;
			}

			base.Dispose(disposing);
		}
	}
}