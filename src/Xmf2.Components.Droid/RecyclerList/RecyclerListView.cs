using System;
using Xmf2.Core.Subscriptions;
using Android.Support.V7.Widget;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Droid.Interfaces;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Content.Res;
using Android.Support.V4.Content;
using Android.Graphics;
using Android.Views;
using Android.Support.V4.View;

namespace Xmf2.Components.Droid.RecyclerList
{
	public class RecyclerListView : RecyclerItemsView
	{
		public virtual int Orientation => LinearLayoutManager.Vertical;

		public RecyclerListView(IServiceLocator services, Func<IServiceLocator, IComponentView> factory) : base(services, factory) { }

		protected override void SetLayoutManager()
		{
			using (var lm = new LinearLayoutManager(Context, Orientation, reverseLayout: false).DisposeWith(Disposables))
			{
				RecyclerView.SetLayoutManager(lm);
			}
		}

		protected override void OnDesignView()
		{
			base.OnDesignView();
			SetDecorators();
		}

		protected virtual void SetDecorators() { }

		protected RecyclerView.ItemDecoration CreateDrawableSeparator(Context context, Drawable drawable)
		{
			return new DividerItemDecoration(context, Orientation, drawable);
		}

		#region Separator

		public class DividerItemDecoration : RecyclerView.ItemDecoration
		{
			private int[] ATTRS => new int[] { Android.Resource.Attribute.ListDivider };

			public const int HORIZONTAL_LIST = LinearLayoutManager.Horizontal;

			public const int VERTICAL_LIST = LinearLayoutManager.Vertical;

			private Drawable _divider;

			private int _orientation;

			public DividerItemDecoration(Context context, int orientation)
			{
				TypedArray a = context.ObtainStyledAttributes(ATTRS);
				_divider = a.GetDrawable(0);
				a.Recycle();
				SetOrientation(orientation);
			}

			public DividerItemDecoration(Context context, int orientation, int resDrawable)
			{
				_divider = ContextCompat.GetDrawable(context, resDrawable);
				SetOrientation(orientation);
			}

			public DividerItemDecoration(Context context, int orientation, Drawable drawable)
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
				if (_orientation == VERTICAL_LIST)
				{
					outRect.Set(0, 0, 0, _divider.IntrinsicHeight);
				}
				else
				{
					outRect.Set(0, 0, _divider.IntrinsicWidth, 0);
				}
			}
		}

		#endregion
	}
}

