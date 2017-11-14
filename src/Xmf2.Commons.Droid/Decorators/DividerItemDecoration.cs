using System;
namespace Xmf2.Commons.Droid.Decorators
{
	public class DividerItemDecoration : RecyclerView.ItemDecoration
	{
		public enum Orientation
		{
			Horizontal,
			Vertical,
		}
		private int[] ATTRS => new int[] { Android.Resource.Attribute.ListDivider };

		private Drawable _divider;

		private Orientation _orientation;

		public DividerItemDecoration(Context context, Orientation orientation)
		{
			TypedArray a = context.ObtainStyledAttributes(ATTRS);
			_divider = a.GetDrawable(0);
			a.Recycle();
			_orientation = orientation;
		}

		public DividerItemDecoration(Context context, Orientation orientation, int resDrawable)
		{
			_divider = ContextCompat.GetDrawable(context, resDrawable);
			_orientation = orientation;
		}

		protected DividerItemDecoration(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override void OnDraw(Canvas c, RecyclerView parent, RecyclerView.State state)
		{
			base.OnDraw(c, parent, state);
			if (_orientation == Orientation.Vertical)
			{
				DrawVertical(c, parent);
			}
			else
			{
				DrawHorizontal(c, parent);
			}
		}

		public void DrawVertical(Canvas c, RecyclerView parent)
		{
			int left = parent.PaddingLeft;
			int right = parent.Width - parent.PaddingRight;
			int childCount = parent.ChildCount;

			for (int i = 0; i < childCount; i++)
			{
				View child = parent.GetChildAt(i);
				RecyclerView.LayoutParams param = (RecyclerView.LayoutParams)child.LayoutParameters;
				int top = (int)(child.Bottom + param.BottomMargin + Math.Round(ViewCompat.GetTranslationX(child)));
				int bottom = top + _divider.IntrinsicHeight;
				_divider.SetBounds(left, top, right, bottom);
				_divider.Draw(c);
			}
		}

		public void DrawHorizontal(Canvas c, RecyclerView parent)
		{
			int top = parent.PaddingTop;
			int bottom = parent.Height - parent.PaddingBottom;
			int childCount = parent.ChildCount;
			for (int i = 0; i < childCount; i++)
			{
				View child = parent.GetChildAt(i);
				RecyclerView.LayoutParams param = (RecyclerView.LayoutParams)child.LayoutParameters;
				int left = (int)(child.Right + param.RightMargin + Math.Round(ViewCompat.GetTranslationX(child)));
				int right = left + _divider.IntrinsicHeight;
				_divider.SetBounds(left, top, right, bottom);
				_divider.Draw(c);
			}
		}

		public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
		{
			base.GetItemOffsets(outRect, view, parent, state);
			if (_orientation == Orientation.Vertical)
			{
				outRect.Set(0, 0, 0, _divider.IntrinsicHeight);
			}
			else
			{
				outRect.Set(0, 0, _divider.IntrinsicWidth, 0);
			}
		}
	}
}
