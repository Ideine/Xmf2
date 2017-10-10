using System;
using Android.Views;

namespace Xmf2.Rx.Droid.ChipClouds
{
	public class ChipCloudViewHolder : IDisposable
	{
		public readonly View ItemView;

		public ChipCloudViewHolder(View itemView)
		{
			ItemView = itemView;
			ItemView.ViewAttachedToWindow += NotifyViewAttachedToWindow;
			ItemView.ViewDetachedFromWindow += NotifyViewDetachedFromWindow;
		}

		void NotifyViewAttachedToWindow(object sender, View.ViewAttachedToWindowEventArgs e)
		{
			OnViewAttachedToWindow();
		}

		void NotifyViewDetachedFromWindow(object sender, View.ViewDetachedFromWindowEventArgs e)
		{
			OnViewDetachedFromWindow();
			OnViewRecycled();
		}

		public virtual void OnViewAttachedToWindow() { }

		public virtual void OnViewDetachedFromWindow() { }

		public virtual void OnViewRecycled()
		{
			Dispose();
		}

		public virtual void Dispose()
		{
			if (ItemView != null)
			{
				ItemView.ViewDetachedFromWindow -= NotifyViewDetachedFromWindow;
				ItemView.ViewAttachedToWindow -= NotifyViewAttachedToWindow;
			}
		}
	}
}
