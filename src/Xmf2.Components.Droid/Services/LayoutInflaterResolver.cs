using Android.App;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;
#if NET7_0_OR_GREATER
using Microsoft.Maui.ApplicationModel;

#else
using Plugin.CurrentActivity;
#endif

namespace Xmf2.Components.Droid.Services
{
	public class LayoutInflaterResolver : ILayoutInflaterResolver
	{
		public LayoutInflater Inflater()
		{
#if NET7_0_OR_GREATER
			Activity activity = Platform.CurrentActivity;
#else
			var activity = CrossCurrentActivity.Current.Activity;
#endif
			return LayoutInflater.From(activity);
		}
	}
}