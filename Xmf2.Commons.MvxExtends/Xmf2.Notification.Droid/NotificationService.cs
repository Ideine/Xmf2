using System;
using Android.App;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Xmf2.Commons.MvxExtends.Services;
using System.Threading.Tasks;
using Android.Content;
using Android.Gms.Common;

namespace Xmf2.Notification.Droid
{
	public class NotificationService : BaseNotificationService
	{
		private readonly string _gcmId;

		private readonly object _initializeLock = new object();
		private bool _initialized;

		private Activity CurrentActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

		public NotificationService(string gcmId, IKeyValueStorageService settingsService, INotificationDataService notificationDataService) : base(settingsService, notificationDataService)
		{
			_gcmId = gcmId;
		}

		protected override DeviceType Device => DeviceType.Android;

		protected override void DeleteRegisterId()
		{
			Task.Run(() =>
			{
				try
				{
					EnsureInitialized();
					Firebase.Iid.FirebaseInstanceId.Instance.DeleteInstanceId();
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
			Context context = CurrentActivity;

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

				EnsureInitialized();
				string token = Firebase.Iid.FirebaseInstanceId.Instance.GetToken(_gcmId, Firebase.Messaging.FirebaseMessaging.InstanceIdScope);

				Android.Util.Log.Warn("Xmf2/Token", $"PickToken : {token}");

				SetToken(token);
			}
			catch (Exception ex)
			{
				Android.Util.Log.Error("Xmf2/Token", $"Can not get token from firebase {ex}");
			}
		}

		private void EnsureInitialized()
		{
			if(_initialized)
			{
				return;
			}

			lock(_initializeLock)
			{
				if(_initialized)
				{
					return;;
				}

				Firebase.FirebaseApp.InitializeApp(CurrentActivity.Application);
			}
		}
	}
}
