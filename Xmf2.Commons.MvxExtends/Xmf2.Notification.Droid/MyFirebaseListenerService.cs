using System;
using System.Linq;
using Android.App;
using Android.Util;
using Firebase.Iid;
using Firebase.Messaging;
using MvvmCross.Droid.Platform;
using MvvmCross.Platform;
using Xmf2.Commons.MvxExtends.Services;

namespace Xmf2.Notification.Droid
{
	[Service, IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
	public class MyFirebaseListenerService : FirebaseMessagingService
	{
		public override void OnMessageReceived(RemoteMessage message)
		{
			base.OnMessageReceived(message);

			try
			{
				Log.Debug("Xmf2/Notification", $"Receive remote message {message}");
				if (message != null)
				{
					Log.Debug("Xmf2/Notification", $"Message.MessageId {message.MessageId}");
					Log.Debug("Xmf2/Notification", $"Message.MessageType {message.MessageType}");
					Log.Debug("Xmf2/Notification", $"Message.SentTime {message.SentTime}");
					Log.Debug("Xmf2/Notification", $"Message.Data {string.Join(", ", message.Data.Select(x => $"{x.Key}={x.Value}"))}");

					RemoteMessage.Notification notification = message.GetNotification();

					string content = null;
					if (notification != null)
					{
						Log.Debug("Xmf2/Notification", $"Notification.Body {notification.Body}");
						Log.Debug("Xmf2/Notification", $"Notification.BodyLocalizationKey {notification.BodyLocalizationKey}");
						Log.Debug("Xmf2/Notification", $"Notification.ClickAction {notification.ClickAction}");
						Log.Debug("Xmf2/Notification", $"Notification.Color {notification.Color}");
						Log.Debug("Xmf2/Notification", $"Notification.Icon {notification.Icon}");
						Log.Debug("Xmf2/Notification", $"Notification.Sound {notification.Sound}");
						Log.Debug("Xmf2/Notification", $"Notification.Tag {notification.Tag}");
						Log.Debug("Xmf2/Notification", $"Notification.Title {notification.Title}");
						Log.Debug("Xmf2/Notification", $"Notification.TitleLocalizationKey {notification.TitleLocalizationKey}");

						content = notification.Body;
					}
					else if(message.Data.ContainsKey("message"))
					{
						content = message.Data["message"];
					}

					if (content != null)
					{
						var setupSingleton = MvxAndroidSetupSingleton.EnsureSingletonAvailable(ApplicationContext);
						setupSingleton.EnsureInitialized();

						try
						{
							Mvx.Resolve<INotificationDisplayService>().ShowNotification(this, notification, content);
						}
						catch (Exception e)
						{
							Log.Wtf("Geotraceur/Notification", $"Exception while trying to display notification from content: {e.Message} {e.StackTrace}");
						}
					}
					else
					{
						Log.Wtf("Xmf2/Notification", "Receive a notification without content !");
					}
				}
			}
			catch (Exception e)
			{
				Log.Wtf("Xmf2/Notification", $"Exception(general) while trying to display notification: {e.Message} {e.StackTrace}");
			}
		}
	}
}
