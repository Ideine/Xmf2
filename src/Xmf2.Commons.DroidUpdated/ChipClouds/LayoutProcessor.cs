using System.Collections.Generic;
using Android.Views;

namespace Xmf2.Commons.Droid.ChipClouds
{
    public class LayoutProcessor
	{
		private readonly FlowLayout _flowLayout;

		private readonly List<View> _viewsInCurrentRow;
		private readonly List<int> _viewWidths;
		private readonly List<int> _viewHeights;

		public int Width { get; set; }

		private int _rowY;

		public LayoutProcessor(FlowLayout flowLayout)
		{
			_flowLayout = flowLayout;
			_viewsInCurrentRow = new List<View>();
			_viewWidths = new List<int>();
			_viewHeights = new List<int>();
		}

		public void AddViewForLayout(View view, int yPos, int childW, int childH)
		{
			_rowY = yPos;
			_viewsInCurrentRow.Add(view);
			_viewWidths.Add(childW);
			_viewHeights.Add(childH);
		}

		public void Clear()
		{
			_viewsInCurrentRow.Clear();
			_viewWidths.Clear();
			_viewHeights.Clear();
		}

		public void LayoutPreviousRow()
		{
			FlowGravity gravity = _flowLayout.FlowGravity;
			int minimumHorizontalSpacing = _flowLayout.MinimumHorizontalSpacing;
			switch (gravity)
			{
				case FlowGravity.Left:
					int xPos = _flowLayout.PaddingLeft;
					for (int i = 0; i < _viewsInCurrentRow.Count; i++)
					{
						_viewsInCurrentRow[i].Layout(xPos, _rowY, xPos + _viewWidths[i], _rowY + _viewHeights[i]);
						xPos += _viewWidths[i] + minimumHorizontalSpacing;
					}
					break;
				case FlowGravity.Right:
					int xEnd = Width - _flowLayout.PaddingRight;
					for (int i = _viewsInCurrentRow.Count - 1; i >= 0; i--)
					{
						int xStart = xEnd - _viewWidths[i];
						_viewsInCurrentRow[i].Layout(xStart, _rowY, xEnd, _rowY + _viewHeights[i]);
						xEnd = xStart - minimumHorizontalSpacing;
					}
					break;
				case FlowGravity.Staggered:
					int totalWidthOfChildren = 0;
					foreach (int childViewWidth in _viewWidths)
					{
						totalWidthOfChildren += childViewWidth;
					}
					int horizontalSpacingForStaggered = (Width - totalWidthOfChildren - _flowLayout.PaddingLeft
														 - _flowLayout.PaddingRight) / (_viewsInCurrentRow.Count + 1);
					xPos = _flowLayout.PaddingLeft + horizontalSpacingForStaggered;
					for (int i = 0; i < _viewsInCurrentRow.Count; i++)
					{
						_viewsInCurrentRow[i].Layout(xPos, _rowY, xPos + _viewWidths[i], _rowY + _viewHeights[i]);
						xPos += _viewWidths[i] + horizontalSpacingForStaggered;
					}
					break;
				case FlowGravity.Center:
					totalWidthOfChildren = 0;
					foreach (int childViewWidth in _viewWidths)
					{
						totalWidthOfChildren += childViewWidth;
					}
					xPos = _flowLayout.PaddingLeft + (Width - _flowLayout.PaddingLeft - _flowLayout.PaddingRight -
													  totalWidthOfChildren - (minimumHorizontalSpacing * (_viewsInCurrentRow.Count - 1))) / 2;
					for (int i = 0; i < _viewsInCurrentRow.Count; i++)
					{
						_viewsInCurrentRow[i].Layout(xPos, _rowY, xPos + _viewWidths[i], _rowY + _viewHeights[i]);
						xPos += _viewWidths[i] + minimumHorizontalSpacing;
					}
					break;
			}
			Clear();
		}
	}
}
