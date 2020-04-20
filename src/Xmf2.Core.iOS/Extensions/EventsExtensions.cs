using System;
using UIKit;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.iOS.Extensions
{
	public static class EventsExtensions
	{
		public static EventSubscriber<TControl> TouchUpInsideSubscription<TControl>(this TControl button, EventHandler handler, bool autoSubscribe = true)
			where TControl : UIControl
		{
			return new EventSubscriber<TControl>(
				button,
				b => b.TouchUpInside += handler,
				b => b.TouchUpInside -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<TControl> TouchUpInsideSubscription<TControl>(this TControl button, Action handler, bool autoSubscribe = true)
			where TControl : UIControl
		{
			return TouchUpInsideSubscription(button, (sender, e) => handler(), autoSubscribe);
		}

		public static EventSubscriber<UITextField> TextChanged(this UITextField input, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UITextField>(
				input,
				i => i.EditingChanged += handler,
				i => i.EditingChanged -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<UITextField> TextEditingDidBegin(this UITextField input, EventHandler onEditingDidBegin, bool autoSubscribe = true)
			=> new EventSubscriber<UITextField>(
				input,
				i => i.EditingDidBegin += onEditingDidBegin,
				i => i.EditingDidBegin -= onEditingDidBegin,
				autoSubscribe
			);

		public static EventSubscriber<UITextField> TextEditingDidEnd(this UITextField input, EventHandler onEditingDidEnd, bool autoSubscribe = true)
			=> new EventSubscriber<UITextField>(
				input,
				i => i.EditingDidEnd += onEditingDidEnd,
				i => i.EditingDidEnd -= onEditingDidEnd,
				autoSubscribe
			);

		public static EventSubscriber<UIScrollView> ScrollChanged(this UIScrollView scrollView, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UIScrollView>(
				scrollView,
				s => s.Scrolled += handler,
				s => s.Scrolled -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<UIScrollView> OnScrolledToTop(this UIScrollView scrollView, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UIScrollView>(
				scrollView,
				s => s.ScrolledToTop += handler,
				s => s.ScrolledToTop -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<UISearchBar> OnTextChanged(this UISearchBar input, EventHandler<UISearchBarTextChangedEventArgs> handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UISearchBar>(
				input,
				i => i.TextChanged += handler,
				i => i.TextChanged -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<UISearchBar> OnCancelClicked(this UISearchBar input, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UISearchBar>(
				input,
				i => i.CancelButtonClicked += handler,
				i => i.CancelButtonClicked -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<UISearchBar> EditingStarted(this UISearchBar input, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UISearchBar>(
				input,
				i => i.OnEditingStarted += handler,
				i => i.OnEditingStarted -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<UISearchBar> EditingStopped(this UISearchBar input, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UISearchBar>(
				input,
				i => i.OnEditingStopped += handler,
				i => i.OnEditingStopped -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<UISearchBar> OnSearchButtonClicked(this UISearchBar input, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UISearchBar>(
				input,
				i => i.SearchButtonClicked += handler,
				i => i.SearchButtonClicked -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<TUIView> FromGestureRecognizer<TUIView>(this TUIView view, UIGestureRecognizer recognizer, bool autoSubscribe = true) where TUIView : UIView
		{
			return new EventSubscriber<TUIView>(
				view,
				v => v.AddGestureRecognizer(recognizer),
				v => v.RemoveGestureRecognizer(recognizer),
				autoSubscribe
			);
		}
	}
}