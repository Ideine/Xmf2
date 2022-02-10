using System.Diagnostics;
using UIKit;
using Foundation;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using Xmf2.Commons.MvxExtends.Services;

namespace Xmf2.Commons.MvxExtends.Touch.Services
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

	public class NotificationAppDelegate : MvxApplicationDelegate
	{
		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			if (notificationSettings.Types != UIUserNotificationType.None)
			{
				application.RegisterForRemoteNotifications();
			}
			else
			{
				GetNotificationService()?.SetToken(null);
			}
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			GetNotificationService()?.SetToken(TokenToString(deviceToken));
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			Debug.WriteLine($"Failed to register for remote notifications: {error}");
			GetNotificationService()?.SetToken(null);
		}

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			if (userInfo.ContainsKey(new NSString("aps")))
			{
				//Get the aps dictionary
				NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

				string alert = string.Empty;

				if (aps.ContainsKey(new NSString("alert")))
				{
					alert = (aps[new NSString("alert")] as NSString).ToString();
				}

				//Manually show an alert
				if (!string.IsNullOrEmpty(alert))
				{
					ShowNotification(alert);
				}
			}
		}

		protected virtual void ShowNotification(string alert)
		{
			UIAlertView notificationAlert = new UIAlertView("Notification", alert, null, "Ok", null);
			notificationAlert.Show();
		}

		private INotificationService GetNotificationService()
		{
			INotificationService result;
			Mvx.IoCProvider.TryResolve(out result);
			return result;
		}

		private string TokenToString(NSData deviceToken)
		{
			string deviceTokenString = deviceToken.Description;
			deviceTokenString = deviceTokenString.Trim('<', '>');
			deviceTokenString = deviceTokenString.Replace(" ", "");
			deviceTokenString = deviceTokenString.ToUpper();

			return deviceTokenString;
		}
	}
}
