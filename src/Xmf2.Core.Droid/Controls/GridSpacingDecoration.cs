using Android.Graphics;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace Xmf2.Core.Droid.Controls
{
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
}