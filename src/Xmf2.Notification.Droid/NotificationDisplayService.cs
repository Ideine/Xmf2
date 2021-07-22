using System;
using Android.App;
using Android.Content;
using Firebase.Messaging;
using System.Collections.Generic;
using System.Diagnostics;
using AndroidX.Core.App;

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

		//The background color on notification channel
		private readonly int _backgroundColor;

		public BaseNotificationDisplayService(string channelId, string channelName, string channelDescription, int backgroundColor)
		{
			_channelId = channelId;
			_channelName = channelName;
			_channelDescription = channelDescription;
			_backgroundColor = backgroundColor;
		}

		public virtual void ShowNotification(FirebaseMessagingService context, RemoteMessage.Notification notification, IDictionary<string, string> notificationData, string content)
		{
			int pendingIntentId = (int)(DateTime.Now.Date.Millisecond & 0xFFFFFFF);
			PendingIntent notificationContentIntent = PendingIntent.GetActivity(context, pendingIntentId, IntentForNotification(context, notification, notificationData, content), PendingIntentFlags.OneShot);

			NotificationCompat.Builder builder = new NotificationCompat.Builder(context, _channelId)
				.SetStyle(new NotificationCompat.BigTextStyle().BigText(content))
				.SetContentText(content)
				.SetAutoCancel(true)
				.SetContentIntent(notificationContentIntent)
				.SetDefaults((int)NotificationDefaults.Vibrate)
				.SetLights(0x7F00FF00, 100, 25);

			if (_backgroundColor != -1)
			{
				builder.SetColor(_backgroundColor);
			}

			NotificationManager notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);
			Debug.Assert(notificationManager != null, nameof(notificationManager) + " != null");

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
			{
				NotificationChannel notificationChannel = notificationManager.GetNotificationChannel(_channelId);

				if (notificationChannel == null)
				{
					notificationChannel = new NotificationChannel(_channelId, _channelName, NotificationImportance.Default)
					{
						Description = _channelDescription
					};

					notificationChannel.EnableLights(true);
					// Sets the notification light color for notifications posted to this
					// channel, if the device supports this feature.
					notificationChannel.LightColor = 0x7F00FF00;
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