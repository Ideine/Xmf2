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
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
    public class ButtonTextColorTargetBinding : MvxAndroidTargetBinding
    {
        public ButtonTextColorTargetBinding(Button view)
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

            Button btn = target as Button;
            if (btn != null)
            {
                btn.SetTextColor(color);
            }
        }

        public override Type TargetType
        {
            get { return typeof(Color); }
        }
    }
}