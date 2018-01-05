using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Firebase.Messaging;

namespace Xmf2.Notification.Droid
{
	public interface INotificationDisplayService
	{
		void ShowNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, string content, string deeplinkData);
	}

	public abstract class BaseNotificationDisplayService : INotificationDisplayService
	{
		public virtual void ShowNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, string content, string deeplinkData)
		{
			Random random = new Random(DateTime.Now.Millisecond);
			var notificationIntent = IntentForNotification(context, notification, content);
			notificationIntent.AddFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);
			System.Diagnostics.Debug.WriteLine($"Deeplink data from notification: {deeplinkData}");
			notificationIntent.PutExtra("deeplink", deeplinkData);
			PendingIntent notificationContentIntent = PendingIntent.GetActivity(context, random.Next(1_000_000_000), notificationIntent, 0);

			NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
				.SetStyle(new NotificationCompat.BigTextStyle().BigText(content))
				.SetContentText(content)
				.SetAutoCancel(true)
				.SetContentIntent(notificationContentIntent)
				.SetDefaults((int)NotificationDefaults.Vibrate)
				.SetLights(0x7F00FF00, 100, 25)
				;
			BuildNotification(context, builder, notification, content, deeplinkData);

			// Finally publish the notification
			NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
			notificationManager.Notify(DateTime.Now.Millisecond, builder.Build());
		}

		protected abstract Intent IntentForNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, string content);

		protected abstract void BuildNotification(FirebaseMessagingService context, NotificationCompat.Builder builder, RemoteMessage.Notification notification, string content, string deeplinkData);
	}
}
