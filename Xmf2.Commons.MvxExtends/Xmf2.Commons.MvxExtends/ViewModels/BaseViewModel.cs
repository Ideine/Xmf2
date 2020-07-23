using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Xmf2.Common.Extensions;
using Xmf2.Commons.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.ViewModels
{
	public abstract class BaseViewModel<TParameter> : MvxViewModel<TParameter> where TParameter : class
	{
		private readonly Lazy<IMvxNavigationService> _navigationService;
		protected IMvxNavigationService NavigationService => _navigationService.Value;

		protected BaseViewModel()
		{
			_navigationService = new Lazy<IMvxNavigationService>(() => Mvx.IoCProvider.Resolve<IMvxNavigationService>());
		}

		/// <summary>
		/// Gets the service.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <returns>An instance of the service.</returns>
		public TService GetService<TService>() where TService : class
		{
			return Mvx.IoCProvider.Resolve<TService>();
		}

		#region Navigation

		protected Task<bool> ShowViewModel<TViewModel>() where TViewModel : IMvxViewModel
		{
			return _navigationService.Value.Navigate<TViewModel>();
		}

		protected Task<bool> ShowViewModel<TViewModel, TTargetParameter>(TTargetParameter parameter)
			where TViewModel : IMvxViewModel<TTargetParameter>
		{
			return _navigationService.Value.Navigate<TViewModel, TTargetParameter>(parameter);
		}

		protected Task<bool> Close<TViewModel>(TViewModel viewModel)
			where TViewModel : IMvxViewModel
		{
			return _navigationService.Value.Close(viewModel);
		}

		#endregion

		#region Lifecycle

		public virtual void OnEnter() { }

		public virtual void OnPause() { }

		public virtual void OnResume() { }

		public virtual void OnStop() { }

		#endregion

		#region ExecAsync

		private readonly SemaphoreSlim _operationInProgressLock = new SemaphoreSlim(1, 1);
		private CancellationTokenSource _operationInProgressCts;

		protected virtual int GetOperationInProgressDefaultDelay()
		{
			return 60000;
		}

		public Task<bool> ExecAsync(Func<CancellationTokenSource, Task> action, bool withBusy = true, bool isUserAction = true, bool promptErrorMessageToUser = true, Action<Exception> afterErrorCallBack = null)
		{
			return ExecAsync(action, GetOperationInProgressDefaultDelay(), withBusy, isUserAction, promptErrorMessageToUser, afterErrorCallBack);
		}

		public async Task<bool> ExecAsync(Func<CancellationTokenSource, Task> action, int millisecondsDelay, bool withBusy, bool isUserAction, bool promptErrorMessageToUser, Action<Exception> afterErrorCallBack)
		{
			CancellationTokenSource currentCancellationToken = null;

			try
			{
				using (await _operationInProgressLock.LockAsync())
				{
					if (isUserAction && _operationInProgressCts != null)
					{
						Debug.WriteLine("Operation already in progress. ExecAsync canceled");
						return false;
					}

					currentCancellationToken = new CancellationTokenSource(millisecondsDelay);
					if (isUserAction)
					{
						_operationInProgressCts = currentCancellationToken;
					}
				}

				Func<CancellationTokenSource, Task> actionToLaunch = action;
				if (withBusy)
				{
					actionToLaunch = cts =>
					{
						return ExecWithBusyAsync(async () => await action(cts).ConfigureAwait(false));
					};
				}

				// on fait un Task.Run pour changer de thread
				await Task.Run(async () => await actionToLaunch(currentCancellationToken).ConfigureAwait(false)).ConfigureAwait(false);
				return true;
			}
			catch (Exception e)
			{
				IErrorManager errorMgr = GetService<IErrorManager>();
				await errorMgr.TreatErrorAsync(e, promptErrorMessageToUser).ConfigureAwait(false);
				afterErrorCallBack?.Invoke(e);
				return false;
			}
			finally
			{
				if (currentCancellationToken != null)
				{
					if (_operationInProgressCts == currentCancellationToken)
					{
						_operationInProgressCts = null;
					}

					currentCancellationToken.Dispose();
				}
			}
		}

		#endregion

		#region Gestion IsBusy

		private int _busyCount = 0;
		private readonly object _busyLock = new object();

		private bool _isBusy;

		public bool IsBusy
		{
			get => _isBusy;
			private set => SetProperty(ref _isBusy, value);
		}

		public void MoreBusy()
		{
			lock (_busyLock)
			{
				_busyCount++;
				IsBusy = true;
			}
		}

		public void LessBusy()
		{
			lock (_busyLock)
			{
				_busyCount--;

				if (_busyCount < 0)
				{
					throw new InvalidOperationException("LessBusy called without MoreBusy. BusyCount < 0");
				}
				else if (_busyCount == 0)
				{
					IsBusy = false;
				}
			}
		}

		public void ExecWithBusy(Action action)
		{
			try
			{
				MoreBusy();
				action();
			}
			finally
			{
				LessBusy();
			}
		}

		public T ExecWithBusy<T>(Func<T> func)
		{
			try
			{
				MoreBusy();
				return func();
			}
			finally
			{
				LessBusy();
			}
		}

		public async Task ExecWithBusyAsync(Func<Task> action)
		{
			try
			{
				MoreBusy();
				await action().ConfigureAwait(false);
			}
			finally
			{
				LessBusy();
			}
		}

		public async Task<T> ExecWithBusyAsync<T>(Func<Task<T>> action)
		{
			try
			{
				MoreBusy();
				return await action().ConfigureAwait(false);
			}
			finally
			{
				LessBusy();
			}
		}

		#endregion

		#region Dispose

		private bool disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (!disposed)
				{
					if (disposing)
					{
						// Manual release of managed resources.
						DisposeManagedObjects();
					}

					// Release unmanaged resources.
					DisposeUnmanagedObjects();

					disposed = true;
				}
			}
			catch { }
		}

		~BaseViewModel()
		{
			Dispose(false);
		}

		protected virtual void DisposeManagedObjects() { }

		protected virtual void DisposeUnmanagedObjects() { }

		#endregion
	}
}