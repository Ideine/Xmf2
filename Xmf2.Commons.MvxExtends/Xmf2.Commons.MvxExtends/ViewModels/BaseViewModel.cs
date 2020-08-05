using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.ViewModels;
using Nito.AsyncEx;
using Xmf2.Commons.MvxExtends.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.ViewModels
{
	public abstract class BaseViewModel<TParameter> : MvxViewModel<TParameter>
	{
		#region ExecAsync

		private readonly AsyncLock _operationInProgressLock = new AsyncLock();
		private CancellationTokenSource _operationInProgressCts;

		protected virtual int GetOperationInProgressDefaultDelay()
		{
			return 60000;
		}

		public Task<bool> ExecAsync(Func<CancellationTokenSource, Task> action, bool withBusy = true, bool isUserAction = true)
		{
			return ExecAsync(action, GetOperationInProgressDefaultDelay(), withBusy, isUserAction);
		}

		public async Task<bool> ExecAsync(Func<CancellationTokenSource, Task> action, int millisecondsDelay, bool withBusy, bool isUserAction)
		{
			CancellationTokenSource currentCancellationToken = null;

			try
			{
				using (await _operationInProgressLock.LockAsync().ConfigureAwait(false))
				{
					if (isUserAction && _operationInProgressCts != null)
					{
						System.Diagnostics.Debug.WriteLine("Operation already in progress. ExecAsync canceled");
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
				IErrorManager errorMgr = Mvx.IoCProvider.Resolve<IErrorManager>();
				errorMgr.TreatError(e);
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

		private int _busyCount;
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

		private bool _disposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			try
			{
				if (!_disposed)
				{
					if (disposing)
					{
						// Manual release of managed resources.
						DisposeManagedObjects();
					}

					// Release unmanaged resources.
					DisposeUnmanagedObjects();

					_disposed = true;
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