using System;
using Android.OS;
using Xmf2.Core.Services;

namespace Xmf2.Core.Droid.Services
{
	public class UIDispatcher : IUIDispatcher
	{
		private readonly Handler _handler;

		public UIDispatcher()
		{
			_handler = new Handler(Looper.MainLooper);
		}

		public void OnMainThread(Action action)
		{
			_handler.Post(action);
		}
	}
}
