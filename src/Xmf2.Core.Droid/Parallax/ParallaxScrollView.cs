using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.Droid.Parallax
{
	/// <summary>
	/// CODE FROM : https://github.com/nirhart/ParallaxScroll/blob/master/ParallaxScroll/src/com/nirhart/parallaxscroll/views/ParallaxScrollView.java
	/// </summary>
	public class ParallaxScrollView : ScrollView
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();

		private const int DEFAULT_PARALLAX_VIEWS = 1;
		private const float DEFAULT_INNER_PARALLAX_FACTOR = 1.9F;
		private const float DEFAULT_PARALLAX_FACTOR = 1.9F;
		private const float DEFAULT_ALPHA_FACTOR = -1F;
		private const int DEFAULT_SCROLLBAR_OFFSET = 0;

		private int _numOfParallaxViews = DEFAULT_PARALLAX_VIEWS;
		private float _innerParallaxFactor = DEFAULT_PARALLAX_FACTOR;
		private float _parallaxFactor = DEFAULT_PARALLAX_FACTOR;
		private float _alphaFactor = DEFAULT_ALPHA_FACTOR;

		private readonly List<ParallaxedView> _parallaxedViews = new List<ParallaxedView>();

		protected ParallaxScrollView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public ParallaxScrollView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Init(context, attrs);
		}

		public ParallaxScrollView(Context context, IAttributeSet attrs) : base(context, attrs)
		{
			Init(context, attrs);
		}

		public ParallaxScrollView(Context context) : base(context) { }

		protected void Init(Context context, IAttributeSet attrs)
		{
			var typeArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.ParallaxScroll);
			_parallaxFactor = typeArray.GetFloat(Resource.Styleable.ParallaxScroll_parallax_factor, DEFAULT_PARALLAX_FACTOR);
			_alphaFactor = typeArray.GetFloat(Resource.Styleable.ParallaxScroll_alpha_factor, DEFAULT_ALPHA_FACTOR);
			_innerParallaxFactor = typeArray.GetFloat(Resource.Styleable.ParallaxScroll_inner_parallax_factor, DEFAULT_INNER_PARALLAX_FACTOR);
			_numOfParallaxViews = typeArray.GetInt(Resource.Styleable.ParallaxScroll_parallax_views_num, DEFAULT_PARALLAX_VIEWS);
			typeArray.Recycle();
		}

		protected override void OnFinishInflate()
		{
			base.OnFinishInflate();
			MakeViewsParallax();
		}

		private void MakeViewsParallax()
		{
			if (ChildCount > 0 && GetChildAt(0) is ViewGroup viewHolders)
			{
				int numOfParallaxViews = Math.Min(_numOfParallaxViews, viewHolders.ChildCount);
				for (int i = 0; i < numOfParallaxViews; i++)
				{
					AddParallaxView(viewHolders.GetChildAt(i));
				}
			}
		}

		public void AddParallaxView(View v)
		{
			_parallaxedViews.Add(new ParallaxedView(v).DisposeWith(_disposable));
		}

		protected override void OnScrollChanged(int horizontalScrollOrigin, int verticalScrollOrigin, int oldHorizontalScrollOrigin, int oldVerticalScrollOrigin)
		{
			base.OnScrollChanged(horizontalScrollOrigin, verticalScrollOrigin, oldHorizontalScrollOrigin, oldVerticalScrollOrigin);
			float parallax = _parallaxFactor;
			float alpha = _alphaFactor;
			foreach (ParallaxedView parallaxedView in _parallaxedViews)
			{
				parallaxedView.SetOffset(verticalScrollOrigin / parallax, (int) Math.Round(verticalScrollOrigin / _parallaxFactor));
				parallax *= _innerParallaxFactor;
				if (alpha != DEFAULT_ALPHA_FACTOR)
				{
					float fixedAlpha = verticalScrollOrigin <= 0 ? 1 : 100 / (verticalScrollOrigin * alpha);
					parallaxedView.SetAlpha(fixedAlpha);
					alpha /= _alphaFactor;
				}

				parallaxedView.AnimateNow();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}