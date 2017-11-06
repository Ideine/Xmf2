using Android.Graphics;
using Android.Views;
using Xmf2.Commons.Extensions;

namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public enum Direction
	{
		LeftToRight,
		RightToLeft,
		TopToBottom,
		BottomToTop,
		//TODO diagonale
	}

	public class TranslationRevealValues : Java.Lang.Object, IRevealValues
	{
		private readonly Direction _direction;

		public bool IsClipping { get; set; }

		public float Percentage { get; set; }

		public View Target { get; }

		public TranslationRevealValues(View target, Direction direction)
		{
			_direction = direction;
			Target = target;
		}

		protected TranslationRevealValues(System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public bool ApplyTransformation(Canvas canvas, View child)
		{
			if (child != Target || !IsClipping)
			{
				return false;
			}

			Rect clippingRect = GetTransformationRectFromDirection(child, _direction);

			canvas.ClipRect(clippingRect, Region.Op.Replace);

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
			{
				child.InvalidateOutline();
			}

			return true;
		}

		private Rect GetTransformationRectFromDirection(View child, Direction dir)
		{
			switch (dir)
			{
				case Direction.LeftToRight:
					return new Rect((int)(child.GetX()), (int)child.GetY(), (int)(child.GetX() + child.Width * Percentage), (int)(child.GetY() + child.Height));
				case Direction.RightToLeft:
					return new Rect((int)(child.GetX() + child.Width * (1 - Percentage)), (int)child.GetY(), (int)(child.GetX() + child.Width), (int)(child.GetY() + child.Height));
				case Direction.BottomToTop:
					return new Rect();//TODO
				case Direction.TopToBottom:
					return new Rect((int)(child.GetX()), (int)child.GetY(), (int)(child.GetX() + child.Width), (int)(child.GetY() + child.Height * Percentage));
				default:
					throw dir.GetNotSupportedException();
			}
		}
	}
}