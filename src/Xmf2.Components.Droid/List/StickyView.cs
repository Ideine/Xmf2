using System;
using Android.Runtime;
using Android.Views;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.List
{
	public class StickyView : View
	{
		private Xmf2Disposable _disposable = new Xmf2Disposable();

		private View _boundView;

		protected StickyView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public StickyView(View boundView) : base(boundView.Context)
		{
			_boundView = boundView;

			GlobalLayoutHelper layoutHelper = new GlobalLayoutHelper(_boundView, OnGlobalLayout).DisposeWith(_disposable);

			new EventSubscriber<View>(
				_boundView,
				view => view.ViewTreeObserver.AddOnGlobalLayoutListener(layoutHelper),
				view => view.ViewTreeObserver.RemoveOnGlobalLayoutListener(layoutHelper)
			).DisposeWith(_disposable);
		}

		public void OnGlobalLayout()
		{
			ViewGroup.LayoutParams param = LayoutParameters;
			if (param != null && _boundView != null && param.Height != _boundView.Height)
			{
				param.Height = _boundView.Height;
				LayoutParameters = param;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable.Dispose();
				_disposable = null;

				_boundView = null;
			}

			base.Dispose(disposing);
		}
	}
}