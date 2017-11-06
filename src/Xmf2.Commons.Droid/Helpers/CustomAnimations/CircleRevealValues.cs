using System;
using Android.Graphics;
using Android.Views;

namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public class CircleRevealValues : Java.Lang.Object, IRevealValues
	{
		private readonly bool _clockwise;
		private readonly Path _path = new Path();

		public bool IsClipping { get; set; }

		public float Percentage { get; set; }

		public View Target { get; }

		public CircleRevealValues(View target, bool clockwise)
		{
			_clockwise = clockwise;
			Target = target;
		}

		protected CircleRevealValues(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public bool ApplyTransformation(Canvas canvas, View child)
		{
			if (child != Target || !IsClipping)
			{
				return false;
			}

			_path.Reset();

			var centerX = child.Width / 2f;
			var centerY = child.Height / 2f;

			var maxSize = Math.Max(child.Width, child.Height) / 2f * Percentage;

			var x = child.GetX();
			var y = child.GetY();

			var relativeCenterX = x + centerX;
			var relativeCenterY = y + centerY;

			_path.AddCircle(relativeCenterX, relativeCenterY, maxSize, Path.Direction.Cw);

			canvas.ClipPath(_path, Region.Op.Replace);

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
			{
				child.InvalidateOutline();
			}

			return true;
		}
	}
}