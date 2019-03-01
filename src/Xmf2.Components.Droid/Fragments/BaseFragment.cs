using System;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Droid.Views;
using Xmf2.Components.Events;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.Fragments
{
	public abstract class BaseFragment<TComponentViewModel, TComponentView> : Fragment, IBackFragment, IViewFor
		where TComponentViewModel : class, IComponentViewModel
		where TComponentView : class, IComponentView
	{
		protected TComponentView Component { get; private set; }
		private TComponentViewModel _viewModel;

		private EventSubscriber _stateChangedSubscriber;
		protected Xmf2Disposable Disposables;

		protected IServiceLocator Services => _viewModel.Services;

		public Type ViewModelType => typeof(TComponentViewModel);

		protected BaseFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public BaseFragment(TComponentViewModel viewModel, Func<IServiceLocator, TComponentView> factory)
		{
			Disposables = new Xmf2Disposable();

			_viewModel = viewModel;
			Component = factory(Services).DisposeComponentWith(Disposables);

			_stateChangedSubscriber = new EventSubscriber(
				() => ApplicationState.StateChanged += UpdateState,
				() => ApplicationState.StateChanged -= UpdateState,
				autoSubscribe: false
			).DisposeEventWith(Disposables);
		}

		private void UpdateState()
		{
			var state = _viewModel?.ViewState();
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
			_viewModel.Lifecycle.Initialize();
		}

		public override void OnStart()
		{
			base.OnStart();
			_viewModel.Lifecycle.Start();
		}

		public override void OnResume()
		{
			base.OnResume();
			_viewModel.Lifecycle.Resume();
			_stateChangedSubscriber.Subscribe();
			UpdateState();
		}

		public override void OnPause()
		{
			base.OnPause();
			_viewModel.Lifecycle.Pause();
			_stateChangedSubscriber.Unsubscribe();
		}

		public override void OnStop()
		{
			base.OnStop();
			_viewModel.Lifecycle.Stop();
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
				_viewModel = null;
				_stateChangedSubscriber = null;
				Disposables.Dispose();
				Disposables = null;
			}

			base.Dispose(disposing);
		}
	}
}