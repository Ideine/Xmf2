using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Common;
using Android.Gms.Extensions;
using Android.Util;
using Firebase;
using Firebase.Installations;
using Firebase.Messaging;
using Xmf2.Commons.Services.Notifications;

namespace Xmf2.Notification.Droid
{
	public class NotificationService : BaseNotificationService
	{
		private readonly string _gcmId;

		private readonly object _initializeLock = new();
		private bool _initialized;
		private FirebaseApp _app;

		private readonly Context _context;

		public NotificationService(string gcmId, Context applicationContext, IKeyValueStorageService settingsService, INotificationDataService notificationDataService) : base(settingsService, notificationDataService)
		{
			_gcmId = gcmId;
			_context = applicationContext;
		}

		protected override DeviceType Device => DeviceType.Android;

		protected override void DeleteRegisterId()
		{
			Task.Run(async () =>
			{
				try
				{
					await FirebaseInstallations.Instance.Delete();
				}
				catch (Exception) { }
			}).ConfigureAwait(false);
		}

		protected override void RequestToken()
		{
			Task.Run(() => PickToken()).ConfigureAwait(false);
		}

		protected virtual bool IsNotificationAvailable()
		{
			Context context = _context;

			if (context == null)
			{
				return false;
			}

			int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(context);
			return resultCode == ConnectionResult.Success;
		}

		private async Task PickToken()
		{
			try
			{
				if (!IsNotificationAvailable())
				{
					SetToken(null);
					return;
				}

				EnsureInitialized();
				string token = (string)await FirebaseMessaging.Instance.GetToken();
				Log.Warn("Xmf2/Token", $"PickToken : {token}");
				SetToken(token);
			}
			catch (Exception ex)
			{
				Log.Error("Xmf2/Token", $"Can not get token from firebase {ex}");
			}
		}

		private FirebaseApp EnsureInitialized()
		{
			if (_initialized)
			{
				return _app;
			}

			lock (_initializeLock)
			{
				if (_initialized)
				{
					return _app;
				}

				Log.Error("Xmf2/Token", $"Initialize firebase app with token {_gcmId}");
				try
				{
					_app = FirebaseApp.InitializeApp(_context);
					_initialized = true;
					Log.Info("Xmf2/Token", "Firebase app initialized !");
					Log.Info("Xmf2/Token", $"Has instance ? => {(FirebaseApp.Instance != null ? "true" : "false")}");
				}
				catch (Exception ex)
				{
					if (ex.Message.Contains("FirebaseApp name [DEFAULT] already exists!"))
					{
						_initialized = true;
					}
					else
					{
						Log.Error("Xmf2/Token", $"Exception while initializing firebase app {ex}");
						_initialized = false;
					}
				}

				return _app;
			}
		}
	}
}