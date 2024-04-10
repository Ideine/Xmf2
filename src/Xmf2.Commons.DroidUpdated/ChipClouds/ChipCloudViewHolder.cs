using System;
using Android.Views;

namespace Xmf2.Commons.Droid.ChipClouds
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

		~ChipCloudViewHolder()
		{
			Dispose(false);
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

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(disposing)
			{
				if (ItemView != null)
				{
					ItemView.ViewDetachedFromWindow -= NotifyViewDetachedFromWindow;
					ItemView.ViewAttachedToWindow -= NotifyViewAttachedToWindow;
				}
			}
		}
	}
}
