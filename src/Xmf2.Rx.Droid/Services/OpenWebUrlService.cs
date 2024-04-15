using Android.Content;
using Android.Net;
using Splat;
using Xmf2.Commons.Droid.Services;
using Xmf2.Commons.Services;

namespace Xmf2.Rx.Droid.Services
{
	public class OpenWebUrlService : IOpenWebUrlService
    {
        public void Open(string url)
        {
            var currentActivity = Locator.Current.GetService<ICurrentActivity>().Activity;

            var intent = new Intent(Intent.ActionView);
            intent.SetData(Uri.Parse(url));
            currentActivity.StartActivity(intent);
        }
    }
}