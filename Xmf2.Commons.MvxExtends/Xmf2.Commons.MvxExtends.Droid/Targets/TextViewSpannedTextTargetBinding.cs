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
using Android.Graphics;
using Android.Text;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
    public class TextViewSpannedTextTargetBinding : MvxAndroidTargetBinding
    {
        public TextViewSpannedTextTargetBinding(TextView view)
            : base(view)
        {

        }

        protected override void SetValueImpl(object target, object value)
        {
            if (value != null && !(value is ISpanned))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' could not be parsed as a ISpanned", value);
            }

            TextView tvw = target as TextView;
            if (tvw != null)
            {
                tvw.SetText((ISpanned)value, TextView.BufferType.Spannable);
            }
        }

        public override Type TargetType
        {
            get { return typeof(ISpanned); }
        }
    }
}