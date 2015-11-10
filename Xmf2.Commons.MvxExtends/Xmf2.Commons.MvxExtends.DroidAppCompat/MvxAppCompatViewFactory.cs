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
using Cirrious.MvvmCross.Binding.Droid.Binders;

namespace Xmf2.Commons.MvxExtends.DroidAppCompat
{
    public class MvxAppCompatViewFactory : MvxAndroidViewFactory
    {
        private static readonly int SdkInt = (int)Build.VERSION.SdkInt;

        public override View CreateView(View parent, string name, Context context, Android.Util.IAttributeSet attrs)
        {
            var activity = context as Activity;
            if (activity != null)
            {
                LayoutInflater originalInflater = activity.LayoutInflater;

                if (SdkInt >= 11)
                {
                    var view = originalInflater.Factory2.OnCreateView(parent, name, context, attrs);
                    if (view != null)
                        return view;
                }

                var viewFact = originalInflater.Factory.OnCreateView(name, context, attrs);
                if (viewFact != null)
                    return viewFact;
            }

            return base.CreateView(parent, name, context, attrs);
        }
    }
}