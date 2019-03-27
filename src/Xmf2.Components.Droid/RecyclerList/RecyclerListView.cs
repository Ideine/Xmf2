using System;
using Xmf2.Core.Subscriptions;
using Android.Support.V7.Widget;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Droid.Interfaces;

namespace Xmf2.Components.Droid.RecyclerList
{
	public class RecyclerListView : RecyclerItemsView
	{
		public virtual int Orientation => LinearLayoutManager.Vertical;

		public RecyclerListView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services, factory) { }

		protected override void SetLayoutManager()
		{
			using (var lm = new LinearLayoutManager(Context, Orientation, reverseLayout: false).DisposeWith(Disposables))
			{
				RecyclerView.SetLayoutManager(lm);
			}
		}
	}
}
