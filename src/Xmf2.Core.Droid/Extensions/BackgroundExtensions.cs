using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using Xmf2.Common.Extensions;
using Xmf2.Core.Droid.Helpers;

namespace Xmf2.Core.Droid.Extensions
{
	public static class BackgroundExtensions
	{
		public static void SetRoundedCornersBackground(this View view, Context context, Color backgroundColor, float radius)
		{
			int radiusInPx = UIHelper.DpToPx(context, radius);
			view.SetXmf2Background(new Dictionary<int[], (Color bgColor, (Color color, int width)? stroke)>
			{
				[Array.Empty<int>()] = (backgroundColor, null)
			}, radiusInPx);
		}

		public static void SetRoundedCornersBackground(this View view, Context context, Color backgroundColor, float topLeftRadius, float topRightRadius, float bottomRightRadius, float bottomLeftRadius)
		{
			int topLeft = UIHelper.DpToPx(context, topLeftRadius);
			int topRight = UIHelper.DpToPx(context, topRightRadius);
			int bottomRight = UIHelper.DpToPx(context, bottomRightRadius);
			int bottomLeft = UIHelper.DpToPx(context, bottomLeftRadius);

			view.SetXmf2Background(new Dictionary<int[], (Color bgColor, (Color color, int width)? stroke)>
			{
				[Array.Empty<int>()] = (backgroundColor, null)
			}, new float[]
			{
				topLeft,
				topLeft,
				topRight,
				topRight,
				bottomRight,
				bottomRight,
				bottomLeft,
				bottomLeft
			});
		}

		public static void SetRoundedCornersBackgroundWithStroke(this View view, Context context, Color backgroundColor, Color strokeColor, float strokeWidth, float radius)
		{
			int radiusInPx = UIHelper.DpToPx(context, radius);
			int strokeWidthInPx = UIHelper.DpToPx(context, strokeWidth);

			view.SetXmf2Background(new Dictionary<int[], (Color bgColor, (Color color, int width)? stroke)>
			{
				[Array.Empty<int>()] = (backgroundColor, (strokeColor, strokeWidthInPx))
			}, radiusInPx);
		}

		public static void SetRoundedCornersBackgroundWithHighlight(this View view, Context context, Color backgroundColor, Color highlightColor, float radius)
		{
			int radiusInPx = UIHelper.DpToPx(context, radius);
			view.SetXmf2Background(new Dictionary<int[], (Color bgColor, (Color color, int width)? stroke)>
			{
				[Android.Resource.Attribute.StatePressed.WrapInArray()] = (highlightColor, null),
				[Array.Empty<int>()] = (backgroundColor, null)
			}, radiusInPx);
		}

		public static void SetBackgroundWithStrokeAndHighlight(this View view, Context context, Color backgroundColor, Color strokeColor, float strokeWidth, Color highlightColor, Color highlightStrokeColor, float highlightStrokeWidth, float radius = 0)
		{
			int radiusInPx = UIHelper.DpToPx(context, radius);
			int strokeWidthInPx = UIHelper.DpToPx(context, strokeWidth);
			int highlightStrokeWidthInPx = UIHelper.DpToPx(context, highlightStrokeWidth);
			view.SetXmf2Background(new Dictionary<int[], (Color bgColor, (Color color, int width)? stroke)>
			{
				[Android.Resource.Attribute.StatePressed.WrapInArray()] = (highlightColor, (highlightStrokeColor, highlightStrokeWidthInPx)),
				[Array.Empty<int>()] = (backgroundColor, (strokeColor, strokeWidthInPx))
			}, radiusInPx);
		}

		public static void SetBackgroundWithHighlightAndSelected(this View view, Color backgroundColor, Color selectedColor, Color highlightColor)
		{
			view.SetXmf2Background(new Dictionary<int[], (Color bgColor, (Color color, int width)? stroke)>
			{
				[Android.Resource.Attribute.StatePressed.WrapInArray()] = (highlightColor, null),
				[Android.Resource.Attribute.StateSelected.WrapInArray()] = (selectedColor, null),
				[Array.Empty<int>()] = (backgroundColor, null),
			});
		}

		#region SetXmf2Background

		/// <summary>
		/// stateList's keys will be keys of StateListDrawable if there is more than 1 key
		/// <see cref="StateListDrawable"/>
		/// </summary>
		/// <param name="view"></param>
		/// <param name="stateList"></param>
		/// <param name="cornerRadii">must be in px</param>
		public static void SetXmf2Background(this View view, Dictionary<int[], (Color bgColor, (Color color, int width)? stroke)> stateList, float[] cornerRadii)
		{
			if (stateList.Count is 0)
			{
				return;
			}

			if (stateList.Count is not 1)
			{
				List<Drawable> drawableToDispose = new(stateList.Count);
				using StateListDrawable st = new();

				foreach (KeyValuePair<int[], (Color bgColor, (Color color, int width)? stroke)> kvp in stateList)
				{
					Drawable background = CreateDrawable(view, kvp.Value.bgColor, cornerRadii: cornerRadii, strokeColor: kvp.Value.stroke?.color, strokeWidthInPx: kvp.Value.stroke?.width ?? 0);
					drawableToDispose.Add(background);
					st.AddState(kvp.Key, background);
				}

				view.Background = st;

				foreach (Drawable drawable in drawableToDispose)
				{
					drawable.Dispose();
				}

				drawableToDispose.Clear();
			}
			else
			{
				(Color bgColor, (Color color, int width)? stroke) = stateList.First().Value;
				using Drawable background = CreateDrawable(view, bgColor, cornerRadii: cornerRadii, strokeColor: stroke?.color, strokeWidthInPx: stroke?.width ?? 0);
				view.Background = background;
			}
		}

		/// <summary>
		/// stateList's keys will be keys of StateListDrawable if there is more than 1 key
		/// <see cref="StateListDrawable"/>
		/// </summary>
		/// <param name="view"></param>
		/// <param name="stateList"></param>
		/// <param name="cornerRadius">must be in px</param>
		public static void SetXmf2Background(this View view, Dictionary<int[], (Color bgColor, (Color color, int width)? stroke)> stateList, float cornerRadius = 0)
		{
			if (stateList.Count is 0)
			{
				return;
			}

			if (stateList.Count is not 1)
			{
				List<Drawable> drawableToDispose = new(stateList.Count);
				using StateListDrawable st = new();

				foreach (KeyValuePair<int[], (Color bgColor, (Color color, int width)? stroke)> kvp in stateList)
				{
					Drawable background = CreateDrawable(view, kvp.Value.bgColor, cornerRadiusInPx: cornerRadius, strokeColor: kvp.Value.stroke?.color, strokeWidthInPx: kvp.Value.stroke?.width ?? 0);
					drawableToDispose.Add(background);
					st.AddState(kvp.Key, background);
				}

				view.Background = st;

				foreach (Drawable drawable in drawableToDispose)
				{
					drawable.Dispose();
				}

				drawableToDispose.Clear();
			}
			else
			{
				(Color bgColor, (Color color, int width)? stroke) = stateList.First().Value;
				using Drawable background = CreateDrawable(view, bgColor, cornerRadiusInPx: cornerRadius, strokeColor: stroke?.color, strokeWidthInPx: stroke?.width ?? 0);
				view.Background = background;
			}
		}

		#endregion

		#region CreateDrawable

		public static GradientDrawable CreateDrawable(this object _, Color color, float? cornerRadiusInPx = 0, Color? strokeColor = null, int strokeWidthInPx = 0, int dashedWidthInPx = 0, int dashGap = 0, int width = 0, int height = 0)
		{
			GradientDrawable bg = new();
			bg.SetColor(color);
			if (width != 0 || height != 0)
			{
				bg.SetSize(width, height);
			}

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

		public static GradientDrawable CreateDrawable(this object _, Color color, float[] cornerRadii, Color? strokeColor = null, int strokeWidthInPx = 0, int dashedWidthInPx = 0, int dashGap = 0, int width = 0, int height = 0)
		{
			GradientDrawable bg = new();
			bg.SetColor(color);
			if (width != 0 || height != 0)
			{
				bg.SetSize(width, height);
			}

			if (cornerRadii != null)
			{
				bg.SetCornerRadii(cornerRadii);
			}

			if (strokeColor.HasValue)
			{
				bg.SetStroke(strokeWidthInPx, strokeColor.Value, dashedWidthInPx, dashGap);
			}

			return bg;
		}

		#endregion
	}
}