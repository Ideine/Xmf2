using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Xmf2.Commons.Droid.Controls
{
	public class SquareRelativeLayout : RelativeLayout
	{
		protected SquareRelativeLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public SquareRelativeLayout(Context context) : base(context) { }

		public SquareRelativeLayout(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public SquareRelativeLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			if (widthMeasureSpec < heightMeasureSpec)
			{
				base.OnMeasure(widthMeasureSpec, widthMeasureSpec);
			}
			else
			{
				base.OnMeasure(heightMeasureSpec, heightMeasureSpec);
			}
		}
	}
}