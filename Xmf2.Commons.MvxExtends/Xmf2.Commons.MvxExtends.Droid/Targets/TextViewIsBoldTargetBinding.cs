using Android.Graphics;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class TextViewIsBoldTargetBinding : MvxAndroidTargetBinding<TextView, bool>
	{
		public TextViewIsBoldTargetBinding(TextView view) : base(view) { }

		protected override void SetValueImpl(TextView target, bool value)
		{
			if (value)
			{
				target.SetTypeface(target.Typeface, TypefaceStyle.Bold);
			}
			else
			{
				target.SetTypeface(target.Typeface, TypefaceStyle.Normal);
			}
		}
	}
}