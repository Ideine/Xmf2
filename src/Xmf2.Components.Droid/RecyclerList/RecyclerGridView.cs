using System;
using Android.Views;
using Android.Graphics;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;
using Android.Support.V7.Widget;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Droid.Interfaces;

namespace Xmf2.Components.Droid.RecyclerList
{
	public abstract class RecyclerGridView : RecyclerItemsView
	{
		protected virtual int Columns => 2;

		//Space
		protected virtual int TopSpacing => 0;
		protected virtual int LeftSpacing => 0;
		protected virtual int RightSpacing => 0;
		protected virtual int BottomSpacing => 0;

		private Func<IServiceLocator, IComponentView> _factory;

		public RecyclerGridView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services, factory) { }

		protected override void SetLayoutManager()
		{
			using (var lm = new GridLayoutManager(Context, 1, GridLayoutManager.Vertical, reverseLayout: false).DisposeWith(Disposables))
			{
				lm.SpanCount = Columns;
				RecyclerView.SetLayoutManager(lm);
			}
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

		#region Decorator

		public class GridSpacingDecoration : RecyclerView.ItemDecoration
		{
			private readonly int _spanCount;

			private readonly int _botSpacing;
			private readonly int _leftSpacing;
			private readonly int _topSpacing;
			private readonly int _rightSpacing;

			private readonly bool _includeEdges;

			public GridSpacingDecoration(int spanCount, int leftSpacing, int topSpacing, int botSpacing, int rightSpacing, bool includeEdges = false)
			{
				_spanCount = spanCount;
				_leftSpacing = leftSpacing;
				_topSpacing = topSpacing;
				_botSpacing = botSpacing;
				_rightSpacing = rightSpacing;
				_includeEdges = includeEdges;
			}

			public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
			{
				var position = parent.GetChildAdapterPosition(view);
				var column = position % _spanCount;

				if (_includeEdges)
				{
					outRect.Left = _leftSpacing - column * _leftSpacing / _spanCount;
					outRect.Right = (column + 1) * _rightSpacing / _spanCount;
					if (position < _spanCount)
					{
						outRect.Top = _topSpacing;
					}
					outRect.Bottom = _botSpacing;
				}
				else
				{
					outRect.Left = column * _leftSpacing / _spanCount;
					outRect.Right = _rightSpacing - (column - 1) * _rightSpacing / _spanCount;
					if (position >= _spanCount)
					{
						outRect.Top = _topSpacing;
					}
				}
			}
		}

		#endregion

	}
}
