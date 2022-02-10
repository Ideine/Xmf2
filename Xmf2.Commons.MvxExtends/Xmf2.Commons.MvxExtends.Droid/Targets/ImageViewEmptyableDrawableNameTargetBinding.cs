using Android.Widget;
using MvvmCross.Platforms.Android.Binding.Target;

namespace Xmf2.Commons.MvxExtends.Droid.Targets
{
	public class ImageViewEmptyableDrawableNameTargetBinding : MvxImageViewDrawableNameTargetBinding
	{
		public ImageViewEmptyableDrawableNameTargetBinding(ImageView imageView) : base(imageView) { }
		/*
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
		*/
	}
}