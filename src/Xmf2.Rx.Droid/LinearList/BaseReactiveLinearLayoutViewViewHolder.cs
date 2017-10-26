using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Android.Views;
using ReactiveUI;
using Xmf2.Commons.Droid.LinearList;

namespace Xmf2.Rx.Droid.LinearList
{
	public class BaseReactiveLinearLayoutViewViewHolder<TViewModel> : LinearListViewHolder, IViewFor<TViewModel>, IViewFor, ICanActivate where TViewModel : class, IReactiveObject
	{
		private readonly Subject<Unit> _activated = new Subject<Unit>();
		public IObservable<Unit> Activated => _activated;

		private readonly Subject<Unit> _deactivated = new Subject<Unit>();
		public IObservable<Unit> Deactivated => _deactivated;

		public void Activate()
		{
			RxApp.MainThreadScheduler.Schedule(() => (_activated).OnNext(Unit.Default));
		}

		public void Deactivate()
		{
			RxApp.MainThreadScheduler.Schedule(() => (_deactivated).OnNext(Unit.Default));
		}

		public TViewModel ViewModel
		{
			get => DataContext as TViewModel;
			set
			{
				if (!Equals(DataContext, value))
				{
					DataContext = value;
				}
				OnViewModelSet();
				Activate();
			}
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = value as TViewModel;
		}


		public BaseReactiveLinearLayoutViewViewHolder(View view) : base(view)
		{
			OnContentViewSet();
			SetViewModelBindings();
		}

		protected BaseReactiveLinearLayoutViewViewHolder(IntPtr javaRef, Android.Runtime.JniHandleOwnership transfer) : base(javaRef, transfer) { }

		protected virtual void OnContentViewSet() { }

		protected virtual void SetViewModelBindings() { }

		protected virtual void OnViewModelSet() { }

		#region Lifecycle

		public virtual void OnViewRecycled() { }

		#endregion
	}
}
