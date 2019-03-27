using Android.Graphics;
using Android.Graphics.Drawables;

namespace Xmf2.Core.Droid.Extensions
{
	public static class BackgroundExtensions
	{
		public static Drawable CreateDrawable(this object _, Color color, int? cornerRadiusInPx = 0, Color? strokeColor = null, int stokeWidthInPx = 0, int dashedWidthInPx = 0, int dashGap = 0, int? width = null, int? height = null)
		{
			var bg = new GradientDrawable();
			bg.SetShape(ShapeType.Rectangle);
			bg.SetOrientation(GradientDrawable.Orientation.LeftRight);
			bg.SetColors(new int[] { color, color });
			if (width.HasValue && height.HasValue)
			{
				bg.SetSize(width.Value, height.Value);
			}

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
	}
}
