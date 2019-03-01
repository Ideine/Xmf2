using Android.Views;

namespace Xmf2.Core.Droid.Extensions
{
	public static class ConverterExtensions
	{
		public static ViewStates ToVisibility(this bool v)
		{
			return v ? ViewStates.Visible : ViewStates.Gone;
		}
	}
}