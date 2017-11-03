using System;
using Android.Animation;
using Android.Views;

namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public static class ViewRevealAnimationHelper
	{
		public static Animator StartTranslationRevealAnimation(View view, Direction direction, int duration = 800) => StartAnimation(view, new TranslationRevealValues(view, direction), duration, true);

		public static Animator StartTranslationHideAnimation(View view, Direction direction, int duration = 800) => StartAnimation(view, new TranslationRevealValues(view, direction), duration, false);

		public static Animator StartClockRevealAnimation(View view) => StartAnimation(view, new ClockRevealValues(view, true), 1500, true);

		public static Animator StartClockHideAnimation(View view) => StartAnimation(view, new ClockRevealValues(view, false), 1500, false);

		public static Animator StartCircleRevealAnimation(View view) => StartAnimation(view, new CircleRevealValues(view, true), 600, true);

		public static Animator StartCircleHideAnimation(View view) => StartAnimation(view, new CircleRevealValues(view, false), 600, false);

		private static Animator StartAnimation(View view, IRevealValues values, int duration, bool show)
		{
			if (!(view.Parent is IRevealViewGroup parent))
			{
				throw new InvalidOperationException("View must be a child of IRevealViewGroup");
			}

			ViewRevealManager manager = parent.ViewRevealManager;
			Animator animator = manager.StartAnimator(values, show);
			animator.SetDuration(duration);
			return animator;
		}
	}
}
