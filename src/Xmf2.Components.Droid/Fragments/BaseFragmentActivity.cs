using System;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;
using System.Threading.Tasks;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Xmf2.Components.Events;
using Xmf2.Core.Subscriptions;
using Xmf2.Core.Droid.Permissions;
using Xmf2.Components.Droid.Events;
using Xmf2.Components.Bootstrappers;
using Xmf2.Components.Droid.Navigations;
using Fragment = Android.Support.V4.App.Fragment;

namespace Xmf2.Components.Droid.Fragments
{
	public abstract class BaseFragmentActivity : AppCompatActivity, IFragmentActivity, IPermissionHandlingActivity
	{
		protected Xmf2Disposable Disposables = new Xmf2Disposable();

		public abstract int FragmentContainerId { get; }

		protected abstract int LayoutId { get; }

		protected Fragment CurrentFragment { get; private set; }

		protected BaseFragmentActivity(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public BaseFragmentActivity() { }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(LayoutId);

			new EventSubscriber<FragmentManager>(
				SupportFragmentManager,
				fragManager => fragManager.BackStackChanged += OnBackStackChanged,
				fragManager => fragManager.BackStackChanged -= OnBackStackChanged
			).DisposeEventWith(Disposables);

			if (Intent?.Extras?.ContainsKey(NavigationPresenter.FRAGMENT_START_PARAMETER_CODE) ?? false)
			{
				string navigationKey = Intent.Extras.GetString(NavigationPresenter.FRAGMENT_START_PARAMETER_CODE);
				IDeferredNavigationAction deferredNavigationAction = NavigationParameterContainer.GetDeferredNavigationAction(navigationKey);
				deferredNavigationAction.Execute(this);
			}
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			BaseApplicationBootstrapper.StaticServices.Resolve<IGlobalEventBus>().Publish(new ConfigurationChangedEvent(newConfig: newConfig));
		}

		protected virtual void OnBackStackChanged(object sender, EventArgs eventArgs)
		{
			CurrentFragment = SupportFragmentManager.GetTopFragment();
		}

		public override void OnBackPressed()
		{
			if (CurrentFragment is IBackFragment frag)
			{
				frag.BackPressed();
			}
		}

		#region IPermissionHandlingActivity

		public Task<Permission[]> WaitForPermission(int code) => PermissionContainer.WaitForPermission(code);

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
		{
			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			PermissionContainer.OnResult(requestCode, grantResults);
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Disposables.Dispose();
				Disposables = null;
				CurrentFragment = null;
			}

			base.Dispose(disposing);
		}
	}
}