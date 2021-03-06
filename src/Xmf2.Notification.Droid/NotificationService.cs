﻿using System;
using Firebase;
using Android.Content;
using Android.Gms.Common;
using Xmf2.Core.Services;
using System.Threading.Tasks;

namespace Xmf2.Notification.Droid
{
	public class NotificationService : BaseNotificationService
	{
		private readonly string _gcmId;

		private readonly object _initializeLock = new object();
		private bool _initialized = false;
		private FirebaseApp _app = null;

		private readonly Context _context;
		private readonly IDroidNotificationConstants _constants;

		public NotificationService(string gcmId, Context applicationContext, IKeyValueStorageService settingsService, INotificationDataService notificationDataService, IDroidNotificationConstants constants) : base(settingsService, notificationDataService)
		{
			_gcmId = gcmId;
			_context = applicationContext;
			_constants = constants;
		}

		protected override DeviceType Device => DeviceType.Android;

		protected override void DeleteRegisterId()
		{
			Task.Run(() =>
			{
				try
				{
					//*
					EnsureInitialized();
					Firebase.Iid.FirebaseInstanceId.Instance.DeleteInstanceId();
					// */
				}
				catch (Exception ex) { }
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
				EnsureInitialized();
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
					if (_constants != null)
					{
						FirebaseOptions options = new FirebaseOptions.Builder()
							.SetApplicationId(_constants.AppId)
							.SetApiKey(_constants.ApiKey)
							.SetGcmSenderId(_constants.GcmId)
							.SetProjectId(_constants.ProjectId)
							.Build();

						_app = FirebaseApp.InitializeApp(_context, options);
					}
					else
					{
						//23/06/2020 vju : si cette ligne ne fonctionne pas, il faut passer par les constantes pour indiquer les options
						//en effet la cible GoogleServiceJson peut ne pas fonctionner dans la version 60.1142.1
						_app = FirebaseApp.InitializeApp(_context);
					}

					_initialized = true;
					Android.Util.Log.Info("Xmf2/Token", "Firebase app initialized !");
					Android.Util.Log.Info("Xmf2/Token", $"Has instance ? => {(Firebase.FirebaseApp.Instance != null ? "true" : "false")}");
				}
				catch (Exception ex)
				{
					if(ex.Message.Contains("FirebaseApp name [DEFAULT] already exists!"))
					{
						_initialized = true;
					}
					else
					{
						Android.Util.Log.Error("Xmf2/Token", $"Exception while initializing firebase app {ex}");
						_initialized = false;
					}
				}

				return _app;
			}
		}
	}
}