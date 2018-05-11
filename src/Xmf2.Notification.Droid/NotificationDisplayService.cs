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
		private readonly string _channelId;

		// The user-visible name of the channel.
		private readonly string _channelName;

		// The user-visible description of the channel.
		private readonly string _channelDescription;

		public BaseNotificationDisplayService(string channelId,string channelName,string channelDescription)
		{
			_channelId = channelId;
			_channelName = channelName;
			_channelDescription = channelDescription;
		}

		public virtual void ShowNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, IDictionary<string, string> notificationData, string content)
		{
			PendingIntent notificationContentIntent = PendingIntent.GetActivity(context, 0, IntentForNotification(context, notification, notificationData, content), 0);

			NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
			NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
					.SetStyle(new NotificationCompat.BigTextStyle().BigText(content))
					.SetContentText(content)
					.SetAutoCancel(true)
					.SetContentIntent(notificationContentIntent)
					.SetDefaults((int)NotificationDefaults.Vibrate)
					.SetLights(0x7F00FF00, 100, 25)
				;

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
			{
				NotificationChannel notificationChannel = notificationManager.GetNotificationChannel(_channelId);
				if (notificationChannel == null)
				{
					notificationChannel = new NotificationChannel(_channelId, _channelName, NotificationImportance.Default);

					// Configure the notification channel.
					notificationChannel.Description = _channelDescription;

					notificationChannel.EnableLights(true);
					// Sets the notification light color for notifications posted to this channel, if the device supports this feature.
					notificationChannel.LightColor = 0x7F00FF00;

					notificationChannel.EnableVibration(true);

					notificationManager.CreateNotificationChannel(notificationChannel);
				}
			}

			BuildNotification(context, builder, notification, notificationData, content);

			// Finally publish the notification
			notificationManager.Notify(DateTime.Now.Millisecond, builder.Build());
		}

		protected abstract Intent IntentForNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, IDictionary<string, string> notificationData, string content);

		protected abstract void BuildNotification(FirebaseMessagingService context, NotificationCompat.Builder builder, RemoteMessage.Notification notification, IDictionary<string, string> notificationData, string content);
	}
}
