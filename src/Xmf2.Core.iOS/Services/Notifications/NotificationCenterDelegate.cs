using System;
using Foundation;
using UserNotifications;

namespace Xmf2.Core.iOS.Services.Notifications
{
	internal class NotificationCenterDelegate : UNUserNotificationCenterDelegate
	{
		private readonly Action<NSDictionary> _notificationCallback;

		public NotificationCenterDelegate(Action<NSDictionary> notificationCallback)
		{
			_notificationCallback = notificationCallback;
		}

		protected NotificationCenterDelegate(NSObjectFlag t) : base(t) { }

		protected internal NotificationCenterDelegate(IntPtr handle) : base(handle) { }

		/// <summary>
		/// En théorie cette méthode n'est appelé que si l'app est lancé
		/// Sur iOS 12 elle est appelé aussi lorsqu'elle n'est pas lancé (enjoy)
		/// </summary>
		public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
			if (response.IsDismissAction)
			{
				return;
			}

			if (response.IsDefaultAction)
			{
				NSDictionary userInfo = response.Notification?.Request?.Content?.UserInfo;

				if (userInfo == null)
				{
					return;
				}

				_notificationCallback?.Invoke(userInfo);
			}
		}

		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			Console.WriteLine("Active Notification: {0}", notification);
			completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
		}
	}
}