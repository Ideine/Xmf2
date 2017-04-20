using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using MvvmCross.Binding.Droid.Views;

namespace Xmf2.Commons.MvxExtends.Droid
{
	[Register("xmf2.commons.mvxextends.droid.views.MvxTopCropImageView")]
	public class MvxTopCropImageView : MvxImageView
	{
		public MvxTopCropImageView(Context context, IAttributeSet attrs)
					: base(context, attrs)
		{
			Init();
		}

		public MvxTopCropImageView(Context context)
					: base(context)
		{
			Init();
		}

		protected MvxTopCropImageView(IntPtr javaReference, JniHandleOwnership transfer)
					: base(javaReference, transfer)
		{
			Init();
		}

		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);
			RecomputeImgMatrix();
		}

		protected override bool SetFrame(int l, int t, int r, int b)
		{
			RecomputeImgMatrix();
			return base.SetFrame(l, t, r, b);
		}

		private void Init()
		{
			SetScaleType(ScaleType.Matrix);
		}

		private void RecomputeImgMatrix()
		{
			if (Drawable == null)
			{
				return;
			}

			int viewWidth = Width - PaddingRight - PaddingLeft;
			int viewHeight = Height - PaddingTop - PaddingBottom;
			float scale;
			if (Drawable.IntrinsicWidth * viewHeight > Drawable.IntrinsicHeight * viewWidth)
			{
				scale = (float)viewHeight / (float)Drawable.IntrinsicHeight;
			}
			else
			{
				scale = (float)viewWidth / (float)Drawable.IntrinsicWidth;
			}
			var matrix = ImageMatrix;
			matrix.SetScale(scale, scale);
			ImageMatrix = matrix;

		}
	}
}
