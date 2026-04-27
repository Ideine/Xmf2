using System;
using Firebase;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Extensions;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Xmf2.Commons.Services;

namespace Xmf2.Notification.Droid
{
	public class NotificationService : BaseNotificationService
	{
		private Context _context;

		public NotificationService(Context applicationContext, IKeyValueStorageService settingsService, INotificationDataService notificationDataService) : base(settingsService, notificationDataService)
		{
			_context = applicationContext;
		}

		private const string PostNotificationsPermission = "android.permission.POST_NOTIFICATIONS";
		private const int PermissionRequestCode = 1001;
		private TaskCompletionSource<bool> _permissionTcs;

		protected override DeviceType Device => DeviceType.Android;

		public override Task AskForPermissionIfNeeded(bool showRationale, Func<Task<bool>> onShowRationale)
		{
			if (!OperatingSystem.IsAndroidVersionAtLeast(33))
			{
				return Task.CompletedTask;
			}

			if (_context.CheckSelfPermission(PostNotificationsPermission) == Permission.Granted)
			{
				return Task.CompletedTask;
			}

			var activity = Platform.CurrentActivity;
			if (activity == null)
			{
				Android.Util.Log.Error("Xmf2/Permissions", "Cannot request notification permission: Platform.CurrentActivity is null");
				return Task.CompletedTask;
			}

			_permissionTcs = new TaskCompletionSource<bool>();

			activity.RunOnUiThread(() =>
			{
				activity.RequestPermissions(new[] { PostNotificationsPermission }, PermissionRequestCode);
			});

			return _permissionTcs.Task;
		}

		public void OnPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			if (requestCode == PermissionRequestCode)
			{
				_permissionTcs?.TrySetResult(grantResults.Length > 0 && grantResults[0] == Permission.Granted);
				_permissionTcs = null;
			}
		}

		protected override void DeleteRegisterId()
		{
			Task.Run((Func<Task>)(async () =>
			{
				try
				{
					await Firebase.Messaging.FirebaseMessaging.Instance.DeleteToken().AsAsync();
				}
				catch (Exception ex)
				{
					Android.Util.Log.Error("Xmf2/Token", $"Can not delete token from firebase {ex}");
				}
			})).ConfigureAwait(false);
		}

		protected override void RequestToken()
		{
			Task.Run(async () => await PickTokenAsync()).ConfigureAwait(false);
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

		private async Task PickTokenAsync()
		{
			try
			{
				if (!IsNotificationAvailable())
				{
					SetToken(null);
					return;
				}

				var result = await Firebase.Messaging.FirebaseMessaging.Instance.GetToken().AsAsync<Java.Lang.String>();
				string token = result?.ToString() ?? string.Empty;

				Android.Util.Log.Warn("Xmf2/Token", $"PickToken : {token}");

				SetToken(token);
			}
			catch (Exception ex)
			{
				Android.Util.Log.Error("Xmf2/Token", $"Can not get token from firebase {ex}");
			}
		}
	}
}
