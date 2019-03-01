using System;
using System.Diagnostics;
using System.IO;
using Foundation;
using Newtonsoft.Json;
using UIKit;
using UserNotifications;
using Xmf2.Core.iOS.Extensions;
using Xmf2.Core.Services;

namespace Xmf2.Core.iOS.Services
{
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
				if (launchOptions.TryGet(UIApplication.LaunchOptionsLocalNotificationKey, out UILocalNotification notification))
				{
					if (notification.UserInfo != null)
					{
						DeeplinkFromNotification(notification.UserInfo);
					}
				}
			}

			if (launchOptions.TryGet(UIApplication.LaunchOptionsRemoteNotificationKey, out NSDictionary userInfo))
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

		protected void DeeplinkFromNotification(NSDictionary userInfo)
		{
			NSString dataKey = new NSString("data");
			JsonNotificationWriter writer = new JsonNotificationWriter();
			string content = string.Empty;
			if (userInfo.ContainsKey(dataKey))
			{
				writer.WriteObject(userInfo[dataKey]);
				content = writer.ToString();
			}
			else
			{
				writer.WriteObject(userInfo);
				content = writer.ToString();
			}

			HandleDeeplink(content);
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
				HandleDataUpdate(userInfo);
				return;
			}

			NSString apsKey = new NSString("aps");
			NSString alertKey = new NSString("alert");

			//non localized notification
			NSString titleKey = new NSString("title");
			NSString contentKey = new NSString("body");
			//localized parameters
			NSString localizedTitleKey = new NSString("title-loc-key");
			NSString localizedTitleArgsKey = new NSString("title-loc-args");
			NSString localizedContentKey = new NSString("loc-key");
			NSString localizedContentArgsKey = new NSString("loc-args");

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

		public virtual void ShowNotification(string title, string message, NSDictionary userInfo)
		{
			ShowLocalNotification(title, message, userInfo);
		}

		protected virtual void HandleDeeplink(string jsonData) { }

		protected virtual void HandleDataUpdate(NSDictionary userInfo) { }

		protected void ShowLocalNotification(string title, string message, NSDictionary userInfo)
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
			{
				var notificationCenter = UNUserNotificationCenter.Current;
				var notification = new UNMutableNotificationContent
				{
					Title = title,
					Body = message,
					Sound = UNNotificationSound.Default
				};
				if (userInfo != null)
				{
					notification.UserInfo = userInfo;
				}
				const double oneSecond = 1;
				var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(oneSecond, false);
				var notificationRequest = UNNotificationRequest.FromIdentifier(new Guid().ToString(), notification, trigger);

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
						SoundName = UILocalNotification.DefaultSoundName
					};
					if (userInfo != null)
					{
						notification.UserInfo = userInfo;
					}
						UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
				});
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

		private NSObject[] ToNSObjects(NSArray array)
		{
			NSObject[] result = new NSObject[(int)array.Count];
			for (nuint i = 0; i < array.Count; ++i)
			{
				result[i] = array.GetItem<NSObject>(i);
			}

			return result;
		}

		private class LocalNotificationDelegate : UNUserNotificationCenterDelegate
		{
			private readonly Action<NSDictionary> _notificationCallback;

			public LocalNotificationDelegate(Action<NSDictionary> notificationCallback)
			{
				_notificationCallback = notificationCallback;
			}

			protected LocalNotificationDelegate(NSObjectFlag t) : base(t) { }

			protected internal LocalNotificationDelegate(IntPtr handle) : base(handle) { }

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

		public class JsonNotificationWriter
		{
			private readonly StringWriter _textWriter = new StringWriter();
			private readonly JsonWriter _jsonWriter;

			public JsonNotificationWriter(Formatting formatting = Formatting.None)
			{
				_jsonWriter = new JsonTextWriter(_textWriter)
				{
					Formatting = formatting
				};
			}

			public override string ToString() => _textWriter.ToString();

			private void Write(NSDictionary dictionary)
			{
				_jsonWriter.WriteStartObject();

				foreach (NSObject key in dictionary.Keys)
				{
					_jsonWriter.WritePropertyName(key.ToString());
					NSObject value = dictionary[key];

					WriteObject(value);
				}

				_jsonWriter.WriteEndObject();
			}

			private void Write(NSArray array)
			{
				_jsonWriter.WriteStartArray();

				for (nuint i = 0; i < array.Count; ++i)
				{
					try
					{
						NSString value = array.GetItem<NSString>(i);

						WriteObject(value);
					}
					catch (Exception)
					{
						//ignored by design
					}
				}

				_jsonWriter.WriteEndArray();
			}

			public void WriteObject(NSObject value)
			{
				if (value is NSDictionary childDictionary)
				{
					Write(childDictionary);
				}
				else if (value is NSArray childArray)
				{
					Write(childArray);
				}
				else
				{
					_jsonWriter.WriteValue(value.ToString());
				}
			}
		}
	}
}