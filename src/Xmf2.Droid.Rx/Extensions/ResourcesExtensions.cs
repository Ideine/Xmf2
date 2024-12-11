using Android.Content;
using Android.Graphics;
using AndroidX.Core.Content;

namespace Xmf2.Droid.Rx.Extensions
{
	public static class ResourcesExtensions
	{
		public static Color LoadColor(this Context ctx, int id) => new(ContextCompat.GetColor(ctx, id));
	}
}