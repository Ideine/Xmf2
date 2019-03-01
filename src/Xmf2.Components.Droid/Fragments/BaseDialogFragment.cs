using System;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Xmf2.Components.Droid.Interfaces;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Droid.Fragments
{
	public abstract class BaseDialogFragment<TComponentViewModel, TComponentView> : DialogFragment
		where TComponentViewModel : class, IComponentViewModel
		where TComponentView : class, IComponentView
	{
		private TComponentView _componentView;
		private TComponentViewModel _componentViewModel;

		private EventSubscriber _stateChangedSubscriber;

		protected Xmf2Disposable Disposable = new Xmf2Disposable();

		protected IServiceLocator Services => _componentViewModel.Services;

		protected BaseDialogFragment(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public BaseDialogFragment(TComponentViewModel viewModel, Func<IServiceLocator, TComponentView> factory)
		{
			_componentViewModel = viewModel;
			_componentView = factory(Services).DisposeComponentWith(Disposable);

			_stateChangedSubscriber = new EventSubscriber(
				subscribe: () => ApplicationState.StateChanged += UpdateState,
				unsubscribe: () => ApplicationState.StateChanged -= UpdateState,
				autoSubscribe: false
			).DisposeEventWith(Disposable);
		}

		private void UpdateState()
		{
			var state = _componentViewModel?.ViewState();
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
			_componentViewModel.Lifecycle.Initialize();
		}

		#region Lifecycle

		public override void OnStart()
		{
			base.OnStart();
			_componentViewModel.Lifecycle.Start();
		}

		public override void OnResume()
		{
			base.OnResume();
			_componentViewModel.Lifecycle.Resume();
			_stateChangedSubscriber.Subscribe();
			UpdateState();
		}

		public override void OnPause()
		{
			base.OnPause();
			_componentViewModel.Lifecycle.Pause();
			_stateChangedSubscriber.Unsubscribe();
		}

		public override void OnStop()
		{
			base.OnStop();
			_componentViewModel.Lifecycle.Stop();
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
				_componentViewModel = null;
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