using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Xmf2.Commons.Droid.Controls
{
	public class SquareRelativeLayout : RelativeLayout
	{
		public enum Type
		{
			Width,
			Height,
			Greatest,
			Smallest
		}

		public Type SquareType { get; set; }

		protected SquareRelativeLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public SquareRelativeLayout(Context context) : base(context) { }

		public SquareRelativeLayout(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public SquareRelativeLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			var width = MeasureSpec.GetSize(widthMeasureSpec);
			var height = MeasureSpec.GetSize(heightMeasureSpec);
			switch (SquareType)
			{
				case Type.Width:
					base.OnMeasure(widthMeasureSpec, widthMeasureSpec);
					break;
				case Type.Height:
					base.OnMeasure(heightMeasureSpec, heightMeasureSpec);
					break;
				case Type.Greatest:
					if (width > height)
					{
						base.OnMeasure(widthMeasureSpec, widthMeasureSpec);
					}
					else
					{
						base.OnMeasure(heightMeasureSpec, heightMeasureSpec);
					}
					break;
				case Type.Smallest:
					if (width > height)
					{
						base.OnMeasure(heightMeasureSpec, heightMeasureSpec);
					}
					else
					{
						base.OnMeasure(widthMeasureSpec, widthMeasureSpec);
					}
					break;
			}
		}
	}
}