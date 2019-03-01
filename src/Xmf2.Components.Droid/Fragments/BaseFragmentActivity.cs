using System;
using System.Linq;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Xmf2.Components.Bootstrappers;
using Xmf2.Components.Droid.Events;
using Xmf2.Components.Droid.Services;
using Xmf2.Components.Events;
using Xmf2.Core.Droid.Permissions;
using Xmf2.Core.Subscriptions;
using Fragment = Android.Support.V4.App.Fragment;

namespace Xmf2.Components.Droid.Fragments
{
	public abstract class BaseFragmentActivity : AppCompatActivity, IFragmentActivity, IPermissionHandlingActivity
	{
		protected Xmf2Disposable Disposables = new Xmf2Disposable();

		protected abstract int FragmentContainerId { get; }

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

			if (Intent?.Extras?.ContainsKey(FragmentResolverService.ENTRY_POINT) ?? false)
			{
				var entry = Intent.Extras.GetString(FragmentResolverService.ENTRY_POINT);
				var typefrag = BaseApplicationBootstrapper.StaticServices.Resolve<IFragmentResolverService>().GetType(entry);
				RunOnUiThread(() =>
				{
					var frag = Activator.CreateInstance(typefrag) as Fragment;
					this.ShowFragment(frag, FragmentContainerId, true);
				});
			}

			if (Intent?.Extras?.ContainsKey(FragmentResolverService.LIST_ENTRY_POINT) ?? false)
			{
				var entryArray = Intent.Extras.GetStringArray(FragmentResolverService.LIST_ENTRY_POINT);
				var typeArray = entryArray.Select(BaseApplicationBootstrapper.StaticServices.Resolve<IFragmentResolverService>().GetType).ToArray();

				RunOnUiThread(() =>
				{
					foreach (var type in typeArray)
					{
						var frag = Activator.CreateInstance(type) as Fragment;
						this.ShowFragment(frag, FragmentContainerId, true);
					}
				});
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

		public virtual void ShowFragment<TFragment>(bool addToBackstack = true)
			where TFragment : Fragment, new()
		{
			RunOnUiThread(() => this.ShowFragment(new TFragment(), FragmentContainerId, addToBackstack));
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