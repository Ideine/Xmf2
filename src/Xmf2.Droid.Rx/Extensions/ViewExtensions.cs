using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;

namespace Xmf2.Droid.Rx.Extensions
{
	public static class ViewExtensions
	{
		public static void DisposeImage(this ImageView imageView)
		{
			imageView?.RecycleImage();
			imageView?.Dispose();
		}

		public static void RecycleImage(this ImageView imageView)
		{
			if (imageView?.Drawable is BitmapDrawable bd && (!bd.Bitmap?.IsRecycled ?? true))
			{
				bd.Bitmap.Recycle();
			}
		}

		public static void DisposeBackgroundImage(this View view)
		{
			if (view?.Background is { } d)
			{
				d.Dispose();
			}
		}

		public static IEnumerable<View> GetAllChilds(this LinearLayout layout)
		{
			return Enumerable.Range(0, layout.ChildCount).Select(layout.GetChildAt);
		}

		public static int GetDistanceTopToTop<TView>(this View view)
		{
			if (view is TView)
			{
				return 0;
			}
			else if (view?.Parent is View parent)
			{
				return GetDistanceTopToTop<TView>(parent) + view.Top;
			}

			return 0;
		}

		public static Drawable CreateDrawable(this object _, Color color, int? cornerRadiusInPx = 0, Color? strokeColor = null, int strokeWidthInPx = 0, bool dashed = false, int dashedWidthInPx = 0, int dashGap = 0)
		{
			var bg = new GradientDrawable();
			bg.SetShape(ShapeType.Rectangle);
			bg.SetOrientation(GradientDrawable.Orientation.LeftRight);
			bg.SetColors(new int[]
			{
				color,
				color
			});
			if (cornerRadiusInPx.HasValue)
			{
				bg.SetCornerRadius(cornerRadiusInPx.Value);
			}

			if (strokeColor.HasValue)
			{
				bg.SetStroke(strokeWidthInPx, strokeColor.Value, dashedWidthInPx, dashGap);
			}

			return bg;
		}
	}
}