using System;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;
#if __ANDROID_29__
using AndroidX.RecyclerView.Widget;
#else
using Android.Support.V7.Widget;
#endif
using Xmf2.Components.Interfaces;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Core.Droid.Controls;

namespace Xmf2.Components.Droid.List
{
	public abstract class RecyclerGridView : RecyclerItemsView
	{
		protected virtual int Columns => 2;

		//Space
		protected virtual int TopSpacing => 0;
		protected virtual int LeftSpacing => 0;
		protected virtual int RightSpacing => 0;
		protected virtual int BottomSpacing => 0;

		public RecyclerGridView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services, factory) { }

		protected override void SetLayoutManager()
		{
			using var lm = new GridLayoutManager(Context, 1, LinearLayoutManager.Vertical, reverseLayout: false)
			{
				SpanCount = Columns
			};
			RecyclerView.SetLayoutManager(lm);
		}

		protected override void OnDesignView()
		{
			RecyclerView.AddItemDecoration(new GridSpacingDecoration(
				spanCount: Columns,
				leftSpacing: UIHelper.DpToPx(Context, LeftSpacing),
				topSpacing: UIHelper.DpToPx(Context, TopSpacing),
				botSpacing: UIHelper.DpToPx(Context, BottomSpacing),
				rightSpacing: UIHelper.DpToPx(Context, RightSpacing)
			).DisposeWith(Disposables));
		}
	}
}