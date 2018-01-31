using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using ReactiveUI;
using Xmf2.Rx.Helpers;

namespace Xmf2.Rx.Droid.BaseView
{
	public class ReactiveLinearLayout<TViewModel> : LinearLayout, IViewFor<TViewModel>, ICanActivate where TViewModel : class
	{
		protected CompositeDisposable _dispo = new CompositeDisposable();

		#region Constructor

		public ReactiveLinearLayout(Context context) : base(context) { }

		public ReactiveLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public ReactiveLinearLayout(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }

		protected ReactiveLinearLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		#endregion

		#region Reactive UI

		private TViewModel _viewModel;
		public virtual TViewModel ViewModel
		{
			get => _viewModel;
			set
			{
				if (!Equals(_viewModel, value))
				{
					_viewModel = value;
				}

				OnViewModelSet();

				if (value != null)
				{
					this.Activate();
				}
			}
		}

		object IViewFor.ViewModel
		{
			get => ViewModel as TViewModel;
			set => ViewModel = value as TViewModel;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_dispo?.Dispose();
				_dispo = null;
			}
			base.Dispose(disposing);
		}

		private readonly CanActivateImplementation _activationImpl = new CanActivateImplementation();

		public new IObservable<Unit> Activated => _activationImpl.Activated;

		public IObservable<Unit> Deactivated => _activationImpl.Deactivated;

		public void Activate() => _activationImpl.Activate();

		public void Deactivate() => _activationImpl.Deactivate();

		#endregion

		protected virtual void OnViewModelSet() { }
	}
}
