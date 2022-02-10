using System;
using Android.Widget;
using Android.Graphics;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class ButtonTextColorTargetBinding : MvxAndroidTargetBinding
	{
		public ButtonTextColorTargetBinding(Button view) : base(view) { }

		public override Type TargetType => typeof(Color);

		protected override void SetValueImpl(object target, object value)
		{
			if (!(value is Color))
			{
				System.Diagnostics.Debug.WriteLine("Value '{0}' could not be parsed as a valid Color", value);
			}

			Color color = (Color)value;

			Button btn = target as Button;
			btn?.SetTextColor(color);
		}
	}
}