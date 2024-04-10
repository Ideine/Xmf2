using Android.Graphics;
using Android.Views;

namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public interface IRevealValues
	{
		float Percentage { get; set; }

		bool IsClipping { get; set; }

		View Target { get; }

		bool ApplyTransformation(Canvas canvas, View child);
	}
}