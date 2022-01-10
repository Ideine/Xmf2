using System;
using Android.Text;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

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
                System.Diagnostics.Debug.Write($"Value '{value}' could not be parsed as a ISpanned");
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