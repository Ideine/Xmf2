using System;
using Splat;
using Xmf2.Rx.ViewModels;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using Android.OS;
using Android.Graphics;
using Xmf2.Commons.Droid.Helpers;
using Xmf2.Commons.Droid.Services;

namespace Xmf2.Rx.Droid.BaseView
{
	public abstract class BaseActivity<TViewModel> : XMFAppCompatActivity<TViewModel> where TViewModel : BaseViewModel
	{
		public abstract TViewModel GetViewModel();

		private readonly Lazy<ILifecycleMonitor> _lifecycleMonitor = new Lazy<ILifecycleMonitor>(() => Locator.Current.GetService<ILifecycleMonitor>());

		#region Busy Indicator

		protected virtual int LoadingViewLayout => -1;
		protected virtual int LoadingViewProgressId => -1;

		protected LoadingViewHelper LoadingViewHelper { get; private set; }

		public virtual bool IsBusy
		{
			get => LoadingViewHelper?.IsBusy ?? false;
			set
			{
				if (LoadingViewHelper != null)
				{
					LoadingViewHelper.IsBusy = value;
				}
			}
		}

		#endregion

		protected BaseActivity() { }

		protected BaseActivity(IntPtr handle, JniHandleOwnership transer) : base(handle, transer) { }


		protected virtual void OnViewModelSet() { }

		protected virtual void SetViewModelBindings() { }

		protected virtual void OnContentViewSet() { }

		protected void ColorizeStatusBar(int color)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
				Window?.SetStatusBarColor(new Color(color));
			}
		}

		#region Life cycle

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			_lifecycleMonitor.Value.OnCreate(this);
			if (LoadingViewLayout >= 0 && LoadingViewProgressId >= 0)
			{
				LoadingViewHelper = new LoadingViewHelper(this, LoadingViewLayout, LoadingViewProgressId);
			}
			OnContentViewSet();
			ViewModel = GetViewModel();
			OnViewModelSet();
			SetViewModelBindings();
		}

		protected override void OnStart()
		{
			base.OnStart();
			_lifecycleMonitor.Value.OnStart(this);
			ViewModel?.LifecycleManager.Start();
		}

		protected override void OnRestart()
		{
			base.OnRestart();
			_lifecycleMonitor.Value.OnRestart(this);
		}

		protected override void OnResume()
		{
			base.OnResume();
			_lifecycleMonitor.Value.OnResume(this);
			ViewModel?.LifecycleManager.Resume();
		}

		protected override void OnPause()
		{
			ViewModel?.LifecycleManager.Pause();
			_lifecycleMonitor.Value.OnPause(this);
			base.OnPause();
		}

		protected override void OnStop()
		{
			ViewModel?.LifecycleManager.Stop();
			_lifecycleMonitor.Value.OnStop(this);
			base.OnStop();
		}

		protected override void OnDestroy()
		{
			_lifecycleMonitor.Value.OnDestroy(this);
			base.OnDestroy();
		}

		#endregion

		//TODO: voir pour le sortir de la classe dans un helper par exemple
		#region Keyboard 

		public void HideKeyboard(View nextFocus)
		{
			HideKeyboard();
			nextFocus?.RequestFocus();
		}

		public void HideKeyboard()
		{
			InputMethodManager inputManager = (InputMethodManager)GetSystemService(InputMethodService);
			if (inputManager != null && CurrentFocus != null)
			{
				inputManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
			}
		}

		#endregion
	}
}
