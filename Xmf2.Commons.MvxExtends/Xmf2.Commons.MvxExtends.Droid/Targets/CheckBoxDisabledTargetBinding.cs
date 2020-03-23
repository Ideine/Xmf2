using System;
using Android.Widget;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;
using MvvmCross.Binding.Droid.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class CheckBoxDisabledTargetBinding : MvxAndroidTargetBinding
	{
		public CheckBoxDisabledTargetBinding(CheckBox view) : base(view) { }

		public override Type TargetType=> typeof(bool); 

		protected override void SetValueImpl(object target, object value)
		{
			if (!(value is bool))
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Value '{0}' could not be parsed as a valid bool", value);
			}

			var isDisabled = (bool)value;

			var cbx = target as CheckBox;
			if (cbx != null)
			{
				cbx.Enabled = !isDisabled;
			}
		}
	}
}