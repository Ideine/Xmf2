using System;
using CoreGraphics;
using UIKit;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.iOS.Helpers
{
	public static class OnTouchTransformer
	{
		private static readonly CGAffineTransform _noTransform = new CGAffineTransform(1, 0, 0, 1, 0, 0);

		public const float DEFAULT_SCALE_DOWN_RATIO = 0.95f;
		public static EventSubscriber<UIControl> AffineTransformOnTouch(this UIControl control, CGAffineTransform affineTransform, UIView transformedView, bool animate)
		{
			void applyAffineTransform(object s, EventArgs e)
			{
				if (animate)
				{
					UIView.Animate(0.2, () => transformedView.Transform = affineTransform);
				}
				else
				{
					transformedView.Transform = affineTransform;
				}
			}
			void cancelAffineTransform(object s, EventArgs e)
			{
				if (animate)
				{
					UIView.Animate(0.2, () => transformedView.Transform = _noTransform);
				}
				else
				{
					transformedView.Transform = _noTransform;
				}
			}

			return new EventSubscriber<UIControl>(
				control,
				v =>
				{
					v.TouchDown += applyAffineTransform;
					v.TouchCancel += cancelAffineTransform;
					v.TouchUpInside += cancelAffineTransform;
					v.TouchUpOutside += cancelAffineTransform;
				},
				v =>
				{
					v.TouchDown -= applyAffineTransform;
					v.TouchCancel -= cancelAffineTransform;
					v.TouchUpInside -= cancelAffineTransform;
					v.TouchUpOutside -= cancelAffineTransform;
				}
			);
		}
		public static EventSubscriber<UIControl> FadeOnTouch(this UIControl control, UIView updatedView, bool animate = true)
		{
			return control.UpdateOnTouch(updatedView,
				toTouchedState: v => v.FadeTo58Percent(),
				fromTouchedState: v => v.FadeTo100Percent(),
				animate: animate
			);
		}
		public static EventSubscriber<UIControl> ScaleOnTouch(this UIControl control, float ratio = DEFAULT_SCALE_DOWN_RATIO, UIView scaledView = null, bool animate = true)
		{
			var scaleDownTransform = (scaledView ?? control).GetScaleTransform(ratio);
			return control.AffineTransformOnTouch(scaleDownTransform, scaledView, animate);
		}
		public static EventSubscriber<UIControl> UpdateOnTouch<TView>(this UIControl control, TView updatedView, Action<TView> toTouchedState, Action<TView> fromTouchedState, bool animate = true)
			where TView : UIView
		{
			void applyToTouchedState(object s, EventArgs e)
			{
				if (animate)
				{
					UIView.Animate(0.2, () => toTouchedState(updatedView));
				}
				else
				{
					toTouchedState(updatedView);
				}
			}
			void applyFromTouchedState(object s, EventArgs e)
			{
				if (animate)
				{
					UIView.Animate(0.2, () => fromTouchedState(updatedView));
				}
				else
				{
					fromTouchedState(updatedView);
				}
			}

			return new EventSubscriber<UIControl>(
				control,
				v =>
				{
					v.TouchDown += applyToTouchedState;
					v.TouchCancel += applyFromTouchedState;
					v.TouchUpInside += applyFromTouchedState;
					v.TouchUpOutside += applyFromTouchedState;
				},
				v =>
				{
					v.TouchDown -= applyToTouchedState;
					v.TouchCancel -= applyFromTouchedState;
					v.TouchUpInside -= applyFromTouchedState;
					v.TouchUpOutside -= applyFromTouchedState;
				}
			);
		}

		public static TControl WithAffineTransformOnTouch<TControl>(this TControl control, Xmf2Disposable disposer, CGAffineTransform affineTransform, UIView appliedOn, bool animate) where TControl : UIControl
		{
			control.AffineTransformOnTouch(affineTransform, appliedOn, animate).DisposeEventWith(disposer);
			return control;
		}
		public static TControl WithFadeOnTouch<TControl>(this TControl control, Xmf2Disposable disposer, UIView updatedView, bool animate = true)
		 	where TControl : UIControl
		{
			control.FadeOnTouch(updatedView, animate: animate).DisposeWith(disposer);
			return control;
		}
		public static TControl WithScaleOnTouch<TControl>(this TControl control, Xmf2Disposable disposer, UIView scaledView, float ratio = DEFAULT_SCALE_DOWN_RATIO, bool animate = true) where TControl : UIControl
		{
			control.ScaleOnTouch(ratio, scaledView, animate).DisposeWith(disposer);
			return control;
		}
		public static TControl WithUpdateOnTouch<TControl, TView>(this TControl control, Xmf2Disposable disposer, TView updatedView, Action<TView> toTouchedState, Action<TView> fromTouchedState, bool animate)
			where TControl : UIControl
			where TView : UIView
		{
			control.UpdateOnTouch(updatedView, toTouchedState, fromTouchedState, animate).DisposeEventWith(disposer);
			return control;
		}
	}
}