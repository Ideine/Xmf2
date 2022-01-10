using System;
using Android.Graphics;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

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
                System.Diagnostics.Debug.Write($"Value '{value}' could not be parsed as a valid Color");
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