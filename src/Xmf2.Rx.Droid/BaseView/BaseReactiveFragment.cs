using System;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
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
			OnViewModelSet();
		}

		protected virtual void OnViewModelSet()
		{
			SetViewModelBindings();
		}

		protected virtual void SetViewModelBindings()
		{

		}
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
}
