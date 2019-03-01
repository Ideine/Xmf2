using System;
using Xmf2.Core.Helpers;

namespace Xmf2.Core.iOS.Controls
{
	public class UIActionHighlightButton : UIBaseHighlightButton
	{
		private Action<UIActionHighlightButton> _toHighlightedAnimation = CreatorExtensions.FadeTo58Percent;
		private Action<UIActionHighlightButton> _fromHighlightedAnimation = CreatorExtensions.FadeTo100Percent;
		private Action<UIActionHighlightButton> _toSelectedAnimation = CreatorExtensions.FadeTo58Percent;
		private Action<UIActionHighlightButton> _fromSelectedAnimation = CreatorExtensions.FadeTo100Percent;

		public Action<UIActionHighlightButton> ToHighlightedAnimation
		{
			get => _toHighlightedAnimation;
			set => _toHighlightedAnimation = value;
		}
		public Action<UIActionHighlightButton> FromHighlightedAnimation
		{
			get => _fromHighlightedAnimation;
			set => _fromHighlightedAnimation = value;
		}
		public Action<UIActionHighlightButton> ToSelectedAnimation
		{
			get => _toSelectedAnimation;
			set => _toSelectedAnimation = value;
		}
		public Action<UIActionHighlightButton> FromSelectedAnimation
		{
			get => _fromSelectedAnimation;
			set => _fromSelectedAnimation = value;
		}

		public double ToHighlightedAnimationDuration { get; set; } = 0.2;
		public double FromHighlightedAnimationDuration { get; set; } = 0.2;
		public double ToSelectedAnimationDuration { get; set; } = 0.2;
		public double FromSelectedAnimationDuration { get; set; } = 0.2;

		protected override void OnHighlighted() => Animate(ToHighlightedAnimationDuration, () => ToHighlightedAnimation(this), ActionHelper.NoOp);
		protected override void OnUnhighlighted() => Animate(ToHighlightedAnimationDuration, () => FromHighlightedAnimation(this), ActionHelper.NoOp);
		protected override void OnSelected() => Animate(ToSelectedAnimationDuration, () => ToSelectedAnimation(this), ActionHelper.NoOp);
		protected override void OnUnselected() => Animate(FromSelectedAnimationDuration, () => FromSelectedAnimation(this), ActionHelper.NoOp);


		protected override void Dispose(bool disposing)
		{
			ToHighlightedAnimation = null;
			FromHighlightedAnimation = null;
			ToSelectedAnimation = null;
			FromSelectedAnimation = null;
			base.Dispose(disposing);
		}
	}
}