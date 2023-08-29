using Foundation;
using WebKit;

namespace Xmf2.Core.iOS.Helpers
{
	public class WKBusyNavigationDelegate : WKNavigationDelegate
	{
		private bool _isBusy;
		private LoadingViewHelper _loadingViewHelper;

		[Export("webView:didFinishNavigation:")]
		public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
		{
			_isBusy = false;
			if (_loadingViewHelper != null)
			{
				_loadingViewHelper.IsBusy = false;
			}
		}

		[Export("webView:didStartProvisionalNavigation:")]
		public override void DidStartProvisionalNavigation(WKWebView webView, WKNavigation navigation)
		{
			_isBusy = true;
			if (_loadingViewHelper != null)
			{
				_loadingViewHelper.IsBusy = true;
			}
		}

		[Export("webView:didFailProvisionalNavigation:")]
		public override void DidFailProvisionalNavigation(WKWebView webView, WKNavigation navigation, NSError error)
		{
			_isBusy = false;
			if (_loadingViewHelper != null)
			{
				_loadingViewHelper.IsBusy = false;
			}
		}

		public override void DidFailNavigation(WKWebView webView, WKNavigation navigation, NSError error)
		{
			_isBusy = false;
			if (_loadingViewHelper != null)
			{
				_loadingViewHelper.IsBusy = false;
			}
		}

		public WKBusyNavigationDelegate WillUpdate(LoadingViewHelper loadingViewHelper)
		{
			_loadingViewHelper = loadingViewHelper;
			_loadingViewHelper.IsBusy = _isBusy;
			return this;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_loadingViewHelper = null;
			}

			base.Dispose(disposing);
		}
	}
}