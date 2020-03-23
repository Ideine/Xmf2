using System;
using Android.Text;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Droid.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class TextViewSpannedTextTargetBinding : MvxAndroidTargetBinding
	{
		public TextViewSpannedTextTargetBinding(TextView view) : base(view) { }

		public override Type TargetType => typeof(ISpanned);

		protected override void SetValueImpl(object target, object value)
		{
			if (value != null && !(value is ISpanned))
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value '{0}' could not be parsed as a ISpanned", value);
			}

			TextView tvw = target as TextView;
			if (tvw != null)
			{
				tvw.SetText((ISpanned)value, TextView.BufferType.Spannable);
			}
		}

	}
}