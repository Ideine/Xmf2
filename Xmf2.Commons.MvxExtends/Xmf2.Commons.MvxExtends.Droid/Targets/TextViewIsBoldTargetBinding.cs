using Android.Graphics;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets;

public class TextViewIsBoldTargetBinding : MvxAndroidTargetBinding<TextView, bool>
{
	public TextViewIsBoldTargetBinding(TextView view) : base(view) { }

	protected override void SetValueImpl(TextView target, bool value)
	{
		target.SetTypeface(target.Typeface, value ? TypefaceStyle.Bold : TypefaceStyle.Normal);
	}
}