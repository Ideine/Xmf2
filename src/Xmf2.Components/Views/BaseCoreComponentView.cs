using System;
using Xmf2.Components.Events;
using Xmf2.Components.Interfaces;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.Views
{
	public abstract class BaseCoreComponentView<TViewState> : IDisposable where TViewState : class, IViewState
	{
		private IEventBus _eventBus;
		private IGlobalEventBus _globalEventBus;

		protected IServiceLocator Services { get; }
		protected Xmf2Disposable Disposables { get; }

		protected IEventBus EventBus => _eventBus ??= Services.Resolve<IEventBus>();
		protected IGlobalEventBus GlobalEventBus => _globalEventBus ??= Services.Resolve<IGlobalEventBus>();
		protected TViewState CurrentState { get; private set; }

		protected BaseCoreComponentView(IServiceLocator services)
		{
			Disposables = new Xmf2Disposable();
			Services = services;
		}

		public void SetState(IViewState state)
		{
			if (state == null)
			{
				return;
			}

			if (state is TViewState typedState)
			{
				OnStateUpdate(typedState);

				CurrentState = typedState;
				return;
			}

			throw new ArgumentException($"expected state type for {GetType().Name} to be {typeof(TViewState)}, got {state.GetType()}", nameof(state));
		}

		protected virtual void OnStateUpdate(TViewState state) { }

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_eventBus = null;
				CurrentState = null;
				Disposables.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~BaseCoreComponentView()
		{
			Dispose(false);
		}

		#endregion
	}
}