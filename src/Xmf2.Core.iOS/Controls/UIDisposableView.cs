using UIKit;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Core.iOS.Controls
{
	public class UIDisposableView : UIView
	{
		protected Xmf2Disposable Disposable { get; }

		public UIDisposableView()
		{
			Disposable = new Xmf2Disposable();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Disposable.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
