using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public class RevealFrameLayout : FrameLayout, IRevealViewGroup
	{
		public ViewRevealManager ViewRevealManager { get; } = new ViewRevealManager();

		public RevealFrameLayout(Context ctx) : base(ctx)
		{

		}

		public RevealFrameLayout(Context ctx, IAttributeSet attrs) : base(ctx, attrs)
		{

		}

		public RevealFrameLayout(Context ctx, IAttributeSet attrs, int defStyle) : base(ctx, attrs, defStyle)
		{

		}

		protected RevealFrameLayout(IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public void ForceDraw()
		{
			Invalidate();
		}

		public override void OnViewAdded(View child)
		{
			base.OnViewAdded(child);
		}

		protected override void DispatchDraw(Canvas canvas)
		{
			base.DispatchDraw(canvas);
		}

		protected override bool DrawChild(Canvas canvas, View child, long drawingTime)
		{
			try
			{
				canvas.Save();
				ViewRevealManager.Transform(canvas, child);
				return base.DrawChild(canvas, child, drawingTime);
			}
			finally
			{
				canvas.Restore();
			}
		}
	}
}