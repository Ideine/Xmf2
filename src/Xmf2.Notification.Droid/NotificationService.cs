﻿using System;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Extensions;
using Android.OS;
using Android.Util;
using Firebase;
using Firebase.Installations;
using Firebase.Messaging;
using Plugin.CurrentActivity;
using Xmf2.Core.Droid.Permissions;
using Xmf2.Core.Services;
using Context = Android.Content.Context;

namespace Xmf2.Notification.Droid
{
	public class NotificationService : BaseNotificationService
	{
		private readonly string _gcmId;

		private readonly object _initializeLock = new();
		private bool _initialized;
		private FirebaseApp _app;

		private readonly Context _context;
		private readonly IDroidNotificationConstants _constants;

		public NotificationService(string gcmId, Context applicationContext, IKeyValueStorageService settingsService, INotificationDataService notificationDataService, IDroidNotificationConstants constants) : base(settingsService, notificationDataService)
		{
			_gcmId = gcmId;
			_context = applicationContext;
			_constants = constants;
		}

		protected override DeviceType Device => DeviceType.Android;

		public override bool CheckIfPermissionForNotificationIsGranted()
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
			{
				Activity activity = CrossCurrentActivity.Current.Activity;
				return activity.CheckSelfPermission(Manifest.Permission.PostNotifications) == Permission.Granted;
			}
			else
			{
				NotificationManager notificationManager = (NotificationManager)_context.GetSystemService(Context.NotificationService);
				return notificationManager.AreNotificationsEnabled();
			}
		}

		public override async Task AskForPermissionIfNeeded(bool showRationale, Func<Task<bool>> onShowRationale)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
			{
				Activity activity = CrossCurrentActivity.Current.Activity;
				if (activity.CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted)
				{
					bool result = true;
					if (showRationale && activity.ShouldShowRequestPermissionRationale(Manifest.Permission.PostNotifications))
					{
						if (onShowRationale != null)
						{
							result = await onShowRationale.Invoke();
						}
						else
						{
							System.Diagnostics.Debug.WriteLine($"Missing {nameof(onShowRationale)} argument");
							result = false;
						}
					}

					if (result)
					{
						const int PERMISSION_CODE = 4041;
						activity.RequestPermissions(new[]
						{
							Manifest.Permission.PostNotifications
						}, PERMISSION_CODE);

						if (activity is IPermissionHandlingActivity permissionHandlingActivity)
						{
							await permissionHandlingActivity.WaitForPermission(PERMISSION_CODE);
						}
					}
				}
			}
		}

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
				System.Diagnostics.Debug.WriteLine($"PickToken : {token}");
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