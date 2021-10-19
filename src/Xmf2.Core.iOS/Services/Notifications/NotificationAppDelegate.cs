using System;
using System.Diagnostics;
using System.Linq;
using Foundation;
using UIKit;
using UserNotifications;
using Xmf2.Core.Services;
using Xmf2.iOS.Extensions.Extensions;

namespace Xmf2.Core.iOS.Services.Notifications
{
	public abstract class NotificationAppDelegate : UIApplicationDelegate
	{
		protected abstract INotificationService NotificationService { get; }

		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			if (notificationSettings.Types != UIUserNotificationType.None)
			{
				application.RegisterForRemoteNotifications();
			}
			else
			{
				NotificationService?.SetToken(null);
			}
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				UNUserNotificationCenter.Current.Delegate = new NotificationCenterDelegate(DeeplinkFromNotification);
			}

			if (launchOptions != null)
			{
				HandleLaunchOptions(launchOptions);
			}

			return true;
		}

		/// <summary>
		/// Appelé quand l'app est
		/// Informe l'appli qu'une notif est arrivé
		/// Apparemment c'est aussi lancé quand on clic sur une notif !!!
		/// </summary>
		/// <see cref="https://developer.apple.com/documentation/uikit/uiapplicationdelegate/1622930-application"/>
		/// <remarks>
		/// Deprecated depuis iOS 10
		/// Pour retrocompat iOS 9
		/// </remarks>
		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			//Si ApplicationState est active c'est que l'appli est en train de run
			//Si c'est inactive c'est que l'appli est en background (donc on est passé par le centre de notif)
			if (notification.UserInfo != null && application.ApplicationState == UIApplicationState.Inactive)
			{
				DeeplinkFromNotification(notification.UserInfo);
			}
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			UpdateBadgeFromUserInfo(userInfo);
			if (application.ApplicationState != UIApplicationState.Active)
			{
				HandleDataUpdate(userInfo);
				return;
			}

			(string notificationTitle, string notificationMessage) = GetNotificationTitleAndMessage(userInfo);

			//Manually show an alert
			if (!string.IsNullOrEmpty(notificationMessage) || !string.IsNullOrEmpty(notificationTitle))
			{
				ShowNotification(notificationTitle, notificationMessage, userInfo);
			}
			else
			{
				HandleDataUpdate(userInfo);
			}
		}

		protected static void UpdateBadgeFromUserInfo(NSDictionary userInfo)
		{
			var apsKey = new NSString("aps");
			//badge key
			var badgeKey = new NSString("badge");

			if (userInfo is null)
			{
				return;
			}

			if (userInfo.TryGet(apsKey, out NSDictionary aps) && aps.TryGet(badgeKey, out NSNumber badgeCountNumber))
			{
				int badgeCount = badgeCountNumber.Int32Value;
				UIApplication.SharedApplication.InvokeOnMainThread(() =>
				{
					//do not inline
					UIApplication.SharedApplication.ApplicationIconBadgeNumber = badgeCount;
				});
			}
		}

		#region Register

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			NotificationService?.SetToken(TokenToString(deviceToken));
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			Debug.WriteLine($"Failed to register for remote notifications: {error}");
			NotificationService?.SetToken(null);
		}

		#endregion Register

		protected void DeeplinkFromNotification(NSDictionary userInfo)
		{
			string content;
			using (var writer = new JsonNotificationWriter())
			{
				if (userInfo.TryGet("data", out NSObject dataValue))
				{
					writer.WriteObject(dataValue);
				}
				else
				{
					writer.WriteObject(userInfo);
				}

				content = writer.ToString();
			}

			HandleDeeplink(content);
		}

		private void HandleLaunchOptions(NSDictionary launchOptions)
		{
			if (launchOptions == null)
			{
				return;
			}

			if (!UIDevice.CurrentDevice.CheckSystemVersion(10, 0)) // iOS < 10.0
			{
				if (launchOptions.TryGet(UIApplication.LaunchOptionsLocalNotificationKey, out UILocalNotification notification)
				    && notification.UserInfo != null)
				{
					DeeplinkFromNotification(notification.UserInfo);
				}
			}

			//inférieur à 12
			//sur 12 on reçoit un ping dans NotificationCenterDelegate
			if (!UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
			{
				if (launchOptions.TryGet(UIApplication.LaunchOptionsRemoteNotificationKey, out NSDictionary userInfo))
				{
					DeeplinkFromNotification(userInfo);
				}
			}
		}

		protected (string notificationTitle, string notificationMessage) GetNotificationTitleAndMessage(NSDictionary userInfo)
		{
			const string apsKey = "aps";
			const string alertKey = "alert";
			const string titleKey = "title";
			const string contentKey = "body";
			const string localizedTitleKey = "title-loc-key";
			const string localizedTitleArgsKey = "title-loc-args";
			const string localizedContentKey = "loc-key";
			const string localizedContentArgsKey = "loc-args";

			string notificationMessage = null;
			string notificationTitle = null;

			if (userInfo.TryGet(apsKey, out NSDictionary aps))
			{
				if (aps.TryGet(alertKey, out NSString alertText))
				{
					notificationMessage = alertText;
				}
				else if (aps.TryGet(alertKey, out NSDictionary alertDictionary))
				{
					if (alertDictionary.TryGet(titleKey, out NSString title))
					{
						notificationTitle = title.ToString();
					}

					if (alertDictionary.TryGet(contentKey, out NSString content))
					{
						notificationMessage = content.ToString();
					}

					if (alertDictionary.TryGet(localizedTitleKey, out NSString localizedTitle))
					{
						notificationTitle = NSBundle.MainBundle.GetLocalizedString(localizedTitle) ?? notificationTitle ?? localizedTitle;
						if (alertDictionary.TryGet(localizedTitleArgsKey, out NSArray localizedTitleArgs))
						{
							notificationTitle = NSString.LocalizedFormat(notificationTitle, ToNSObjects(localizedTitleArgs));
						}
					}

					if (alertDictionary.TryGet(localizedContentKey, out NSString localizedContent))
					{
						notificationMessage = NSBundle.MainBundle.GetLocalizedString(localizedContent) ?? notificationMessage ?? localizedContent;
						if (alertDictionary.TryGet(localizedContentArgsKey, out NSArray localizedContentArgs))
						{
							notificationMessage = NSString.LocalizedFormat(notificationMessage, ToNSObjects(localizedContentArgs));
						}
					}
				}
			}

			return (notificationTitle, notificationMessage);
		}

		protected virtual void HandleDeeplink(string jsonData) { }

		protected virtual void HandleDataUpdate(NSDictionary userInfo) { }

		public virtual void ShowNotification(string title, string message, NSDictionary userInfo)
			=> ShowLocalNotification(title, message, userInfo);

		protected void ShowLocalNotification(string title, string message, NSDictionary userInfo)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				var notification = new UNMutableNotificationContent
				{
					Title = title,
					Body = message,
					Sound = UNNotificationSound.Default,
					UserInfo = userInfo
				};

				const double oneSecond = 1;
				var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(oneSecond, false);
				var notificationRequest = UNNotificationRequest.FromIdentifier(new Guid().ToString(), notification, trigger);

				var notificationCenter = UNUserNotificationCenter.Current;
				notificationCenter.AddNotificationRequest(
					request: notificationRequest,
					completionHandler: (err) =>
					{
						if (err != null)
						{
							Debug.WriteLine($"LocalNotificationError: Code={err.Code} / Description={err.Description} / FailureReason={err.LocalizedFailureReason}");
						}
					});
			}
			else
			{
				UIApplication.SharedApplication.InvokeOnMainThread(() =>
				{
					UILocalNotification notification = new UILocalNotification
					{
						AlertTitle = title,
						AlertBody = message,
						SoundName = UILocalNotification.DefaultSoundName,
						UserInfo = userInfo
					};
					UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
				});
			}
		}

		private static string TokenToString(NSData deviceToken)
		{
			return string.Join(string.Empty, deviceToken.Select(x => x.ToString("x2")));
		}

		private static NSObject[] ToNSObjects(NSArray array)
		{
			NSObject[] result = new NSObject[(int)array.Count];
			for (nuint i = 0 ; i < array.Count ; ++i)
			{
				result[i] = array.GetItem<NSObject>(i);
			}

			return result;
		}
	}
}