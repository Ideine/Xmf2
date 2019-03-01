using System;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Xmf2.Core.Droid.Controls
{
	public class RoundedFrameLayout : FrameLayout
	{
		public float Radius { get; set; } = 0;

		private Path _path = new Path();

		public RoundedFrameLayout(Context context) : base(context) { }

		public RoundedFrameLayout(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public RoundedFrameLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected RoundedFrameLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);
			_path.Reset();
			using (var rect = new RectF())
			{
				rect.Set(0, 0, w, h);
				_path.AddRoundRect(rect, Radius, Radius, Path.Direction.Cw);
			}

			_path.Close();
		}

		protected override void DispatchDraw(Canvas canvas)
		{
			int save = canvas.Save();
			canvas.ClipPath(_path);
			base.DispatchDraw(canvas);
			canvas.RestoreToCount(save);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_path?.Dispose();
				_path = null;
			}
			base.Dispose(disposing);
		}
	}
}
