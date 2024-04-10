using System;
using ReactiveUI;
using Android.Runtime;
using Xmf2.Rx.ViewModels;

namespace Xmf2.Rx.Droid.BaseView
{
    public abstract class BaseReactiveFragment<TViewModel> : XMFFragment<TViewModel> where TViewModel : BaseViewModel
	{
		protected abstract TViewModel GetViewModel();

		public BaseReactiveFragment() { }

		protected BaseReactiveFragment(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

		public override void OnActivityCreated(Android.OS.Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);
			ViewModel = GetViewModel();
			if (ViewModel != null)
			{
				OnViewModelSet();
			}
		}

		protected virtual void OnViewModelSet() => SetViewModelBindings();

		protected virtual void SetViewModelBindings() { }
	}

	public abstract class BaseReactiveFragmentWithLifeCycleManagement<TViewModel> : BaseReactiveFragment<TViewModel> where TViewModel : BaseViewModel
	{
		public BaseReactiveFragmentWithLifeCycleManagement() { }

		protected BaseReactiveFragmentWithLifeCycleManagement(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

		public override void OnStart()
		{
			base.OnStart();
			ViewModel?.LifecycleManager.Start();
		}

		public override void OnPause()
		{
			base.OnPause();
			ViewModel?.LifecycleManager.Pause();
		}
		public override void OnResume()
		{
			base.OnResume();
			ViewModel?.LifecycleManager.Resume();
		}
		public override void OnStop()
		{
			base.OnStop();
			ViewModel?.LifecycleManager.Stop();
		}
	}

	public abstract class BaseReactiveFragmentWithLifeCycleManagement : XMFFragment, IViewFor
	{
		protected abstract object GetViewModel();

		private object _ViewModel;
		public virtual object ViewModel
		{
			get => _ViewModel;
			set => this.RaiseAndSetIfChanged(ref _ViewModel, value);
		}

		object IViewFor.ViewModel
		{
			get => _ViewModel;
			set => _ViewModel = value;
		}

		public BaseReactiveFragmentWithLifeCycleManagement() { }
		protected BaseReactiveFragmentWithLifeCycleManagement(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership) { }

		public override void OnActivityCreated(Android.OS.Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);
			ViewModel = GetViewModel();
			if (ViewModel != null)
			{
				OnViewModelSet();
			}
		}

		protected virtual void OnViewModelSet() => SetViewModelBindings();

		protected virtual void SetViewModelBindings() { }
		public override void OnStart()
		{
			base.OnStart();
			if (ViewModel is BaseViewModel baseViewModel)
			{
				baseViewModel.LifecycleManager.Start();
			}
		}

		public override void OnPause()
		{
			base.OnPause();
			if (ViewModel is BaseViewModel baseViewModel)
			{
				baseViewModel.LifecycleManager.Pause();
			}
		}
		public override void OnResume()
		{
			base.OnResume();
			if (ViewModel is BaseViewModel baseViewModel)
			{
				baseViewModel.LifecycleManager.Resume();
			}
		}
		public override void OnStop()
		{
			base.OnStop();
			if (ViewModel is BaseViewModel baseViewModel)
			{
				baseViewModel.LifecycleManager.Stop();
			}
		}
	}
}
