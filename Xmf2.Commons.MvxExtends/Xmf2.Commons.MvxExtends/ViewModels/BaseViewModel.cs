using System;
using System.Threading;
using System.Threading.Tasks;
using Xmf2.Commons.ErrorManagers;
using Xmf2.Commons.MvxExtends.Extensions;
using System.Windows.Input;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Commands;

namespace Xmf2.Commons.MvxExtends.ViewModels
{
	public abstract class BaseViewModel : MvxViewModel
	{
		private static readonly Task CompletedTask = Task.FromResult<object>(null);
		private readonly Lazy<IErrorManager> _errorManager = new Lazy<IErrorManager>(Mvx.IoCProvider.Resolve<IErrorManager>);
		protected IErrorManager ErrorManager => _errorManager.Value;

		private ICommand _closeCommand;
		public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new MvxCommand(CloseAction));

		public virtual void OnEnter() { }

		public virtual void OnResume() { }

		public virtual void OnPause() { }

		public virtual void OnStop() { }

		protected virtual void CloseViewModel()
		{
			//Close(this); todo
			//Close(this);
		}

		protected virtual void CloseAction() => CloseViewModel();

		#region ExecAsync

		SemaphoreSlim _operationInProgressLock = new SemaphoreSlim(1);
		CancellationTokenSource _operationInProgressCTS;

		protected virtual int GetOperationInProgressDefaultDelay()
		{
			return 60000;
		}

		public Task<bool> ExecAsync(Func<CancellationTokenSource, Task> action, bool withBusy = true, bool isUserAction = true, bool promptErrorMessageToUser = true, Action<Exception> afterErrorCallBack = null)
		{
			return ExecAsync(action, this.GetOperationInProgressDefaultDelay(), withBusy, isUserAction, promptErrorMessageToUser, afterErrorCallBack);
		}

		public Task<bool> Exec(Action action, bool withBusy = true, bool isUserAction = true, bool promptErrorMessageToUser = true, Action<Exception> afterErrorCallBack = null)
		{
			return ExecAsync(cts =>
			{
				action();
				return CompletedTask;
			}, this.GetOperationInProgressDefaultDelay(), withBusy, isUserAction, promptErrorMessageToUser, afterErrorCallBack);
		}

		/// <returns>Retourne <c>true</c> si l'appel a pu être effectué, <c>false s'il a échoué.</c></returns>
		private async Task<bool> ExecAsync(Func<CancellationTokenSource, Task> action, int millisecondsDelay, bool withBusy, bool isUserAction, bool promptErrorMessageToUser, Action<Exception> afterErrorCallBack)
		{
			CancellationTokenSource currentCancellationToken = null;
			try
			{
				currentCancellationToken = new CancellationTokenSource(millisecondsDelay);
				if (isUserAction)
				{
					using (await _operationInProgressLock.LockAsync().DontStickOnThread())
					{
						if (isUserAction && _operationInProgressCTS != null)
						{
							//Mvx.Warning("User operation already in progress. ExecAsync canceled"); todo
							return false;
						}
						_operationInProgressCTS = currentCancellationToken;
					}
				}

				Func<CancellationTokenSource, Task> actionToLaunch = action;
				if (withBusy)
				{
					actionToLaunch = (cts) => ExecWithBusy(() => action(cts));
				}

				// on fait un Task.Run pour changer de thread
				await Task.Run(() => actionToLaunch(currentCancellationToken)).ConfigureAwait(false);
				return true;
			}
			catch (Exception e)
			{
				await ErrorManager.TreatErrorAsync(e, promptErrorMessageToUser).ConfigureAwait(false);
				afterErrorCallBack?.Invoke(e);
				return false;
			}
			finally
			{
				if (_operationInProgressCTS == currentCancellationToken)
				{
					_operationInProgressCTS = null;
				}
				currentCancellationToken?.Dispose();
			}
		}

		#endregion

		#region Gestion IsBusy

		private int _busyCount = 0;
		private SemaphoreSlim _busyLock = new SemaphoreSlim(1);

		private bool _isBusy;

		public bool IsBusy
		{
			get { return _isBusy; }
			private set { SetProperty(ref _isBusy, value); }
		}

		private void MoreBusy()
		{
			using (_busyLock.Lock())
			{
				_busyCount++;
				if (!IsBusy)
				{
					IsBusy = true;
				}
			}
		}

		private async Task MoreBusyAsync()
		{
			using (await _busyLock.LockAsync().DontStickOnThread())
			{
				_busyCount++;
				if (!IsBusy)
				{
					IsBusy = true;
				}
			}
		}

		private void LessBusy()
		{
			using (_busyLock.Lock())
			{
				_busyCount--;

				if (_busyCount < 0)
				{
					throw new InvalidOperationException("LessBusy called without MoreBusy. BusyCount < 0");
				}

				if (_busyCount == 0)
				{
					IsBusy = false;
				}
			}
		}

		private async Task LessBusyAsync()
		{
			using (await _busyLock.LockAsync().DontStickOnThread())
			{
				_busyCount--;

				if (_busyCount < 0)
				{
					throw new InvalidOperationException("LessBusy called without MoreBusy. BusyCount < 0");
				}

				if (_busyCount == 0)
				{
					IsBusy = false;
				}
			}
		}

		private void ExecWithBusy(Action action)
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

		private T ExecWithBusy<T>(Func<T> func)
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

		private async Task ExecWithBusy(Func<Task> action)
		{
			try
			{
				await MoreBusyAsync().DontStickOnThread();
				await action().DontStickOnThread();
			}
			finally
			{
				await LessBusyAsync().DontStickOnThread();
			}
		}

		private async Task<T> ExecWithBusy<T>(Func<Task<T>> action)
		{
			try
			{
				await MoreBusyAsync().DontStickOnThread();
				return await action().DontStickOnThread();
			}
			finally
			{
				await LessBusyAsync().DontStickOnThread();
			}
		}

		#endregion  Gestion IsBusy

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
						this.DisposeManagedObjects();
					}
					// Release unmanaged resources.
					this.DisposeUnmanagedObjects();

					disposed = true;
				}
			}
			catch
			{
				//ignored
			}
		}

		~BaseViewModel()
		{
			Dispose(false);
		}

		protected virtual void DisposeManagedObjects()
		{

		}

		protected virtual void DisposeUnmanagedObjects()
		{

		}

		#endregion Dispose
	}
}
