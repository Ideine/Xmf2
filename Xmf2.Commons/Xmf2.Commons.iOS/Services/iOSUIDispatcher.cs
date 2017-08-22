using System;
using UIKit;
using Xmf2.Commons.Services;

namespace Xmf2.Commons.iOS.Services
{
	public class iOSUIDispatcher : IUIDispatcher
	{
		public void OnMainThread(Action action)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(action);
		}
	}
}
