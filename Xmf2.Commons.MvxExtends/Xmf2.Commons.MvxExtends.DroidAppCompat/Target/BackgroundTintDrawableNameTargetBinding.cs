using System;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat.Target
{
    public class BackgroundTintDrawableNameTargetBinding : MvxAndroidTargetBinding
    {
        public BackgroundTintDrawableNameTargetBinding(View view)
            : base(view)
        {

        }

        protected override void SetValueImpl(object target, object value)
        {
            if (value == null)
                return;

            if (!(value is string))
            {
                System.Diagnostics.Debug.WriteLine($"Value '{value}' could not be parsed as a valid string identifier");
                return;
            }

            var resources = AndroidGlobals.ApplicationContext.Resources;
            var id = resources.GetIdentifier((string)value, "drawable", AndroidGlobals.ApplicationContext.PackageName);
            if (id == 0)
            {
                System.Diagnostics.Debug.WriteLine($"Value '{value}' was not a known drawable name");
                return;
            }

            var colorList = AndroidGlobals.ApplicationContext.Resources.GetColorStateList(id);

            ITintableBackgroundView tintableBackgroundView = null;
            try
            {
                tintableBackgroundView = ((View)target).JavaCast<ITintableBackgroundView>();
            }
            catch { }
            
            if (tintableBackgroundView != null)
                tintableBackgroundView.SupportBackgroundTintList = colorList;
            else
                ((Button)target).BackgroundTintList = colorList;
        }

        public override Type TargetType
        {
            get { return typeof(string); }
        }
    }
}