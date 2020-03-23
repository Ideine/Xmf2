using System;
using Android.Util;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Graphics;
using MvvmCross.Binding.Droid.Views;

namespace Xmf2.Commons.MvxExtends.Droid.Views
{
	[Register("xmf2.commons.mvxextends.droid.views.CircleImageView")]
	public class CircleImageView : MvxImageView
	{
		//* Constructors
		public CircleImageView(Context context, IAttributeSet attrs) : base(context, attrs)=> this.Init();
		public CircleImageView(Context context) : base(context)=> this.Init();
		protected CircleImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)=> this.Init();

		private void Init()
		{
			//Only enable hardware accelleration on lollipop
			if ((int)Android.OS.Build.VERSION.SdkInt < 21)
			{
				SetLayerType(LayerType.Software, null);
			}
		}

		public override void Draw(Android.Graphics.Canvas canvas)
		{
			try
			{
				int width = this.Width - this.PaddingLeft - this.PaddingRight;
				int height = this.Height - this.PaddingBottom - this.PaddingTop;
				var radius = Math.Min(width, height) / 2;
				//var strokeWidth = ((float)(5 * Math.Min(width, height))) / 100;
				//radius -= (int)Math.Round(strokeWidth / 2);

				// A revoir: Est-ce que c'est bien centré avec les padding?
				Path path = new Path();
				path.AddCircle(this.PaddingLeft + (width / 2), this.PaddingTop + (height / 2), radius, Path.Direction.Ccw);
				canvas.Save();
				canvas.ClipPath(path);

				base.Draw(canvas);

				canvas.Restore();

				//path = new Path();
				//path.AddCircle(this.PaddingLeft + (width / 2), this.PaddingTop + (height / 2), radius, Path.Direction.Ccw);

				//var paint = new Paint();
				//paint.AntiAlias = true;
				//paint.StrokeWidth = strokeWidth;
				//paint.SetStyle(Paint.Style.Stroke);
				//paint.Color = Color.Black;

				//canvas.DrawPath(path, paint);

				//paint.Dispose();
				path.Dispose();
				return;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Unable to create circle image: " + ex);
			}
			base.Draw(canvas);
		}
	}
}