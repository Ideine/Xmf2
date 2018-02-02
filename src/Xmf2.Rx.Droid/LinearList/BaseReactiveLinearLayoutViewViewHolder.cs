using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.Views;
using ReactiveUI;
using Xmf2.Commons.Droid.LinearList;
using Xmf2.Rx.Helpers;
using Xmf2.Rx;

namespace Xmf2.Rx.Droid.LinearList
{
	public class BaseReactiveLinearLayoutViewViewHolder<TViewModel> : LinearListViewHolder, IViewFor<TViewModel>, IViewFor, ICanActivate
		where TViewModel : class, IReactiveObject
	{
		protected XmfDisposable Disposable = new XmfDisposable();

		private readonly CanActivateImplementation _activationImplementation = new CanActivateImplementation();

		public IObservable<Unit> Activated => _activationImplementation.Activated;

		public IObservable<Unit> Deactivated => _activationImplementation.Deactivated;

		public void Activate()
		{
			_activationImplementation.Activate();
		}

		public void Deactivate()
		{
			_activationImplementation.Deactivate();
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

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				Disposable.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}
