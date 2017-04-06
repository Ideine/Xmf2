using Android.App;
using Firebase.Iid;
using MvvmCross.Platform;
using Xmf2.Commons.MvxExtends.Services;

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
			INotificationService result;
			Mvx.TryResolve(out result);
			return result;
		}
	}
}
