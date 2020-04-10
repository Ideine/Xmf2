using System;
using Android.Runtime;
using Android.Views;
using Xmf2.Components.Events;

namespace Xmf2.Components.Droid.Helpers
{
	public class CollapseLevelScrollChangedHelper : Java.Lang.Object, ViewTreeObserver.IOnScrollChangedListener, ViewTreeObserver.IOnGlobalLayoutListener
	{
		private float _lastValue;
		private Func<int> _getTopLine;
		private IEventBus _eventBus;
		private Func<int> _getScrollY;
		private bool _force;
		private readonly bool _withBackground;

		public bool Force
		{
			get => _force;
			set
			{
				_force = value;
				OnScrollChanged();
			}
		}

		protected CollapseLevelScrollChangedHelper(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		public CollapseLevelScrollChangedHelper(IEventBus eventBus, Func<int> getScrollY, Func<int> getTopLine, bool withBackground)
		{
			_getTopLine = getTopLine;
			_eventBus = eventBus;
			_getScrollY = getScrollY;
			_withBackground = withBackground;
		}

		public void OnScrollChanged()
		{
			if (Force)
			{
				_lastValue = 1;
				_eventBus.Publish(new CollapseLevelEvent(1, _withBackground));
				return;
			}

			int scrollY;
			int topLine;
			try
			{
				scrollY = _getScrollY();
				topLine = _getTopLine();
			}
			catch (NullReferenceException)
			{
				//ignore exception caused by dispose
				return;
			}

			if (scrollY >= topLine)
			{
				float val = Math.Min((scrollY - topLine) * 4, 255) / 255f;
				if (_lastValue != val)
				{
					_eventBus.Publish(new CollapseLevelEvent(val, _withBackground));
					_lastValue = val;
				}
			}
			else
			{
				if (_lastValue != 0)
				{
					_eventBus.Publish(new CollapseLevelEvent(0, _withBackground));
					_lastValue = 0;
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_eventBus = null;
				_getScrollY = null;
				_getTopLine = null;
			}

			base.Dispose(disposing);
		}

		public void OnGlobalLayout()
		{
			OnScrollChanged();
		}
	}
}