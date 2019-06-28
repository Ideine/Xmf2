using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Firebase.Messaging;

namespace Xmf2.Notification.Droid
{
	public interface INotificationDisplayService
	{
		void ShowNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, IDictionary<string, string> notificationData, string content);
	}

	public abstract class BaseNotificationDisplayService : INotificationDisplayService
	{
		public virtual void ShowNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, IDictionary<string, string> notificationData, string content)
		{
			PendingIntent notificationContentIntent = PendingIntent.GetActivity(context, 0, IntentForNotification(context, notification, notificationData, content), 0);

			NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
				.SetStyle(new NotificationCompat.BigTextStyle().BigText(content))
				.SetContentText(content)
				.SetAutoCancel(true)
				.SetContentIntent(notificationContentIntent)
				.SetDefaults((int)NotificationDefaults.Vibrate)
				.SetLights(0x7F00FF00, 100, 25)
				;
			BuildNotification(context, builder, notification, notificationData, content);

			// Finally publish the notification
			NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
			notificationManager.Notify(DateTime.Now.Millisecond, builder.Build());
		}

		protected abstract Intent IntentForNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, IDictionary<string, string> notificationData, string content);

		protected abstract void BuildNotification(FirebaseMessagingService context, NotificationCompat.Builder builder, RemoteMessage.Notification notification, IDictionary<string, string> notificationData, string content);
	}
}
