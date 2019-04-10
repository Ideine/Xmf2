using System;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Xmf2.Components.Bootstrappers;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Subscriptions;
using Xmf2.NavigationGraph.Core.Interfaces;

namespace Xmf2.Components.Droid.Fragments
{
	public abstract class BaseDialogFragment<TComponentViewModel, TComponentView> : NavigationGraph.Droid.Bases.BaseDialogFragment<TComponentViewModel>
		where TComponentViewModel : class, IComponentViewModel
		where TComponentView : class, IComponentView
	{
		private TComponentView _componentView;

		private EventSubscriber _stateChangedSubscriber;

		protected Xmf2Disposable Disposable = new Xmf2Disposable();

		protected IServiceLocator Services => ViewModel.Services;

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

		protected abstract Func<IServiceLocator, TComponentView> Factory { get; }

		protected override IViewModelLocatorService<TComponentViewModel> ViewModelLocatorService => BaseApplicationBootstrapper.StaticServices.Resolve<IViewModelLocatorService<TComponentViewModel>>();

		private void Init()
		{
			_componentView = Factory(Services).DisposeComponentWith(Disposable);

			_stateChangedSubscriber = new EventSubscriber(
				subscribe: () => ApplicationState.StateChanged += UpdateState,
				unsubscribe: () => ApplicationState.StateChanged -= UpdateState,
				autoSubscribe: false
			).DisposeEventWith(Disposable);
		}

		protected BaseDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		protected BaseDialogFragment() { }

		private void UpdateState()
		{
			var state = ViewModel?.ViewState();
			if (state != null)
			{
				Activity.RunOnUiThread(() => _componentView?.SetState(state));
			}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return _componentView.View(container);
		}

		public override void OnViewCreated(View view, Bundle savedInstanceState)
		{
			base.OnViewCreated(view, savedInstanceState);
			ViewModel.Lifecycle.Initialize();
		}

		#region Lifecycle

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
				_componentView = null;
				ViewModel = null;
				_stateChangedSubscriber = null;
				Disposable.Dispose();
				Disposable = null;
			}

			base.Dispose(disposing);
		}

		protected void StretchContent() => ApplySize(1f, 1f);

		private void ApplySize(float widthRatio, float heightRatio)
		{
			Display display = Activity.WindowManager.DefaultDisplay;
			Point size = new Point();
			display.GetSize(size);
			int width = (int)(size.X * widthRatio);
			int height = (int)(size.Y * heightRatio);
			Dialog.Window.SetLayout(width, height);
		}
	}
}