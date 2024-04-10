using System;
using Android.OS;
using Xmf2.Commons.Services;

namespace Xmf2.Commons.Droid.Services
{
	public class DroidUIDispatcher: IUIDispatcher
	{
		private readonly Handler _handler;

		public DroidUIDispatcher()
		{
			_handler = new Handler(Looper.MainLooper);
		}

		public void OnMainThread(Action action)
		{
			_handler.Post(action);
		}
	}
}
