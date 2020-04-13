using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
#if __ANDROID_29__
using AndroidX.Core.Content;
using AndroidX.Core.View;
using AndroidX.RecyclerView.Widget;
#else
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.Widget;
#endif
using Android.Views;

namespace Xmf2.Core.Droid.Controls
{
	public class DividerItemDecoration : RecyclerView.ItemDecoration
	{
		private int[] ATTRS => new[]
		{
			Android.Resource.Attribute.ListDivider
		};

		public const int HORIZONTAL_LIST = LinearLayoutManager.Horizontal;

		public const int VERTICAL_LIST = LinearLayoutManager.Vertical;

		private Drawable _divider;

		private int _orientation;

		public DividerItemDecoration(Context context, int orientation)
		{
			using TypedArray a = context.ObtainStyledAttributes(ATTRS);
			_divider = a.GetDrawable(0);
			SetOrientation(orientation);
		}

		public DividerItemDecoration(Context context, int orientation, int resDrawable)
		{
			_divider = ContextCompat.GetDrawable(context, resDrawable);
			SetOrientation(orientation);
		}

		public DividerItemDecoration(int orientation, Drawable drawable)
		{
			_divider = drawable;
			SetOrientation(orientation);
		}

		protected DividerItemDecoration(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public void SetOrientation(int orientation)
		{
			if (orientation != HORIZONTAL_LIST && orientation != VERTICAL_LIST)
			{
				throw new ArgumentException("invalid orientation");
			}

			_orientation = orientation;
		}

		public override void OnDraw(Canvas c, RecyclerView parent, RecyclerView.State state)
		{
			base.OnDraw(c, parent, state);
			if (_orientation == VERTICAL_LIST)
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

			for (int i = 0 ; i < childCount ; i++)
			{
				View child = parent.GetChildAt(i);
				var param = (RecyclerView.LayoutParams)child.LayoutParameters;
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
			for (int i = 0 ; i < childCount ; i++)
			{
				View child = parent.GetChildAt(i);
				var param = (RecyclerView.LayoutParams)child.LayoutParameters;
				int left = (int)(child.Right + param.RightMargin + Math.Round(ViewCompat.GetTranslationX(child)));
				int right = left + _divider.IntrinsicHeight;
				_divider.SetBounds(left, top, right, bottom);
				_divider.Draw(c);
			}
		}

		public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
		{
			base.GetItemOffsets(outRect, view, parent, state);
			if (_orientation == VERTICAL_LIST)
			{
				outRect.Set(0, 0, 0, _divider.IntrinsicHeight);
			}
			else
			{
				outRect.Set(0, 0, _divider.IntrinsicWidth, 0);
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_divider?.Dispose();
				_divider = null;
			}

			base.Dispose(disposing);
		}
	}
}