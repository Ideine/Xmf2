using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using ReactiveUI;
using UIKit;
using Xmf2.iOS.Controls.ItemControls;

namespace Xmf2.Rx.iOS.Controls.ItemControls
{
	public class BaseReactiveItemCell<TViewModel> : BaseItemCell<TViewModel>, ICanActivate, IActivationForViewFetcher, IViewFor<TViewModel> where TViewModel : class
	{
		public override object DataContext
		{
			get => base.DataContext;
			set
			{
				if (!Equals(base.DataContext, value))
				{
					base.DataContext = value;
				}
				Activate(value != null);
			}
		}

		public TViewModel ViewModel
		{
			get => DataContext as TViewModel;
			set => DataContext = value;
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (TViewModel)value;
		}

		Subject<Unit> activated = new Subject<Unit>();
		public IObservable<Unit> Activated => activated.AsObservable();

		Subject<Unit> deactivated = new Subject<Unit>();
		public IObservable<Unit> Deactivated => deactivated.AsObservable();

		public BaseReactiveItemCell() { }

		public int GetAffinityForView(Type view)
		{
			return (typeof(ICanActivate).GetTypeInfo().IsAssignableFrom(view.GetTypeInfo())) ? 10 : 0;
		}

		public IObservable<bool> GetActivationForView(IActivatable view)
		{
			var ca = view as ICanActivate;
			return ca.Activated.Select(_ => true).Merge(ca.Deactivated.Select(_ => false));
		}

		private void Activate(bool activate)
		{
			RxApp.MainThreadScheduler.Schedule(() => (activate ? activated : deactivated).OnNext(Unit.Default));
		}

		public override void WillMoveToSuperview(UIView newsuper)
		{
			base.WillMoveToSuperview(newsuper);
			RxApp.MainThreadScheduler.Schedule(() => (newsuper != null ? activated : deactivated).OnNext(Unit.Default));
		}
	}
}
