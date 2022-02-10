using System;
using Android.Widget;
using Android.Graphics;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class TextViewTextColorTargetBinding : MvxAndroidTargetBinding
	{
		public TextViewTextColorTargetBinding(TextView view) : base(view) { }

		public override Type TargetType => typeof(Color);

		protected override void SetValueImpl(object target, object value)
		{
			if (!(value is Color))
			{
				System.Diagnostics.Debug.WriteLine("Value '{0}' could not be parsed as a valid Color", value);
			}

			Color color = (Color)value;

			TextView tvw = target as TextView;
			tvw?.SetTextColor(color);
		}
	}
}