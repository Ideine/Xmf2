using System;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
    public class CheckBoxDisabledTargetBinding : MvxAndroidTargetBinding
    {
        public CheckBoxDisabledTargetBinding(CheckBox view)
            : base(view)
        {

        }

        protected override void SetValueImpl(object target, object value)
        {
            if (!(value is bool))
            {
                System.Diagnostics.Debug.Write($"Value '{value}' could not be parsed as a valid bool");
            }

            var isDisabled = (bool)value;

            var cbx = target as CheckBox;
            if (cbx != null)
            {
                cbx.Enabled = !isDisabled;
            }
        }

        public override Type TargetType
        {
            get { return typeof(bool); }
        }
    }
}