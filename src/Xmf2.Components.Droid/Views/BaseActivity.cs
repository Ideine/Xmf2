using System;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Droid.Permissions;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.Views
{
	public interface IViewFor
	{
		Type ViewModelType { get; }
	}

	public class BaseActivity<TComponentViewModel, TComponentView> : AppCompatActivity, IViewFor, IPermissionHandlingActivity
		where TComponentViewModel : IComponentViewModel
		where TComponentView : IComponentView
	{
		private IComponentView _component;
		private IComponentViewModel _componentViewModel;
		private EventSubscriber _stateChangedSubscriber;
		private Xmf2Disposable _disposables;

		protected IServiceLocator Services => _componentViewModel.Services;

		public BaseActivity(IComponentViewModel viewModel, Func<IServiceLocator, TComponentView> factory)
		{
			_disposables = new Xmf2Disposable();
			_componentViewModel = viewModel;
			_component = factory(Services).DisposeComponentWith(_disposables);

			_stateChangedSubscriber = new EventSubscriber(
				() => ApplicationState.StateChanged += UpdateState,
				() => ApplicationState.StateChanged -= UpdateState,
				autoSubscribe: false
			).DisposeEventWith(_disposables);
		}

		protected BaseActivity(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer) { }

		private void UpdateState()
		{
			RunOnUiThread(() =>
			{
				IViewState state = _componentViewModel?.ViewState();
				_component?.SetState(state);
			});
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(_component.View(Window.DecorView as ViewGroup));

			_componentViewModel.Lifecycle.Initialize();
		}

		protected override void OnStart()
		{
			base.OnStart();

			_componentViewModel.Lifecycle.Start();
		}

		protected override void OnResume()
		{
			base.OnResume();

			_componentViewModel.Lifecycle.Resume();
			_stateChangedSubscriber.Subscribe();
			UpdateState();
		}

		protected override void OnPause()
		{
			base.OnPause();

			_componentViewModel.Lifecycle.Pause();
			_stateChangedSubscriber.Unsubscribe();
		}

		protected override void OnStop()
		{
			base.OnStop();

			_componentViewModel.Lifecycle.Stop();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();

			Dispose();
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
				_component = null;
				_componentViewModel = null;
				_stateChangedSubscriber = null;

				_disposables.Dispose();
				_disposables = null;
			}
			base.Dispose(disposing);
		}

		public Type ViewModelType => _componentViewModel.GetType();
	}
}