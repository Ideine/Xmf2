using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Util;
using Firebase.Iid;
using Firebase.Messaging;
using Splat;

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
					else if (message.Data.ContainsKey("message"))
					{
						content = message.Data["message"];
					}

					if (content != null)
					{
						InitializeSetup(ApplicationContext);

						try
						{
							Locator.Current.GetService<INotificationDisplayService>()
								.ShowNotification(this, notification, content);
						}
						catch (Exception e)
						{
							Log.Wtf("Xmf2/Notification", $"Exception while trying to display notification from content: {e.Message} {e.StackTrace}");
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

		private void InitializeSetup(Context applicationContext)
		{
			var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
						from type in assembly.GetTypes()
						where type.Name == "Bootstrapper"
						where typeof(INotificationSetup).IsAssignableFrom(type)
						select type;

			Type setupType = query.FirstOrDefault();

			if (setupType == null)
			{
				return;
			}

			INotificationSetup setup = Activator.CreateInstance(setupType, applicationContext) as INotificationSetup;
			setup.Initialize();
		}
	}
}
