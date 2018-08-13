﻿using System;
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
	public interface IActivityWithBusyManagement
	{
		bool IsBusy { get; set; }
	}

	public interface IActivityWithKeyboardManagement
	{
		void HideKeyboard(View nextFocus);

		void HideKeyboard();
	}

	public abstract class BaseActivity<TViewModel> : XMFAppCompatActivity<TViewModel>, IActivityWithBusyManagement, IActivityWithKeyboardManagement where TViewModel : BaseViewModel
	{
		public abstract TViewModel GetViewModel();

		protected readonly XmfDisposable Disposable = new XmfDisposable();

		#region Busy Indicator

		protected virtual int LoadingViewLayout => -1;
		protected virtual int LoadingViewProgressId => -1;
		protected virtual int LoadingViewTitleId => -1;
		protected abstract ViewGroup BusyViewGroup { get; }

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

		#region Status bar color

		protected void ColorizeStatusBar(int color)
		{
			ColorizeStatusBar(new Color(color));
		}

		protected void ColorizeStatusBar(Color color)
		{
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				Window?.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
				Window?.SetStatusBarColor(color);
			}
		}

		#endregion

		#region Life cycle

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			OnContentViewSet();
			if (LoadingViewLayout >= 0 && LoadingViewProgressId >= 0 && LoadingViewTitleId >= 0)
			{
				LoadingViewHelper = new LoadingViewHelper(BusyViewGroup, LoadingViewLayout, LoadingViewProgressId, LoadingViewTitleId);
			}
			ViewModel = GetViewModel();
			OnViewModelSet();
			SetViewModelBindings();
		}

		protected override void OnStart()
		{
			base.OnStart();
			ViewModel?.LifecycleManager.Start();
		}

		protected override void OnResume()
		{
			base.OnResume();
			ViewModel?.LifecycleManager.Resume();
		}

		protected override void OnPause()
		{
			ViewModel?.LifecycleManager.Pause();
			base.OnPause();
		}

		protected override void OnStop()
		{
			ViewModel?.LifecycleManager.Stop();
			base.OnStop();
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			Dispose();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				LoadingViewHelper?.Dispose();
				Disposable.Dispose();
			}
			base.Dispose(disposing);
		}

		#endregion

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
