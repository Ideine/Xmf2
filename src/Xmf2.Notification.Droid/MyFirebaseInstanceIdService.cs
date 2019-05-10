using Android.App;
using Firebase.Iid;
using Xmf2.Core.Services;
using Xmf2.Components.Bootstrappers;

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
			INotificationService notificationService;
			BaseApplicationBootstrapper.StaticServices.TryResolve<INotificationService>(out notificationService);
			return notificationService;
		}
	}
}