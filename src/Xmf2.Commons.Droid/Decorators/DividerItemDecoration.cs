using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
using Android.Views;

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

		private readonly Drawable _divider;

		private readonly Orientation _orientation;

		public DividerItemDecoration(Context context, Orientation orientation)
		{
			var a = context.ObtainStyledAttributes(ATTRS);
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

		private void DrawVertical(Canvas c, RecyclerView parent)
		{
			var left = parent.PaddingLeft;
			var right = parent.Width - parent.PaddingRight;
			var childCount = parent.ChildCount;

			for (var i = 0; i < childCount; i++)
			{
				var child = parent.GetChildAt(i);
				var param = (RecyclerView.LayoutParams)child.LayoutParameters;
				var top = (int)(child.Bottom + param.BottomMargin + Math.Round(ViewCompat.GetTranslationX(child)));
				var bottom = top + _divider.IntrinsicHeight;
				_divider.SetBounds(left, top, right, bottom);
				_divider.Draw(c);
			}
		}

		private void DrawHorizontal(Canvas c, RecyclerView parent)
		{
			var top = parent.PaddingTop;
			var bottom = parent.Height - parent.PaddingBottom;
			var childCount = parent.ChildCount;
			for (var i = 0; i < childCount; i++)
			{
				var child = parent.GetChildAt(i);
				var param = (RecyclerView.LayoutParams)child.LayoutParameters;
				var left = (int)(child.Right + param.RightMargin + Math.Round(ViewCompat.GetTranslationX(child)));
				var right = left + _divider.IntrinsicHeight;
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
