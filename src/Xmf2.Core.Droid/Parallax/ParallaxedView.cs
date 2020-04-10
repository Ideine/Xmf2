using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.Droid.Parallax
{
	/// <summary>
	/// CODE FROM : https://github.com/nirhart/ParallaxScroll/blob/master/ParallaxScroll/src/com/nirhart/parallaxscroll/views/ParallaxedView.java
	/// </summary>
	public class ParallaxedView : IDisposable
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();

		private static readonly bool IsApi18 = Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr2;

		protected WeakReference<View> ViewReference;
		protected int LastOffset { get; set; }
		protected readonly List<Animation> Animations;
		protected Rect ClippingRect;
		protected bool FirstRectGet { get; set; } = false;

		private int _lastScrollY;

		public ParallaxedView(View view)
		{
			LastOffset = 0;
			Animations = new List<Animation>();
			ViewReference = new WeakReference<View>(view);
			ClippingRect = new Rect().DisposeWith(_disposable);
		}

		public bool IsView(View v)
		{
			return v != null && ViewReference != null && ViewReference.TryGetTarget(out var view) && view.Equals(v);
		}

		public void SetOffset(float offset)
		{
			if (ViewReference.TryGetTarget(out var view))
			{
				view.TranslationY = offset;
			}
		}

		public void SetOffset(float offset, int originalScrollY)
		{
			if (ViewReference.TryGetTarget(out var view))
			{
				if (!FirstRectGet)
				{
					ClippingRect.Top = view.Top;
					ClippingRect.Left = view.Left;
					ClippingRect.Right = view.Right;
					ClippingRect.Bottom = view.Bottom;
				}

				if (_lastScrollY == 0)
				{
					_lastScrollY = originalScrollY;
				}

				int delta = LastOffset - originalScrollY;

				ClippingRect.Bottom += delta;
				view.TranslationY = (float) Math.Round(offset);

				if (IsApi18)
				{
					view.ClipBounds = ClippingRect;
				}

				_lastScrollY = originalScrollY;

				if (IsApi18)
				{
					if (ClippingRect.Bottom <= ClippingRect.Top)
					{
						view.Visibility = ViewStates.Invisible;
					}
					else
					{
						view.Visibility = ViewStates.Visible;
					}
				}
			}
		}

		public void SetAlpha(float alpha)
		{
			if (ViewReference.TryGetTarget(out var view))
			{
				view.Alpha = alpha;
			}
		}

		private readonly object _addAnimationLocker = new object();

		protected void AddAnimation(Animation animation)
		{
			lock (_addAnimationLocker)
			{
				Animations.Add(animation);
			}
		}

		private readonly object _animationNowLocker = new object();

		public void AnimateNow()
		{
			lock (_animationNowLocker)
			{
				if (ViewReference.TryGetTarget(out var view))
				{
					var set = new AnimationSet(true).DisposeWith(_disposable);
					foreach (var animation in Animations)
					{
						if (animation != null)
						{
							set.AddAnimation(animation);
						}
					}

					set.Duration = 0;
					set.FillAfter = true;
					view.Animation = set;
					set.Start();
					Animations.Clear();
				}
			}
		}

		public void SetView(View view)
		{
			ViewReference = new WeakReference<View>(view);
		}

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ClippingRect = null;
				ViewReference = null;

				_disposable.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ParallaxedView()
		{
			Dispose(false);
		}

		#endregion
	}
}