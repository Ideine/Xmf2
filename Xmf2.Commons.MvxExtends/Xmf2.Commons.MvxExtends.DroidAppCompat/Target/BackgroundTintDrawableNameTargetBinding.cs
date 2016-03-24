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
using Android.Support.V4.View;
using Android.Content.Res;
using Android.Support.V7.Widget;

using Java.Lang;
using MvvmCross.Binding.Droid.Target;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;

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
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' could not be parsed as a valid string identifier", value);
                return;
            }

            var resources = AndroidGlobals.ApplicationContext.Resources;
            var id = resources.GetIdentifier((string)value, "drawable", AndroidGlobals.ApplicationContext.PackageName);
            if (id == 0)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Value '{0}' was not a known drawable name", value);
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