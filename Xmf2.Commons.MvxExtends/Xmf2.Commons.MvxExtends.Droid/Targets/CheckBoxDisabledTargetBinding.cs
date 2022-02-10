using System;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class CheckBoxDisabledTargetBinding : MvxAndroidTargetBinding
	{
		public override Type TargetType => typeof(bool);

		public CheckBoxDisabledTargetBinding(CheckBox view) : base(view) { }

		protected override void SetValueImpl(object target, object value)
		{
			if (!(value is bool))
			{
				System.Diagnostics.Debug.WriteLine("Value '{0}' could not be parsed as a valid bool", value);
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