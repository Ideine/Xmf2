using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;

namespace Xmf2.Components.Droid.Controls
{
	public class ComponentStub : View
	{
		public ComponentStub(Context context) : base(context)
		{
			Initialize();
		}

		public ComponentStub(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Initialize();
		}

		public ComponentStub(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{
			Initialize();
		}

		public ComponentStub(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
		{
			Initialize();
		}

		protected ComponentStub(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
			Initialize();
		}

		private void Initialize()
		{
			Visibility = ViewStates.Gone;
			SetWillNotDraw(true);
		}

		public View SetComponent(IComponentView componentView)
		{
			if (Parent is ViewGroup parent)
			{
				int currentIndexInParent = parent.IndexOfChild(this);
				parent.RemoveViewInLayout(this);

				var layoutParams = LayoutParameters;
				var view = componentView.View(parent);
				view.Id = Id;

				if (layoutParams != null)
				{
					parent.AddView(view, currentIndexInParent, layoutParams);
				}
				else
				{
					parent.AddView(view, currentIndexInParent);
				}

				return view;
			}
			else
			{
				throw new Exception($"{nameof(ComponentStub)}.{Id} need to be in a {nameof(ViewGroup)}");
			}
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			SetMeasuredDimension(0, 0);
		}
	}
}