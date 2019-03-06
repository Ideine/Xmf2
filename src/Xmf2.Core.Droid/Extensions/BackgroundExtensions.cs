using Android.Views;
using Android.Graphics;
using Xmf2.Core.Droid.Helpers;
using Android.Graphics.Drawables;

namespace Xmf2.Core.Droid.Extensions
{
	public static class BackgroundExtensions
	{
		public static Drawable CreateDrawable(this object _, Color color, int? cornerRadiusInPx = 0, Color? strokeColor = null, int stokeWidthInPx = 0, int dashedWidthInPx = 0, int dashGap = 0)
		{
			var bg = new GradientDrawable();
			bg.SetShape(ShapeType.Rectangle);
			bg.SetOrientation(GradientDrawable.Orientation.LeftRight);
			bg.SetColors(new int[] { color, color });
			if (cornerRadiusInPx.HasValue)
			{
				bg.SetCornerRadius(cornerRadiusInPx.Value);
			}
			if (strokeColor.HasValue)
			{
				bg.SetStroke(stokeWidthInPx, strokeColor.Value, dashedWidthInPx, dashGap);
			}
			return bg;
		}

		public static void SetGradientBackground(this View view, Color leftColor, Color righColor, int? cornerRadius = null, Color? stokeColor = null, int stokeWidth = 0)
		{
			using (var bg = new GradientDrawable())
			{
				bg.SetShape(ShapeType.Rectangle);
				bg.SetOrientation(GradientDrawable.Orientation.TopBottom);
				bg.SetColors(new int[] { leftColor, righColor });
				if (cornerRadius.HasValue)
				{
					bg.SetCornerRadius(UIHelper.DpToPx(view.Context, cornerRadius.Value));
				}

				if (stokeColor.HasValue)
				{
					bg.SetStroke(UIHelper.DpToPx(view.Context, stokeWidth), stokeColor.Value);
				}
				view.Background = bg;
			}
		}

	}
}
