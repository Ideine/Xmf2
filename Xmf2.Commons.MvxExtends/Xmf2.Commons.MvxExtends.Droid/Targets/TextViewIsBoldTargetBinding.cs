using System;
using Android.Widget;
using Android.Graphics;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class TextViewIsBoldTargetBinding : MvxAndroidTargetBinding
	{
		public TextViewIsBoldTargetBinding(TextView view) : base(view) { }

		public override Type TargetType => typeof(bool);

		protected override void SetValueImpl(object target, object value)
		{
			if (!(value is bool))
			{
				System.Diagnostics.Debug.WriteLine("Value '{0}' could not be parsed as a boolean", value);
			}

			TextView tvw = target as TextView;
			tvw?.SetTypeface(tvw.Typeface, (bool)value ? TypefaceStyle.Bold : TypefaceStyle.Normal);
		}
	}
}