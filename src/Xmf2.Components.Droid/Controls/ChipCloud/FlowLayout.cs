using System;
using Android.Content;
using Android.Runtime;
using Android.Views;

namespace Xmf2.Components.Droid.Controls.ChipCloud
{
	public enum FlowGravity
	{
		LEFT, RIGHT, CENTER, STAGGERED
	}

	public class FlowLayout : ViewGroup
	{
		private LayoutProcessor _layoutProcessor;

		public virtual int MinimumHorizontalSpacing { get; set; }

		public virtual int VerticalSpacing { get; set; }

		public virtual FlowGravity FlowGravity { get; set; } = FlowGravity.LEFT;

		private int _lineHeight;

		#region Constructors

		protected FlowLayout(IntPtr handle, JniHandleOwnership transer) : base(handle, transer) => Initialize();
		public FlowLayout(Context context) : base(context) => Initialize();
		public FlowLayout(Context context, Android.Util.IAttributeSet attrs) : base(context, attrs) => Initialize();
		public FlowLayout(Context context, Android.Util.IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) => Initialize();

		#endregion

		private void Initialize()
		{
			_layoutProcessor = new LayoutProcessor(this);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);
			if (widthMode == MeasureSpecMode.Unspecified)
			{
				return;
			}

			int width = MeasureSpec.GetSize(widthMeasureSpec) - PaddingLeft - PaddingRight;
			int height = MeasureSpec.GetSize(heightMeasureSpec) - PaddingTop - PaddingBottom;
			int count = ChildCount;
			int lineHeight = 0;

			int xPos = PaddingLeft;
			int yPos = PaddingTop;

			int childHeightMeasureSpec;
			if (MeasureSpec.GetMode(heightMeasureSpec) == MeasureSpecMode.AtMost)
			{
				childHeightMeasureSpec = MeasureSpec.MakeMeasureSpec(height, MeasureSpecMode.AtMost);
			}
			else
			{
				childHeightMeasureSpec = MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
			}

			for (int i = 0; i < count; i++)
			{
				View child = GetChildAt(i);
				if (child.Visibility != ViewStates.Gone)
				{
					child.Measure(MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.AtMost), childHeightMeasureSpec);
					int childW = child.MeasuredWidth;
					lineHeight = Math.Max(lineHeight, child.MeasuredHeight + VerticalSpacing);

					if (xPos + childW > width)
					{
						xPos = PaddingLeft;
						yPos += lineHeight;
					}

					xPos += childW + MinimumHorizontalSpacing;
				}
			}
			_lineHeight = lineHeight;

			if (MeasureSpec.GetMode(heightMeasureSpec) == MeasureSpecMode.Unspecified ||
			   (MeasureSpec.GetMode(heightMeasureSpec) == MeasureSpecMode.AtMost && yPos + lineHeight < height))
			{
				height = yPos + lineHeight;
			}
			SetMeasuredDimension(width, height);
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			int count = ChildCount;
			int width = r - l;
			int xPos = PaddingLeft;
			int yPos = PaddingTop;

			_layoutProcessor.Width = width;

			for (int i = 0; i < count; i++)
			{
				View child = GetChildAt(i);
				if (child.Visibility != ViewStates.Gone)
				{
					int childW = child.MeasuredWidth;
					int childH = child.MeasuredHeight;
					if (xPos + childW > width)
					{
						xPos = PaddingLeft;
						yPos += _lineHeight;
						_layoutProcessor.LayoutPreviousRow();
					}
					_layoutProcessor.AddViewForLayout(child, yPos, childW, childH);
					xPos += childW + MinimumHorizontalSpacing;
				}
			}
			_layoutProcessor.LayoutPreviousRow();
			_layoutProcessor.Clear();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_layoutProcessor?.Dispose();
				_layoutProcessor = null;
			}
			base.Dispose(disposing);
		}
	}
}
