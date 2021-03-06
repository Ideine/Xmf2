﻿using System.Linq;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Droid.Views;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.LinearList
{
	public abstract class LinearListView<TCellComponent> : LinearListView<TCellComponent, IListViewState>
	{
		protected LinearListView(IServiceLocator services) : base(services) { }
	}

	public abstract class LinearListView<TCellComponent, TViewState> : BaseComponentView<TViewState> where TViewState : class, IListViewState
	{
		protected AndroidLinearListView LinearList;
		protected LinearListAdapter Adapter;
		protected virtual Color BackgroundColor => Color.White;
		protected virtual Orientation Orientation => Orientation.Vertical;

		protected LinearListView(IServiceLocator services) : base(services) { }

		protected abstract IComponentView Factory(string itemId);

		protected override View RenderView()
		{
			LinearList = new AndroidLinearListView(Context).DisposeViewWith(Disposables);
			LinearList.Orientation = Orientation;
			LinearList.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent).DisposeWith(Disposables);
			LinearList.SetBackgroundColor(BackgroundColor);

			Adapter = CreateAdapter().DisposeWith(Disposables);
			LinearList.Adapter = Adapter;

			return LinearList;
		}

		protected virtual LinearListAdapter CreateAdapter() => new LinearListAdapter(Context, Factory);

		protected override void OnStateUpdate(TViewState state)
		{
			if (CurrentState == null || !state.Items.SequenceEqual(CurrentState.Items))
			{
				base.OnStateUpdate(state);
				Adapter.ItemSource = state.Items;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				LinearList = null;
				Adapter = null;
			}
			base.Dispose(disposing);
		}

		protected virtual View CreateSeparatorView()
		{
			int eightDp = UIHelper.DpToPx(Context, 8);
			int sixteenDp = UIHelper.DpToPx(Context, 16);

			var layout = new FrameLayout(Context);
			var layoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
			layout.LayoutParameters = layoutParams;
			layout.SetBackgroundColor(Color.Transparent);
			layout.SetPadding(0, eightDp, 0, eightDp);

			var separator = new View(Context);
			var separatorLayoutParams = new ViewGroup.MarginLayoutParams(ViewGroup.LayoutParams.MatchParent, UIHelper.DpToPx(Context, 1))
			{
				MarginStart = sixteenDp,
				MarginEnd = eightDp
			};
			separator.LayoutParameters = separatorLayoutParams;
			separator.SetBackgroundColor(Color.Black);

			layout.AddView(separator);
			return layout;
		}
	}
}
