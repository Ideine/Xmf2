using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;
using Xmf2.Commons.Errors;
using Xmf2.Commons.Extensions;
using Xmf2.Commons.Helpers;

namespace Xmf2.Rx.ViewModels
{
	public abstract class BaseViewModel : ReactiveObject, ISupportsActivation
	{
		protected Lazy<IErrorHandler> ErrorHanler { get; } = new Lazy<IErrorHandler>(Locator.Current.GetService<IErrorHandler>);

		private readonly Subject<bool> _isInitializing = new Subject<bool>();
		private readonly Subject<bool> _isStarting = new Subject<bool>();
		private readonly Subject<bool> _isResuming = new Subject<bool>();
		private readonly Subject<bool> _isPausing = new Subject<bool>();
		private readonly Subject<bool> _isStopping = new Subject<bool>();

		public ViewModelActivator Activator { get; }

		public IViewModelLifecycleManager LifecycleManager { get; }

		public IObservable<bool> IsInitializing { get; }

		public IObservable<bool> IsStarting { get; }

		public IObservable<bool> IsResuming { get; }

		public IObservable<bool> IsPausing { get; }

		public IObservable<bool> IsStopping { get; }

		protected BaseViewModel()
		{
			IsInitializing = _isInitializing.StartWith(false).ToObservableForBinding();
			IsStarting = _isStarting.StartWith(false).ToObservableForBinding();
			IsResuming = _isResuming.StartWith(false).ToObservableForBinding();
			IsPausing = _isPausing.StartWith(false).ToObservableForBinding();
			IsStopping = _isStopping.StartWith(false).ToObservableForBinding();

			Activator = new ViewModelActivator();
			LifecycleManager = new ViewModelLifecycleManager(this);
		}

		#region Wrap for error

		protected Task WrapForError(IObservable<Unit> source, CustomErrorHandler errorHandler = null)
		{
			return ErrorHanler.Value
							  .Execute(source.Timeout(TimeSpan.FromSeconds(30)), errorHandler)
							  .Catch<Unit, Exception>(ex => Observable.Return(default(Unit)))
							  .WaitForOneAsync();
		}

		protected Task<TResult> WrapForError<TResult>(IObservable<TResult> source, CustomErrorHandler errorHandler = null)
		{
			return ErrorHanler.Value
							  .Execute(source.Timeout(TimeSpan.FromSeconds(30)), errorHandler)
							  .Catch<TResult, Exception>(ex => Observable.Return(default(TResult)))
							  .WaitForOneAsync();
		}

		protected Task WrapForError(Func<Task> action, CustomErrorHandler errorHandler = null) => WrapForError(Observable.FromAsync(action), errorHandler);

		protected Task<TResult> WrapForError<TResult>(Func<Task<TResult>> action, CustomErrorHandler errorHandler = null) => WrapForError(Observable.FromAsync(action), errorHandler);

		#endregion

		#region Lifecycle management

		/// <summary>
		/// Method is called prior to any navigation and should be used to 
		/// load data needed when view is displayed.
		/// </summary>
		/// <returns>Awaitable task</returns>
		protected virtual Task Initialize()
		{
			Debug.WriteLine($"[Lifecycle] Initialize {GetType().Name}");
			return TaskHelper.CompletedTask;
		}

		/// <summary>
		/// Method called on the first load of view.
		/// </summary>
		protected virtual Task OnStart()
		{
			Debug.WriteLine($"[Lifecycle] OnStart {GetType().Name}");
			return TaskHelper.CompletedTask;
		}

		/// <summary>
		/// Method called when view is displayed
		/// </summary>
		protected virtual Task OnResume()
		{
			Debug.WriteLine($"[Lifecycle] OnResume {GetType().Name}");
			return TaskHelper.CompletedTask;
		}

		/// <summary>
		/// Method called when view is hidden
		/// </summary>
		protected virtual Task OnPause()
		{
			Debug.WriteLine($"[Lifecycle] OnPause {GetType().Name}");
			return TaskHelper.CompletedTask;
		}

		/// <summary>
		/// Method called when view is stopped
		/// </summary>
		protected virtual Task OnStop()
		{
			Debug.WriteLine($"[Lifecycle] OnStop {GetType().Name}");
			return TaskHelper.CompletedTask;
		}

		private class ViewModelLifecycleManager : IViewModelLifecycleManager
		{
			private enum ViewModelState
			{
				Created,
				Initialized,
				Started,
				Resumed,
				Paused,
				Stopped
			}

			private readonly BaseViewModel _viewModel;
			private readonly ConcurrentQueue<ViewModelState> _transitionsQueue = new ConcurrentQueue<ViewModelState>();
			private readonly object _isRunningMutex = new object();
			private readonly TaskCompletionSource<object> _initializationTask;
			private readonly StateAutomata _stateAutomata;

			private bool _isRunning;

			public ViewModelLifecycleManager(BaseViewModel viewModel)
			{
				_viewModel = viewModel;
				_initializationTask = new TaskCompletionSource<object>();

				_stateAutomata = CreateStateGraph();
			}

			public Task WaitForInitialization()
			{
				return _initializationTask.Task;
			}

			public void Initialize()
			{
				EnqueueState(ViewModelState.Initialized);
			}

			public void Start()
			{
				EnqueueState(ViewModelState.Started);
			}

			public void Resume()
			{
				EnqueueState(ViewModelState.Resumed);
			}

			public void Pause()
			{
				EnqueueState(ViewModelState.Paused);
			}

			public void Stop()
			{
				EnqueueState(ViewModelState.Stopped);
			}

			private void EnqueueState(ViewModelState state)
			{
				_transitionsQueue.Enqueue(state);
				ProcessTransitions();
			}

			private void ProcessTransitions()
			{
				Task.Run(async () =>
				{
					lock (_isRunningMutex)
					{
						if (_isRunning)
						{
							return;
						}

						if (_transitionsQueue.IsEmpty)
						{
							return;
						}
						_isRunning = true;
					}

					await GoToNextState();
				}).Forget();
			}

			private async Task GoToNextState()
			{
				while (!_transitionsQueue.IsEmpty)
				{
					if (_transitionsQueue.TryDequeue(out ViewModelState nextState))
					{
						switch (nextState)
						{
							case ViewModelState.Created:
								break; //nothing to do, you shouldn't even go in this case
							case ViewModelState.Initialized:
								await _stateAutomata.ToState(nameof(ViewModelState.Initialized));
								break;
							case ViewModelState.Started:
								await _stateAutomata.ToState(nameof(ViewModelState.Started));
								break;
							case ViewModelState.Resumed:
								await _stateAutomata.ToState(nameof(ViewModelState.Resumed));
								break;
							case ViewModelState.Paused:
								await _stateAutomata.ToState(nameof(ViewModelState.Paused));
								break;
							case ViewModelState.Stopped:
								await _stateAutomata.ToState(nameof(ViewModelState.Stopped));
								break;
						}
					}
				}

				lock (_isRunningMutex)
				{
					if (_transitionsQueue.IsEmpty)
					{
						_isRunning = false;
						return;
					}
				}

				await GoToNextState();
			}

			private StateAutomata CreateStateGraph()
			{
				StateAutomata.Node created = new StateAutomata.Node(nameof(ViewModelState.Created));
				StateAutomata.Node initialized = new StateAutomata.Node(nameof(ViewModelState.Initialized));
				StateAutomata.Node started = new StateAutomata.Node(nameof(ViewModelState.Started));
				StateAutomata.Node resumed = new StateAutomata.Node(nameof(ViewModelState.Resumed));
				StateAutomata.Node paused = new StateAutomata.Node(nameof(ViewModelState.Paused));
				StateAutomata.Node stopped = new StateAutomata.Node(nameof(ViewModelState.Stopped));

				created.AddTransition(async () =>
				{
					try
					{
						await Run(_viewModel._isInitializing, _viewModel.Initialize);
					}
					finally
					{
						_initializationTask.TrySetResult(null);
					}
				}, initialized);
				initialized.AddTransition(() => Run(_viewModel._isStarting, _viewModel.OnStart), started);
				started.AddTransition(() => Run(_viewModel._isResuming, _viewModel.OnResume), resumed);
				resumed.AddTransition(() => Run(_viewModel._isPausing, _viewModel.OnPause), paused);
				paused.AddTransition(() => Run(_viewModel._isResuming, _viewModel.OnResume), resumed);
				paused.AddTransition(() => Run(_viewModel._isStopping, _viewModel.OnStop), stopped);
				stopped.AddTransition(() => Run(_viewModel._isStarting, _viewModel.OnStart), started);

				return new StateAutomata(created, new List<StateAutomata.Node>
				{
					created, initialized, started, resumed, paused, stopped
				});

				async Task Run(Subject<bool> navigationObserver, Func<Task> navigationMethod)
				{
					try
					{
						navigationObserver.OnNext(true);
						await navigationMethod();
					}
					finally
					{
						navigationObserver.OnNext(false);
					}
				}
			}
		}

		#endregion
	}
}
