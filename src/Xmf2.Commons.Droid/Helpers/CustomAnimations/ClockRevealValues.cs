using System;
using Android.Graphics;
using Android.Views;

namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public class ClockRevealValues : Java.Lang.Object, IRevealValues
	{
		private readonly bool _clockwise;
		private readonly Path _path = new Path();

		public bool IsClipping { get; set; }

		public float Percentage { get; set; }

		public View Target { get; }

		public ClockRevealValues(View target, bool clockwise)
		{
			_clockwise = clockwise;
			Target = target;
		}

		protected ClockRevealValues(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public bool ApplyTransformation(Canvas canvas, View child)
		{
			if (child != Target || !IsClipping)
			{
				return false;
			}

			_path.Reset();

			var centerX = child.Width / 2f;
			var centerY = child.Height / 2f;

			var maxSize = Math.Max(child.Width, child.Height) / 2f;

			var x = child.GetX();
			var y = child.GetY();

			var relativeCenterX = x + centerX;
			var relativeCenterY = y + centerY;
			var rect = new RectF(relativeCenterX - maxSize, relativeCenterY - maxSize, relativeCenterX + maxSize, relativeCenterY + maxSize);
			_path.MoveTo(relativeCenterX, relativeCenterY);
			//_path.LineTo(relativeCenterX, relativeCenterY - maxSize); //move to top center
			if (_clockwise)
			{
				_path.ArcTo(rect, 270, 360 * Percentage, true);
			}
			else
			{
				var angle = 360 * Percentage;
				var start = (270 - angle + 360) % 360;
				_path.ArcTo(rect, start, angle, true);
			}
			_path.LineTo(x + centerX, y + centerY);
			_path.Close();

			canvas.ClipPath(_path, Region.Op.Replace);

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
			{
				child.InvalidateOutline();
			}

			return true;
		}
	}
}