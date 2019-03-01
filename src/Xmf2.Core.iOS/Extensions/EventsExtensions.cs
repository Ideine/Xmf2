﻿using System;
using UIKit;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.iOS.Extensions
{
	public static class EventsExtensions
	{
		public static EventSubscriber<UIControl> TouchUpInsideSubscription(this UIControl button, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UIControl>(
				button,
				b => b.TouchUpInside += handler,
				b => b.TouchUpInside -= handler,
				autoSubscribe);
		}

		public static EventSubscriber<UIControl> TouchUpInsideSubscription(this UIControl button, Action handler, bool autoSubscribe = true)
		{
			return TouchUpInsideSubscription(button, (object sender, EventArgs e) => handler(), autoSubscribe);
		}

		public static EventSubscriber<UITextField> TextChanged(this UITextField input, EventHandler handler, bool autoSubscribe = true)
		{
			return new EventSubscriber<UITextField>(
				input,
				i => i.EditingChanged += handler,
				i => i.EditingChanged -= handler,
				autoSubscribe);
		}

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