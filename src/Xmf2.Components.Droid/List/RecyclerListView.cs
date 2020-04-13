using System;
using Android.Content;
using Android.Graphics.Drawables;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;
#if __ANDROID_29__
using AndroidX.RecyclerView.Widget;
#else
using Android.Support.V7.Widget;
#endif

namespace Xmf2.Components.Droid.List
{
	public class RecyclerListView : RecyclerItemsView
	{
		public virtual int Orientation => LinearLayoutManager.Vertical;

		public RecyclerListView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services, factory) { }

		protected override void SetLayoutManager()
		{
			using var lm = new LinearLayoutManager(Context, Orientation, reverseLayout: false);
			RecyclerView.SetLayoutManager(lm);
		}

		protected RecyclerView.ItemDecoration CreateDrawableSeparator(Context context, Drawable drawable)
		{
			return new Core.Droid.Controls.DividerItemDecoration(Orientation, drawable);
		}
	}
}