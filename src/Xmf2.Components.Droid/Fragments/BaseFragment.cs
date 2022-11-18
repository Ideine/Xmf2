using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xmf2.Components.Bootstrappers;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Events;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Subscriptions;
using Xmf2.NavigationGraph.Core.Interfaces;
using Xmf2.NavigationGraph.Droid.Interfaces;

namespace Xmf2.Components.Droid.Fragments
{
	public abstract class BaseFragment<TComponentViewModel, TComponentView> : NavigationGraph.Droid.Bases.BaseFragment<TComponentViewModel>, IBackFragment
		where TComponentViewModel : class, IComponentViewModel
		where TComponentView : class, IComponentView
	{
		protected TComponentView Component { get; private set; }

		protected override TComponentViewModel ViewModel
		{
			get => base.ViewModel;
			set
			{
				base.ViewModel = value;
				if (value != null)
				{
					Init();
				}
			}
		}

		protected override IRegistrationPresenterService<TComponentViewModel> PresenterService => BaseApplicationBootstrapper.StaticServices.Resolve<IRegistrationPresenterService<TComponentViewModel>>();
		protected override IViewModelLocatorService<TComponentViewModel> ViewModelLocatorService => BaseApplicationBootstrapper.StaticServices.Resolve<IViewModelLocatorService<TComponentViewModel>>();

		protected override IRegistrationPresenterService<TComponentViewModel> PresenterService => Services.Resolve<IRegistrationPresenterService<TComponentViewModel>>();

		private EventSubscriber _stateChangedSubscriber;
		protected Xmf2Disposable Disposables = new();

		protected IServiceLocator Services => ViewModel.Services;

		protected BaseFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected BaseFragment() { }

		protected abstract Func<IServiceLocator, TComponentView> Factory { get; }

		private void Init()
		{
			Component = Factory(Services).DisposeComponentWith(Disposables);

			_stateChangedSubscriber = new EventSubscriber(
				() => ApplicationState.StateChanged += UpdateState,
				() => ApplicationState.StateChanged -= UpdateState,
				autoSubscribe: false
			).DisposeEventWith(Disposables);
		}

		private void UpdateState()
		{
			IViewState state = ViewModel?.ViewState();
			if (state != null)
			{
				Activity.RunOnUiThread(() => Component?.SetState(state));
			}
		}

		public virtual void BackPressed()
		{
			Services.Resolve<IEventBus>().Publish(new BackEvent());
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return Component.View(container);
		}

		#region Lifecycle

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			ViewModel.Lifecycle.Initialize();
		}

		public override void OnStart()
		{
			base.OnStart();
			ViewModel.Lifecycle.Start();
		}

		public override void OnResume()
		{
			base.OnResume();
			ViewModel.Lifecycle.Resume();
			_stateChangedSubscriber.Subscribe();
			UpdateState();
		}

		public override void OnPause()
		{
			base.OnPause();
			ViewModel.Lifecycle.Pause();
			_stateChangedSubscriber.Unsubscribe();
		}

		public override void OnStop()
		{
			base.OnStop();
			ViewModel.Lifecycle.Stop();
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			Dispose();
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Component = null;
				ViewModel = null;
				_stateChangedSubscriber = null;
				Disposables.Dispose();
				Disposables = null;
			}

			base.Dispose(disposing);
		}
	}
}