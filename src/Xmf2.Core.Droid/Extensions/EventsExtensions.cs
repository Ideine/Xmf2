using System;
using Android.Views;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.Droid.Extensions
{
	public static class EventsExtensions
	{
		public static EventSubscriber<T> SubscribeScrollChanged<T>(this T view, ViewTreeObserver.IOnScrollChangedListener listener, bool autoSubscribe = true)
			where T : View
		{
			return new EventSubscriber<T>(
				view,
				v => v.ViewTreeObserver.AddOnScrollChangedListener(listener),
				v => v.ViewTreeObserver.RemoveOnScrollChangedListener(listener),
				autoSubscribe
			);
		}

		public static EventSubscriber<View> Clicked(this View view, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<View>(
				view,
				v => v.Click += handler,
				v => v.Click -= handler,
				autoSubscribe
			);
		}

		public static void SetOnTouchScaleTransformer(this View view, Xmf2Disposable disposable, float ratio = 0.95f)
		{
			new EventSubscriber<View>(
				view,
				btn => btn.SetOnTouchListener(new ScaleTouchTransformer(ratio).DisposeWith(disposable)),
				btn => btn.SetOnTouchListener(null)
			).DisposeEventWith(disposable);
		}

		public static void SetUnderlineTransformer(this View view, Xmf2Disposable disposable)
		{
			new EventSubscriber<View>(
				view,
				btn => btn.SetOnTouchListener(new UnderlineTouchListener().DisposeWith(disposable)),
				btn => btn.SetOnTouchListener(null)
			).DisposeEventWith(disposable);
		}
	}
}