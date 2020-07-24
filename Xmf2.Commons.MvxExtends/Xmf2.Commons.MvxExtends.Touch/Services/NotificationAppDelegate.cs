using System;
using System.Diagnostics;
using Foundation;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.ViewModels;
using UIKit;
using Xmf2.Commons.Services;
using Xmf2.iOS.Extensions.Extensions;

namespace Xmf2.Commons.MvxExtends.Touch.Services
{
	public abstract class NotificationAppDelegate<TSetup, TApp> : MvxApplicationDelegate<TSetup, TApp>
		where TSetup : MvxIosSetup<TApp>, new()
		where TApp : class, IMvxApplication, new()
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

		#region Register

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			NotificationService?.SetToken(deviceToken.ConcatToString());
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			Debug.WriteLine($"Failed to register for remote notifications: {error}");
			NotificationService?.SetToken(null);
		}

		#endregion

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{
			HandleRemoteNotification(application, userInfo, "normal");
		}

		public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
		{
			HandleRemoteNotification(application, userInfo, "background");
			completionHandler(UIBackgroundFetchResult.NewData);
		}

		protected abstract void HandleRemoteNotification(UIApplication application, NSDictionary userInfo, string from);
	}
}