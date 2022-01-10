using System;
using Android.Graphics;
using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

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
                System.Diagnostics.Debug.Write($"Value '{value}' could not be parsed as a valid Color");
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