using Android.Views;
using Xmf2.Components.Droid.Interfaces;

namespace Xmf2.Components.Droid.Services
{
	public class LayoutInflaterResolver : ILayoutInflaterResolver
	{
		public LayoutInflater Inflater() => LayoutInflater.From(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity);
	}
}