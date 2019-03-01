using System;
using Android.Views;
using Android.Runtime;
using Android.Support.V7.Widget;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Droid.Interfaces;

namespace Xmf2.Components.Droid.RecyclerList
{
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

