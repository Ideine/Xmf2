using Android.App;
using Firebase.Iid;
using Splat;
using Xmf2.Commons.Services;

namespace Xmf2.Notification.Droid
{
	[Service, IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
	public class MyFirebaseInstanceIdService : FirebaseInstanceIdService
	{
		public override void OnTokenRefresh()
		{
			var token = FirebaseInstanceId.Instance.Token;
			GetNotificationService()?.SetToken(token);
		}

		private INotificationService GetNotificationService()
		{
			return Locator.Current.GetService<INotificationService>();
		}
	}
}
