using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using ReactiveUI;

namespace Xmf2.Rx.Droid.BaseView
{
	public class ReactiveLinearLayout<TViewModel> : LinearLayout, IViewFor<TViewModel>, ICanActivate where TViewModel : class
	{
		#region Constructor

		public ReactiveLinearLayout(Context context) : base(context) { }

		public ReactiveLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs) { }

		public ReactiveLinearLayout(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { }

		protected ReactiveLinearLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		#endregion

		#region Reactive UI

		private TViewModel _viewModel;
		public TViewModel ViewModel
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

		private readonly Subject<Unit> _activated = new Subject<Unit>();
		public new IObservable<Unit> Activated => _activated;

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

		#endregion

		protected virtual void OnViewModelSet() { }
	}
}
