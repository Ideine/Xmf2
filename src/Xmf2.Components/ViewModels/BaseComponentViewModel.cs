using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xmf2.Components.Events;
using Xmf2.Components.Interfaces;
using Xmf2.Components.Services;
using Xmf2.Components.ViewModels.Operations;
using Xmf2.Core.Errors;
using Xmf2.Core.Extensions;
using Xmf2.Core.Subscriptions;

namespace Xmf2.Components.ViewModels
{
	public abstract class BaseComponentViewModel : BaseServiceContainer, IComponentViewModel, IStateRaiser, ILifecycle
	{
		private IEventBus _eventBus;
		private IGlobalEventBus _globalEventBus;
		private bool _disposed = false;

		IServiceLocator IComponentViewModel.Services => Services;
		public ILifecycleManager Lifecycle { get; }

		protected Xmf2Disposable Disposables { get; }
		protected IBusy Busy { get; }
		protected IEventBus EventBus => _eventBus ?? (_eventBus = Services.Resolve<IEventBus>());
		protected IGlobalEventBus GlobalEventBus => _globalEventBus ?? (_globalEventBus = Services.Resolve<IGlobalEventBus>());

		protected BaseComponentViewModel(IServiceLocator services) : base(services)
		{
			Lifecycle = new LifecycleManager(this);
			Disposables = new Xmf2Disposable();
			Busy = new Busy(this).DisposeWith(Disposables);
		}

		public IViewState ViewState()
		{
			lock (ApplicationState.Mutex)
			{
				if (_disposed)
				{
					return null;
				}

				return NewState();
			}
		}

		public void RaiseStateChanged() => ApplicationState.RaiseStateChanged();

		protected abstract IViewState NewState();

		/// <summary>
		/// Convenience method to assign a single field value only if it changed. Avoid raising a new state if unecessary.
		/// If you need to update several field or property use <see cref="Exec"/> instead.
		/// </summary>
		/// <example>
		/// public class FooViewModel : BaseComponentViewModel
		/// {
		/// 	private bool _bar = false;
		/// 	public bool Bar
		/// 	{
		/// 		get => _bar;
		/// 		set => ExecIfChanged(_bar, value, () => _bar = value);
		/// 	}
		/// }
		/// /***somewhere else***/
		/// private void SomeWork(FooViewModel foo)
		/// {
		///		foo.Bar = false; // Do not raise any state
		///		foo.Bar = true; // Raise a new state.
		/// }
		/// </example>
		protected void ExecIfChanged<T>(T value, T newValue, Action updateAction)
		{
			ExecIfChanged(value, newValue, updateAction, EqualityComparer<T>.Default);
		}
		protected void ExecIfChanged<T>(T value, T newValue, Action updateAction, IEqualityComparer<T> comparer)
		{
			if (comparer.Equals(value, newValue))
				return;
			Exec(updateAction);
		}
		protected void ExecIfChanged<T>(T value, T newValue, Action<T> updateAction)
		{
			ExecIfChanged(value, newValue, updateAction, EqualityComparer<T>.Default);
		}
		protected void ExecIfChanged<T>(T value, T newValue, Action<T> updateAction, IEqualityComparer<T> comparer)
		{
			if (comparer.Equals(value, newValue))
				return;
			Exec(() => updateAction(newValue));
		}

		/// <summary>
		/// DONT USE WITH ASYNC
		/// </summary>
		/// <param name="creator">DONT PUT ASYNC</param>
		protected void Exec(Func<IViewModelOperation, IViewModelOperation> creator, CustomErrorHandler errorHandler = null)
		{
			ExecAsync(creator, errorHandler).ConfigureAwait(false);
		}

		protected void Exec(Action execution, CustomErrorHandler errorHandler = null)
		{
			Exec(operation => operation.ViewModelUpdate(execution), errorHandler);
		}

		protected void ExecSafeSync(Func<IViewModelOperation, IViewModelOperation> creator, CustomErrorHandler errorHandler = null)
		{
			ExecAsync(creator, errorHandler).FireAndForget();
		}

		protected async Task ExecAsync(Func<IViewModelOperation, IViewModelOperation> creator, CustomErrorHandler errorHandler = null)
		{
			IViewModelOperation result = creator(new DefaultViewModelOperation(Busy));

			try
			{
				CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
				await Task.Run(result.Start, cts.Token);
				RaiseStateChanged();
			}
			catch (Exception ex)
			{
				if (!await Services.Resolve<IErrorHandler>().Handle(ex, errorHandler))
				{
					throw;
				}
			}

			result.Dispose();
		}

		protected async Task ExecAsyncWithoutErrorHandler(Func<IViewModelOperation, IViewModelOperation> creator)
		{
			using (IViewModelOperation result = creator(new DefaultViewModelOperation(Busy)))
			{
				await result.Start();
				RaiseStateChanged();
			}
		}

		protected Task ExecAsync(Func<Task> execution, CustomErrorHandler errorHandler = null)
		{
			return ExecAsync(operation => operation.Async(execution), errorHandler);
		}

		#region ILifecycle

		/// <summary>
		/// Method is called prior to any navigation and should be used to 
		/// load data needed when view is displayed.
		/// </summary>
		/// <returns>Awaitable task</returns>
		Task ILifecycle.Initialize()
		{
			return Initialize();
		}

		/// <summary>
		/// Method called on the first load of view.
		/// </summary>
		Task ILifecycle.OnStart()
		{
			return OnStart();
		}

		/// <summary>
		/// Method called when view is displayed
		/// </summary>
		Task ILifecycle.OnResume()
		{
			return OnResume();
		}

		/// <summary>
		/// Method called when view is hidden
		/// </summary>
		Task ILifecycle.OnPause()
		{
			return OnPause();
		}

		/// <summary>
		/// Method called when view is stopped
		/// </summary>
		Task ILifecycle.OnStop()
		{
			return OnStop();
		}

		protected virtual Task Initialize()
		{
			Debug.WriteLine($"[Lifecycle] Initialize {GetType().Name}");
			return Task.CompletedTask;
		}

		protected virtual Task OnStart()
		{
			Debug.WriteLine($"[Lifecycle] OnStart {GetType().Name}");
			return Task.CompletedTask;
		}

		protected virtual Task OnResume()
		{
			Debug.WriteLine($"[Lifecycle] OnResume {GetType().Name}");
			return Task.CompletedTask;
		}

		protected virtual Task OnPause()
		{
			Debug.WriteLine($"[Lifecycle] OnPause {GetType().Name}");
			return Task.CompletedTask;
		}

		protected virtual Task OnStop()
		{
			Debug.WriteLine($"[Lifecycle] OnStop {GetType().Name}");
			return Task.CompletedTask;
		}

		#endregion

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			_disposed = true;
			if (disposing)
			{
				Disposables.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~BaseComponentViewModel()
		{
			Dispose(false);
		}

		#endregion
	}
}