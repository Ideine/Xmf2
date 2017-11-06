using System;
using Android.Animation;
using Android.Graphics;
using Android.Views;

namespace Xmf2.Commons.Droid.Helpers.CustomAnimations
{
	public class ViewRevealManager
	{
		private IRevealValues _values;
		private Animator _animator;

		public Animator StartAnimator(IRevealValues values, bool reveal)
		{
			float start = reveal ? 0 : 1;
			float end = reveal ? 1 : 0;
			_values = values;
			_animator = ObjectAnimator.OfFloat((Java.Lang.Object) values, new RevealValueProperty(), start, end);

			_animator.AnimationStart += AnimationStart;
			_animator.AnimationEnd += AnimationEnd;

			return _animator;
		}

		private void AnimationStart(object sender, EventArgs e)
		{
			_values.IsClipping = true;

			(sender as Animator).AnimationStart -= AnimationStart;
		}

		private void AnimationEnd(object sender, EventArgs e)
		{
			_values.IsClipping = false;

			(sender as Animator).AnimationEnd -= AnimationEnd;
		}

		public bool Transform(Canvas canvas, View child)
		{
			return _values?.ApplyTransformation(canvas, child) == true;
		}
	}
}