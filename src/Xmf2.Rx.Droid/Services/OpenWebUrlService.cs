using System;
using Android.Content;
using Xmf2.Commons.Droid.Services;
using Xmf2.Commons.Services;
using Xmf2.Rx.Helpers;

namespace Xmf2.Rx.Droid.Services
{
	public class OpenWebUrlService: IOpenWebUrlService
	{
		private readonly LazyLocatorOf<ICurrentActivity> _currentActivty = new LazyLocatorOf<ICurrentActivity>();

		public void OpenWebsite(string url)
		{
			var intent = new Intent(Intent.ActionView);
			intent.SetData(Android.Net.Uri.Parse(url));
			_currentActivty.Value.Activity.StartActivity(intent);
		}
	}
}
