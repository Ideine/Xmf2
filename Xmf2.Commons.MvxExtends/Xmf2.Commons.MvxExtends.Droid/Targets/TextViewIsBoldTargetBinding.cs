using System;
using Android.Graphics;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

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
                System.Diagnostics.Debug.Write($"Value '{value}' could not be parsed as a boolean");
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