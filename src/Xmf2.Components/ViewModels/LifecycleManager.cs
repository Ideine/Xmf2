using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xmf2.Components.Interfaces;

namespace Xmf2.Components.ViewModels
{
	internal class LifecycleManager : ILifecycleManager
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

		private readonly ILifecycle _viewModel;
		private readonly TaskCompletionSource<object> _initializationTask;
		private readonly StateAutomata _stateAutomata;
		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

		public LifecycleManager(ILifecycle viewModel)
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
			GoToState(state).ConfigureAwait(false);
		}


		private async Task GoToState(ViewModelState nextState)
		{
			await _semaphore.WaitAsync();
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

			_semaphore.Release();
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
					await Run(_viewModel.Initialize);
				}
				catch (Exception e)
				{
					_initializationTask.TrySetException(e);
				}
				finally
				{
					_initializationTask.TrySetResult(null);
				}
			}, initialized);
			initialized.AddTransition(() => Run(_viewModel.OnStart), started);
			started.AddTransition(() => Run(_viewModel.OnResume), resumed);
			resumed.AddTransition(() => Run(_viewModel.OnPause), paused);
			paused.AddTransition(() => Run(_viewModel.OnResume), resumed);
			paused.AddTransition(() => Run(_viewModel.OnStop), stopped);
			stopped.AddTransition(() => Run(_viewModel.OnStart), started);

			return new StateAutomata(created, new List<StateAutomata.Node>
			{
				created,
				initialized,
				started,
				resumed,
				paused,
				stopped
			});

			async Task Run(Func<Task> navigationMethod)
			{
				await navigationMethod();
			}
		}
	}
}