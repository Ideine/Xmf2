using UIKit;
using Xmf2.Core.Services;
using UserNotifications;
using Foundation;

namespace Xmf2.Core.iOS.Services
{
	public class NotificationService : BaseNotificationService
	{
		public NotificationService(IKeyValueStorageService settingsService, INotificationDataService notificationDataService) : base(settingsService, notificationDataService) { }

		protected override DeviceType Device => DeviceType.iOS;

		protected override void RequestToken()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
#pragma warning disable XI0002 // It's ok, because check targeted version is checked.
				UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, CompletionHandler);
#pragma warning restore XI0002
			}
			else
			{
				UIApplication.SharedApplication.InvokeOnMainThread(() =>
				{
#pragma warning disable XI0003 // C'est ok car on utilise cette API déprécié que si c'est nécessaire.
					var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);
					UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
#pragma warning restore XI0003
				});
			}
		}

		private static void CompletionHandler(bool granted, NSError err)
		{
			if (err != null)
			{
				System.Diagnostics.Debug.WriteLine($"LocalNotificationError: Code={err.Code} / Description={err.Description} / FailureReason={err.LocalizedFailureReason}");
			}
		}
	}
}