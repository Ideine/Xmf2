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

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
    public class ImageViewEmptyableDrawableNameTargetBinding : MvxImageViewDrawableNameTargetBinding
    {
        public ImageViewEmptyableDrawableNameTargetBinding(ImageView imageView)
            : base(imageView)
        {

        }

        protected override bool GetBitmap(object value, out Android.Graphics.Bitmap bitmap)
        {
            if (value == null)
            {
                bitmap = null;
                return false;
            }


            if (value is string)
            {
                if (string.IsNullOrEmpty((string)value))
                {
                    bitmap = null;
                    return false;
                }
            }

            return base.GetBitmap(value, out bitmap);
        }
    }
}