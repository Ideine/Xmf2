using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.Views;
using ReactiveUI;
using Xmf2.Commons.Droid.LinearList;

namespace Xmf2.Rx.Droid.LinearList
{
	public class BaseReactiveLinearLayoutViewViewHolder<TViewModel> : LinearListViewHolder, IViewFor<TViewModel>, IViewFor, ICanActivate where TViewModel : class, IReactiveObject
	{
		readonly Subject<Unit> _activated = new Subject<Unit>();
		public IObservable<Unit> Activated => _activated.AsObservable();

		readonly Subject<Unit> _deactivated = new Subject<Unit>();
		public IObservable<Unit> Deactivated => _deactivated.AsObservable();

		public void Activate()
		{
			_activated.OnNext(Unit.Default);
		}

		public void Deactivate()
		{
			_deactivated.OnNext(Unit.Default);
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
			get => ViewModel as TViewModel;
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
