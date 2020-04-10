using System;
using Android.Runtime;
using Android.Views;
using Xmf2.Core.Droid.Helpers;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.List
{
	public class StickyView: View
	{
		private Xmf2Disposable _disposable = new Xmf2Disposable();

		private View _bindedView;

		protected StickyView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public StickyView(View bindedView) : base(bindedView.Context)
		{
			_bindedView = bindedView;

			GlobalLayoutHelper layoutHelper = new GlobalLayoutHelper(_bindedView, OnGlobalLayout).DisposeWith(_disposable);

			new EventSubscriber<View>(
				_bindedView,
				view => view.ViewTreeObserver.AddOnGlobalLayoutListener(layoutHelper),
				view => view.ViewTreeObserver.RemoveOnGlobalLayoutListener(layoutHelper)
			).DisposeWith(_disposable);
		}

		public void OnGlobalLayout()
		{
			var param = LayoutParameters;
			if (param != null && _bindedView != null && param.Height != _bindedView.Height)
			{
				param.Height = _bindedView.Height;
				LayoutParameters = param;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable.Dispose();
				_disposable = null;

				_bindedView = null;
			}

			base.Dispose(disposing);
		}
	}
}
