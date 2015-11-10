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
    public class TextViewIsBoldTargetBinding : MvxAndroidTargetBinding
    {
        public TextViewIsBoldTargetBinding(TextView view)
            : base(view)
        {

        }

        protected override void SetValueImpl(object target, object value)
        {
            if (!(value is bool))
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' could not be parsed as a boolean", value);
            }

            TextView tvw = target as TextView;
            if (tvw != null)
            {
                if((bool)value)
                    tvw.SetTypeface(tvw.Typeface, TypefaceStyle.Bold);
                else
                    tvw.SetTypeface(tvw.Typeface, TypefaceStyle.Normal);
            }
        }

        public override Type TargetType
        {
            get { return typeof(bool); }
        }
    }
}