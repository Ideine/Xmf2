using System;
using Android.App;
using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Common;
using Xmf2.Commons.Services;
using Firebase;

namespace Xmf2.Notification.Droid
{
	public class NotificationService : BaseNotificationService
	{
		private readonly string _gcmId;

		private readonly object _initializeLock = new object();
		private bool _initialized = false;
		private FirebaseApp _app = null;

		private Context _context;

		public NotificationService(string gcmId, Context applicationContext, IKeyValueStorageService settingsService, INotificationDataService notificationDataService) : base(settingsService, notificationDataService)
		{
			_gcmId = gcmId;
			_context = applicationContext;
		}

		protected override DeviceType Device => DeviceType.Android;

		protected override void DeleteRegisterId()
		{
			Task.Run(() =>
			{
				try
				{
					//*
					//EnsureInitialized();
					Firebase.Iid.FirebaseInstanceId.Instance.DeleteInstanceId();
					// */
				}
				catch (Exception ex)
				{

				}
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

		private void PickToken()
		{
			try
			{
				if (!IsNotificationAvailable())
				{
					SetToken(null);
					return;
				}

				//*
				//EnsureInitialized();
				string token = Firebase.Iid.FirebaseInstanceId.Instance.GetToken(_gcmId, Firebase.Messaging.FirebaseMessaging.InstanceIdScope);
				// */

				Android.Util.Log.Warn("Xmf2/Token", $"PickToken : {token}");

				SetToken(token);
			}
			catch (Exception ex)
			{
				Android.Util.Log.Error("Xmf2/Token", $"Can not get token from firebase {ex}");
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

				Android.Util.Log.Error("Xmf2/Token", $"Initialize firebase app with token {_gcmId}");
				try
				{
					_app = FirebaseApp.InitializeApp(_context);
					Android.Util.Log.Error("Xmf2/Token", $"Firebase app initialized !");
					Android.Util.Log.Error("Xmf2/Token", $"Has instance ? => {(Firebase.FirebaseApp.Instance != null ? "true" : "false")}");
				}
				catch (Exception ex)
				{
					Android.Util.Log.Error("Xmf2/Token", $"Exception while initializing firebase app {ex}");
				}
				_initialized = true;
				return _app;
			}
		}
	}
}
