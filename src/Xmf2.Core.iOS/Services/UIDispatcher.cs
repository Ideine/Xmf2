using System;
using UIKit;
using Xmf2.Core.Services;

namespace Xmf2.Core.iOS.Services
{
	public class UIDispatcher : IUIDispatcher
	{
		public void OnMainThread(Action action)
		{
			UIApplication.SharedApplication.InvokeOnMainThread(action);
		}
	}
}
