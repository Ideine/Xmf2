using System;
using System.Linq;
using CoreGraphics;
using ObjCRuntime;
using UIKit;
using Xmf2.Core.iOS.Extensions;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.iOS.Helpers
{//TODO: VJU reprendre Idelink
	public class ScrollIndicatorHelper : IDisposable
	{
		private readonly Xmf2Disposable _disposable = new Xmf2Disposable();

		private UIScrollView _scrollView;
		private Func<CGRect> _getHeaderFrame;
		private bool _stickyHeader;

		private ScrollIndicatorHelper(UIScrollView scrollView, Func<CGRect> getHeaderFrame, bool stickyHeader = false)
		{
			_scrollView = scrollView;
			_getHeaderFrame = getHeaderFrame;
			_stickyHeader = stickyHeader;

			scrollView.ScrollChanged(OnScroll).DisposeEventWith(_disposable);
		}

		private void OnScroll(object sender, EventArgs eventArgs)
		{
			CGRect rect = _getHeaderFrame();
			var scrollTop = _scrollView.Frame.Top;
			var bot = NMath.Max(0f, (rect.Bottom - scrollTop));
			if(_stickyHeader)
			{
				bot = NMath.Max(rect.Height, bot);
			}

			_scrollView.ScrollIndicatorInsets = new UIEdgeInsets(bot, 0, 0, 0);
		}

		#region Dispose

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_disposable.Dispose();

				_scrollView = null;
				_getHeaderFrame = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ScrollIndicatorHelper()
		{
			Dispose(false);
		}

		#endregion

		#region Creator

		public static ScrollIndicatorHelper CreateForStickyHeader(UIScrollView scrollView, UIView stickyView)
		{
			return new ScrollIndicatorHelper(scrollView, () => scrollView.ConvertRectToView(stickyView.Frame, UIApplication.SharedApplication.KeyWindow.RootViewController.View), true);
		}

		public static ScrollIndicatorHelper CreateForBigTitlePage(UIScrollView scrollView, UIView headerView)
		{
			return new ScrollIndicatorHelper(scrollView, () => scrollView.ConvertRectToView(headerView.Frame, UIApplication.SharedApplication.KeyWindow.RootViewController.View));
		}

		public static ScrollIndicatorHelper CreateForBigTitlePage(UIScrollView scrollView, params UIView[] headerViews)
		{
			return new ScrollIndicatorHelper(scrollView, () =>
				headerViews.Select(v => scrollView.ConvertRectToView(v.Frame, UIApplication.SharedApplication.KeyWindow.RootViewController.View))
						   .Aggregate(CGRect.Union)
			);
		}

		public static ScrollIndicatorHelper CreateForParallaxPage(UIScrollView scrollView, UIView headerView)
		{
			return new ScrollIndicatorHelper(scrollView, () => scrollView.ConvertRectToView(headerView.Frame, UIApplication.SharedApplication.KeyWindow.RootViewController.View));
		}

		#endregion
	}
}