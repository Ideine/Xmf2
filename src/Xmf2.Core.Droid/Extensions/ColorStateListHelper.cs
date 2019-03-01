using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;

namespace Xmf2.Core.Droid.Extensions
{
	public static class ColorStateListHelper
	{
		public static void UseBackgroundAndHighlight(this View view, Color backgroundColor, Color highlightColor)
		{
			var st = new StateListDrawable();

			st.AddState(new int[] { Android.Resource.Attribute.StatePressed }, new ColorDrawable(highlightColor));
			st.AddState(new int[] { }, new ColorDrawable(backgroundColor));

			view.Background = st;
		}
	}
}
