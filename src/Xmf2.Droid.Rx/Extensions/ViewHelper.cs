using Android.Views;

namespace Xmf2.Droid.Rx.Extensions
{
	public static class ViewHelper
	{
		public static void ViewStateVisibleOrGone(this View view, bool visible)
		{
			view.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
		}

		public static void ViewStateVisibleOrInvisible(this View view, bool visible)
		{
			view.Visibility = visible ? ViewStates.Visible : ViewStates.Invisible;
		}
	}
}
