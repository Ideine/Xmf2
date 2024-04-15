using System;
using System.Reactive;
using System.Windows.Input;
using Android.Content;
using Android.Views;
using ReactiveUI;
using Xmf2.Commons.Droid.ChipClouds;
using Xmf2.Rx.Helpers;

namespace Xmf2.Rx.Droid.ChipClouds
{
    public class BaseReactiveChipCloudViewHolder<TViewModel> : ChipCloudViewHolder, IViewFor<TViewModel>, IViewFor, ICanActivate
		where TViewModel : class, IReactiveObject
	{
		public readonly Context Context;

		private TViewModel _viewModel;
		public TViewModel ViewModel
		{
			get => _viewModel;
			set
			{
				_viewModel = value;
				if (value != null)
				{
					OnViewModelSet();
				}
			}
		}

		object IViewFor.ViewModel
		{
			get => ViewModel as TViewModel;
			set => ViewModel = value as TViewModel;
		}

		private readonly CanActivateImplementation _activationImplementation = new CanActivateImplementation();

		public IObservable<Unit> Activated => _activationImplementation.Activated;

		public IObservable<Unit> Deactivated => _activationImplementation.Deactivated;

		public ICommand ItemClick { get; set; }

		public BaseReactiveChipCloudViewHolder(View view) : base(view)
		{
			Context = view.Context;
			OnContentViewSet();
			SetViewModelBindings();
			ItemView.Click += OnClickItem;
		}

		protected virtual void OnViewModelSet() { }

		protected virtual void OnContentViewSet() { }

		protected virtual void SetViewModelBindings() { }

		void OnClickItem(object sender, EventArgs e)
		{
			ItemClick?.TryExecute(ViewModel);
		}

		#region Lifecycle

		public override void OnViewAttachedToWindow()
		{
			_activationImplementation.Activate();
		}

		public override void OnViewDetachedFromWindow()
		{
			_activationImplementation.Deactivate();
		}

		#endregion

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				if (ItemView != null)
				{
					ItemView.Click -= OnClickItem;
				}
			}
		}

	}
}
