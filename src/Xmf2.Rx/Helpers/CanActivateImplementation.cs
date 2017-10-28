using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace Xmf2.Rx.Helpers
{
	public class CanActivateImplementation : ICanActivate
	{
		private readonly Subject<Unit> _activated = new Subject<Unit>();
		private readonly Subject<Unit> _deactivated = new Subject<Unit>();

		public IObservable<Unit> Activated => _activated.AsObservable();

		public IObservable<Unit> Deactivated => _deactivated.AsObservable();

		public void Activate()
		{
			RxApp.TaskpoolScheduler.Schedule(() => _activated.OnNext(Unit.Default));
		}

		public void Deactivate()
		{
			RxApp.TaskpoolScheduler.Schedule(() => _deactivated.OnNext(Unit.Default));
		}
	}
}
