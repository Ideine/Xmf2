using System;
using Android.Widget;
using Android.Graphics;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Droid.Target;

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
				MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value '{0}' could not be parsed as a valid Color", value);
			}

			Color color = (Color)value;

			TextView tvw = target as TextView;
			if (tvw != null)
			{
				tvw.SetTextColor(color);
			}
		}
	}
}