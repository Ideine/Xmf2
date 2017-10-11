using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Windows.Input;
using Android.Content;
using Android.Views;
using ReactiveUI;
using Xmf2.Commons.Droid.ChipClouds;

namespace Xmf2.Rx.Droid.ChipClouds
{
	public class BaseReactiveChipCloudViewHolder<TViewModel> : ChipCloudViewHolder, IViewFor<TViewModel>, IViewFor, ICanActivate where TViewModel : class, IReactiveObject 
	{
		public readonly Context Context;

		private TViewModel _viewModel;
		public TViewModel ViewModel
		{
			get => _viewModel;
			set
			{
				_viewModel = value;
				Activate();
			}
		}

		object IViewFor.ViewModel
		{
			get => ViewModel as TViewModel;
			set => ViewModel = value as TViewModel;
		}

		private readonly Subject<Unit> activated = new Subject<Unit>();
		public IObservable<Unit> Activated => activated;

		private readonly Subject<Unit> deactivated = new Subject<Unit>();
		public IObservable<Unit> Deactivated => deactivated;

		public ICommand ItemClick { get; set; }

		IObservable<Unit> ICanActivate.Activated => throw new NotImplementedException();

		IObservable<Unit> ICanActivate.Deactivated => throw new NotImplementedException();

		public BaseReactiveChipCloudViewHolder(View view) : base(view)
        {
			Context = view.Context;
			OnContentViewSet();
			SetViewModelBindings();
			ItemView.Click += OnClickItem;
		}

		protected virtual void OnContentViewSet() { }

		protected virtual void SetViewModelBindings() { }

		void OnClickItem(object sender, EventArgs e)
		{
			ItemClick?.TryExecute(ViewModel);
		}

		#region Lifecycle

		public override void OnViewAttachedToWindow()
		{
			Activate();
		}

		public override void OnViewDetachedFromWindow()
		{
			Deactivate();
		}

		public void Activate()
		{
			RxApp.MainThreadScheduler.Schedule(() => (activated).OnNext(Unit.Default));
		}

		public void Deactivate()
		{
			RxApp.MainThreadScheduler.Schedule(() => (deactivated).OnNext(Unit.Default));
		}

		#endregion

		public override void Dispose()
		{
			if (ItemView != null)
			{
				ItemView.Click -= OnClickItem;
			}
			base.Dispose();
		}

	}
}
