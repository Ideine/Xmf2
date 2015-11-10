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
using Android.Graphics;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
    public class TextViewTextColorTargetBinding : MvxAndroidTargetBinding
    {
        public TextViewTextColorTargetBinding(TextView view)
            : base(view)
        {

        }

        protected override void SetValueImpl(object target, object value)
        {
            if (!(value is Color))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' could not be parsed as a valid Color", value);
            }

            Color color = (Color)value;

            TextView tvw = target as TextView;
            if (tvw != null)
            {
                tvw.SetTextColor(color);
            }
        }

        public override Type TargetType
        {
            get { return typeof(Color); }
        }
    }
}