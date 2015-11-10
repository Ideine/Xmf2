using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Binding.Droid.Target;
using Cirrious.MvvmCross.Binding;
using Cirrious.CrossCore.Platform;

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
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' could not be parsed as a valid bool", value);
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