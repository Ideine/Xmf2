﻿using System;
using Android;
using Android.Graphics;
using Android.Support.V4.App;
using Android.Support.V7.Transition;
using Android.Views;
using ReactiveUI.AndroidSupport;
using Xmf2.Rx.ViewModels;

namespace Xmf2.Rx.Droid.BaseView
{
	public abstract class BaseReactiveDialogFragment<TViewModel> : XMFReactiveDialogFragment<TViewModel>
		where TViewModel : BaseViewModel
	{
		protected abstract TViewModel GetViewModel();

		protected abstract int LayoutResId { get; }

		protected virtual bool IsDialogCancelable { get; set; } = false;

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			return inflater.Inflate(LayoutResId, container, false);
		}

		public override void OnActivityCreated(Android.OS.Bundle savedInstanceState)
		{
			base.OnActivityCreated(savedInstanceState);
			OnContentViewSet();
			ViewModel = GetViewModel();
			OnViewModelSet();
			SetViewModelBindings();
		}

		protected virtual void OnContentViewSet()
		{
		}

		protected virtual void OnViewModelSet()
		{
		}

		protected virtual void SetViewModelBindings()
		{
			
		}

		protected void StretchContent()
		{
			ApplySize(1f, 1f);
		}

		private void ApplySize(float widthRatio, float heightRatio)
		{
			Display display = Activity.WindowManager.DefaultDisplay;
			Point size = new Point();
			display.GetSize(size);
			int width = (int) (size.X * widthRatio);
			int height = (int) (size.Y * heightRatio);
			Dialog.Window.SetLayout(width, height);
		}
	}
}