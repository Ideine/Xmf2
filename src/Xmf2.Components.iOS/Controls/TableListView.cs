﻿#if NET7_0_OR_GREATER
using System.Runtime.InteropServices;
using ObjCRuntime;
#else
using NFloat = System.nfloat;
#endif
using System;
using System.Diagnostics;
using UIKit;
using Xmf2.Components.Interfaces;
using Xmf2.Components.iOS.Interfaces;
using Xmf2.Components.iOS.TableView;
using Xmf2.Components.iOS.Views;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.iOS.Controls
{
	public class TableListView<TComponentView> : TableListView<TComponentView, IListViewState> where TComponentView : IComponentView
	{
		public TableListView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services, factory) { }
	}

	public class TableListView<TComponentView, TViewState> : BaseComponentView<TViewState>
		where TComponentView : IComponentView
		where TViewState : class, IListViewState
	{
		protected UITableView ListView;

		private TableViewItemSource<TableViewItemCell> _source;
		private Func<IServiceLocator, IComponentView> _factory;

		public virtual NFloat EstimatedRowItemHeight
		{
			get => ListView.EstimatedRowHeight;
			set => ListView.EstimatedRowHeight = value;
		}

		protected virtual UIColor BackgroundColor { get; } = UIColor.White;

		public virtual UIEdgeInsets ContentInset
		{
			get => ListView.ContentInset;
			set => ListView.ContentInset = value;
		}

		public TableListView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services)
		{
			ListView = new UITableView
			{
				SeparatorStyle = UITableViewCellSeparatorStyle.None,
				AllowsSelection = false,
				ContentInset = UIEdgeInsets.Zero,
				BackgroundColor = BackgroundColor,
				Bounces = true,
				AlwaysBounceVertical = true
			}.DisposeViewWith(Disposables);

			_source = CreateSource(ListView);
			ListView.RegisterClassForCellReuse(typeof(TableViewItemCell), TableViewItemCell.NsCellIdentifier);
			ListView.Source = _source;

			if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
			{
				ListView.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
			}

			_factory = factory;
		}

		protected virtual TableViewItemSource<TableViewItemCell> CreateSource(UITableView listView)
		{
			return new TableViewItemSource<TableViewItemCell>(listView, () => _factory(Services), TableViewItemCell.NsCellIdentifier).DisposeWith(Disposables);
		}

		protected override UIView RenderView()
		{
			return ListView;
		}

		protected override void OnStateUpdate(TViewState state)
		{
			try
			{
				base.OnStateUpdate(state);
				_source.ItemSource = state.Items;
			}
			catch (Exception)
			{
				Debugger.Break();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ListView = null;
				_source = null;
				_factory = null;
			}

			base.Dispose(disposing);
		}
	}
}