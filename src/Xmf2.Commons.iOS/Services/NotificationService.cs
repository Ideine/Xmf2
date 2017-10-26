using System;
using Foundation;
using UIKit;
using UserNotifications;
using Xmf2.Commons.Services;

namespace Xmf2.Commons.iOS.Services
{
	public class NotificationService : BaseNotificationService
	{
		public NotificationService(IKeyValueStorageService settingsService, INotificationDataService notificationDataService) : base(settingsService, notificationDataService)
		{
		}

		protected override DeviceType Device => DeviceType.iOS;

		protected override void RequestToken()
		{
			UIApplication.SharedApplication.InvokeOnMainThread(() =>
			{
				UIApplication.SharedApplication.RegisterUserNotificationSettings(UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null));
			});
		}
	}

	public abstract class NotificationAppDelegate : UIApplicationDelegate
	{
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
				UNUserNotificationCenter.Current.Delegate = new LocalNotificationDelegate(DeeplinkFromNotification);
			}

			if (launchOptions != null)
			{
				HandleLaunchOptions(launchOptions);
			}

			return true;
		}

		private void HandleLaunchOptions(NSDictionary launchOptions)
		{
			if (launchOptions == null)
			{
				return;
			}

			if (!UIDevice.CurrentDevice.CheckSystemVersion(10, 0)) // iOS < 10.0
			{
				if (launchOptions.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey)
				   && launchOptions[UIApplication.LaunchOptionsLocalNotificationKey] is UILocalNotification notification)
				{
					if (notification.UserInfo != null)
					{
						DeeplinkFromNotification(notification.UserInfo);
					}
				}
			}

			if (launchOptions.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey)
			   && launchOptions[UIApplication.LaunchOptionsRemoteNotificationKey] is NSDictionary userInfo)
			{
				if (userInfo != null)
				{
					DeeplinkFromNotification(userInfo);
				}
			}
		}

		public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
		{
			if (notification.UserInfo != null)
			{
				DeeplinkFromNotification(notification.UserInfo);
			}
		}

		protected virtual void DeeplinkFromNotification(NSDictionary userInfo)
		{

		}

		protected virtual void BackroundProcessFromNotification(NSDictionary userInfo)
		{

		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			NotificationService?.SetToken(TokenToString(deviceToken));
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to register for remote notifications: {error}");
			NotificationService?.SetToken(null);
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			if (application.ApplicationState != UIApplicationState.Active)
			{
				BackroundProcessFromNotification(userInfo);
				return;
			}

			NSString apsKey = new NSString("aps");
			NSString alertKey = new NSString("alert");
			NSString locArgsKey = new NSString("loc-args");
			NSString locKeyKey = new NSString("loc-key");

			if (userInfo.ContainsKey(apsKey) && userInfo[apsKey] is NSDictionary aps)
			{
				string alert = string.Empty;

				if (aps.ContainsKey(alertKey))
				{
					if (aps[alertKey] is NSString alertText)
					{
						alert = alertText?.ToString();
					}
					else if (aps[alertKey] is NSDictionary alertDictionary)
					{
						NSString localizedKey = null;
						if (alertDictionary.ContainsKey(locKeyKey) && alertDictionary[locKeyKey] is NSString rawLocKey)
						{
							localizedKey = rawLocKey;
						}
						if (localizedKey != null && alertDictionary.ContainsKey(locArgsKey) && alertDictionary[locArgsKey] is NSArray args)
						{
							string format = NSBundle.MainBundle.LocalizedString(localizedKey, null) ?? localizedKey;
							alert = NSString.LocalizedFormat(format, args).ToString();
						}
					}
				}

				//Manually show an alert
				if (!string.IsNullOrEmpty(alert))
				{
					ShowNotification(alert, userInfo);
				}
			}
		}

		protected virtual void ShowNotification(string alert, NSDictionary userInfo)
		{
			ShowLocalNotification(alert, userInfo);
		}

		protected void ShowLocalNotification(string text, NSDictionary userInfo)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				var notificationCenter = UNUserNotificationCenter.Current;
				var notification = new UNMutableNotificationContent
				{
					Body = text,
					Sound = UNNotificationSound.Default,
					UserInfo = userInfo
				};

				var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
				var notificationRequest = UNNotificationRequest.FromIdentifier(new Guid().ToString(), notification, trigger);

				notificationCenter.AddNotificationRequest(notificationRequest, (err) =>
				{
					System.Diagnostics.Debug.WriteLine($"LocalNotificationError: Code={err.Code} / Description={err.Description} / FailureReason={err.LocalizedFailureReason}");
				});
			}
			else
			{
				UILocalNotification notification = new UILocalNotification
				{
					AlertBody = text,
					SoundName = UILocalNotification.DefaultSoundName,
					UserInfo = userInfo
				};
				UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
			}
		}

		protected abstract INotificationService NotificationService { get; }

		private string TokenToString(NSData deviceToken)
		{
			string deviceTokenString = deviceToken.Description;
			deviceTokenString = deviceTokenString.Trim('<', '>');
			deviceTokenString = deviceTokenString.Replace(" ", "");
			deviceTokenString = deviceTokenString.ToUpper();

			return deviceTokenString;
		}

		public class LocalNotificationDelegate : UNUserNotificationCenterDelegate
		{
			private Action<NSDictionary> _notificationCallback;

			public LocalNotificationDelegate(Action<NSDictionary> notificationCallback)
			{
				_notificationCallback = notificationCallback;
			}

			protected LocalNotificationDelegate(NSObjectFlag t) : base(t)
			{
			}

			protected internal LocalNotificationDelegate(IntPtr handle) : base(handle)
			{
			}

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
				completionHandler(UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Badge);
			}
		}
	}
}