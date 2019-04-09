using System;
using Android.OS;
using Android.Runtime;
using Android.Content.PM;
using System.Threading.Tasks;
using Xmf2.Components.Events;
using Xmf2.Core.Subscriptions;
using Xmf2.Core.Droid.Permissions;
using Xmf2.Components.Droid.Events;
using Xmf2.Components.Bootstrappers;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.Droid.Fragments
{
	public abstract class BaseFragmentActivity<TViewModel> : NavigationGraph.Droid.Bases.BaseFragmentActivity<TViewModel>, IPermissionHandlingActivity
		where TViewModel : IComponentViewModel
	{
		protected Xmf2Disposable Disposables = new Xmf2Disposable();

		protected abstract int LayoutId { get; }


		protected BaseFragmentActivity(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public BaseFragmentActivity() { }

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(LayoutId);
		}

		public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged(newConfig);
			BaseApplicationBootstrapper.StaticServices.Resolve<IGlobalEventBus>().Publish(new ConfigurationChangedEvent(newConfig: newConfig));
		}

		public override void OnBackPressed()
		{
			var currentFrag = SupportFragmentManager.GetTopFragment();
			if (currentFrag is IBackFragment frag)
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
			}

			base.Dispose(disposing);
		}
	}
}