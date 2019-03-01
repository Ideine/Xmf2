using System;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.List
{
	public class StickyRecyclerHelper : RecyclerView.OnScrollListener
	{
		private Xmf2Disposable _disposable = new Xmf2Disposable();

		private bool _enabled;
		private View _componentView;
		private View _recyclerView;
		private GlobalLayoutHelper _helper;
		private readonly int _offset;

		public StickyRecyclerHelper(View componentView, View recyclerView, int offset, bool enabled)
		{
			_componentView = componentView;
			_recyclerView = recyclerView;
			_offset = offset;
			_enabled = enabled;

			_helper = new GlobalLayoutHelper(recyclerView, SynchronizeTop).DisposeWith(_disposable);

			new EventSubscriber<View>(
				_recyclerView,
				recycler => recycler.ViewTreeObserver.AddOnGlobalLayoutListener(_helper),
				recycler => recycler.ViewTreeObserver.RemoveOnGlobalLayoutListener(_helper)
			).DisposeEventWith(_disposable);
		}

		protected StickyRecyclerHelper(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
		{
			base.OnScrolled(recyclerView, dx, dy);
			SynchronizeTop();
		}

		public void SetEnabled(bool enabled)
		{
			_enabled = enabled;
			SynchronizeTop();
		}

		private void SynchronizeTop()
		{
			if (_enabled)
			{
				var top = _recyclerView.Top;

				if (_recyclerView.WindowVisibility == ViewStates.Gone)
				{
					top = 0;
				}

				if (top < 0)
				{
					top = 0;
				}

				top += _offset;

				_componentView.Bottom = top + _componentView.Height;
				_componentView.Top = top;
			}
			else
			{
				var top = _recyclerView.Top;
				top += _offset;
				_componentView.Bottom = top + _componentView.Height;
				_componentView.Top = top;
				_componentView.Visibility = _recyclerView.WindowVisibility;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable.Dispose();
				_helper = null;
				_disposable = null;
				_componentView = null;
				_recyclerView = null;
			}
			base.Dispose(disposing);
		}
	}
}
