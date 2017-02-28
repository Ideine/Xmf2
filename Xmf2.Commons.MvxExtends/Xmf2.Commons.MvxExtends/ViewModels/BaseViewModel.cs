﻿using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using Xmf2.Commons.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.ViewModels
{
	public abstract class BaseViewModel : MvxViewModel
	{
		/// <summary>
		/// Gets the service.
		/// </summary>
		/// <typeparam name="TService">The type of the service.</typeparam>
		/// <returns>An instance of the service.</returns>
		public TService GetService<TService>() where TService : class
		{
			return Mvx.Resolve<TService>();
		}

		/// <summary>
		/// Checks if a property already matches a desired value.  Sets the property and
		/// notifies listeners only when necessary.
		/// </summary>
		/// <typeparam name="T">Type of the property.</typeparam>
		/// <param name="backingStore">Reference to a property with both getter and setter.</param>
		/// <param name="value">Desired value for the property.</param>
		/// <param name="property">The property.</param>
		protected void SetProperty<T>(ref T backingStore, T value, Expression<Func<T>> property)
		{
			if (Equals(backingStore, value))
			{
				return;
			}

			backingStore = value;

			this.RaisePropertyChanged(property);
		}

		public virtual void OnEnter()
		{

		}

		public virtual void OnResume()
		{

		}

		public virtual void OnPause()
		{

		}

		public virtual void OnStop()
		{

		}

		#region ExecAsync

		SemaphoreSlim _operationInProgressLock = new SemaphoreSlim(1);
		CancellationTokenSource _operationInProgressCTS;

		protected virtual int GetOperationInProgressDefaultDelay()
		{
			return 60000;
		}

		public Task<bool> ExecAsync(Func<CancellationTokenSource, Task> action, bool withBusy = true, bool isUserAction = true, bool promptErrorMessageToUser = true, Action<Exception> afterErrorCallBack = null)
		{
			return this.ExecAsync(action, this.GetOperationInProgressDefaultDelay(), withBusy, isUserAction, promptErrorMessageToUser, afterErrorCallBack);
		}

		/// <returns>Retourne <c>true</c> si l'appel a pu être effectué, <c>false s'il a échoué.</c></returns>
		public async Task<bool> ExecAsync(Func<CancellationTokenSource, Task> action, int millisecondsDelay, bool withBusy, bool isUserAction, bool promptErrorMessageToUser, Action<Exception> afterErrorCallBack)
		{
			CancellationTokenSource currentCancellationToken = null;
			try
			{
				await _operationInProgressLock.WaitAsync();
				try
				{
					if (isUserAction && _operationInProgressCTS != null)
					{
						Mvx.Warning("Operation already in progress. ExecAsync canceled");
						return false;
					}

					currentCancellationToken = new CancellationTokenSource(millisecondsDelay);
					if (isUserAction)
						_operationInProgressCTS = currentCancellationToken;
				}
				finally
				{
					_operationInProgressLock.Release();
				}

				var actionToLaunch = action;
				if (withBusy)
				{
					actionToLaunch = (cts) =>
					{
						return this.ExecWithBusyAsync(async () => await action(cts).ConfigureAwait(false));
					};
				}

				// on fait un Task.Run pour changer de thread
				await Task.Run(async () => await actionToLaunch(currentCancellationToken).ConfigureAwait(false)).ConfigureAwait(false);
				return true;
			}
			catch (Exception e)
			{
				var errorMgr = this.GetService<IErrorManager>();
				await errorMgr.TreatErrorAsync(e, promptErrorMessageToUser).ConfigureAwait(false);
				afterErrorCallBack?.Invoke(e);
				return false;
			}
			finally
			{
				if (currentCancellationToken != null)
				{
					if (_operationInProgressCTS == currentCancellationToken)
						_operationInProgressCTS = null;
					currentCancellationToken.Dispose();
				}
			}
		}

		#endregion

		#region Gestion IsBusy

		private int _busyCount = 0;
		private object _busyLock = new object();

		private bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			private set { this.SetProperty(ref _isBusy, value, () => this.IsBusy); }
		}

		public void MoreBusy()
		{
			lock (_busyLock)
			{
				_busyCount++;
				this.IsBusy = true;
			}
		}

		public void LessBusy()
		{
			lock (_busyLock)
			{
				_busyCount--;

				if (_busyCount < 0)
					throw new InvalidOperationException("LessBusy called without MoreBusy. BusyCount < 0");
				else if (_busyCount == 0)
					this.IsBusy = false;
			}
		}

		public void ExecWithBusy(Action action)
		{
			try
			{
				this.MoreBusy();
				action();
			}
			finally
			{
				this.LessBusy();
			}
		}

		public T ExecWithBusy<T>(Func<T> func)
		{
			try
			{
				this.MoreBusy();
				return func();
			}
			finally
			{
				this.LessBusy();
			}
		}

		public async Task ExecWithBusyAsync(Func<Task> action)
		{
			try
			{
				this.MoreBusy();
				await action().ConfigureAwait(false);
			}
			finally
			{
				this.LessBusy();
			}
		}

		public async Task<T> ExecWithBusyAsync<T>(Func<Task<T>> action)
		{
			try
			{
				this.MoreBusy();
				return await action().ConfigureAwait(false);
			}
			finally
			{
				this.LessBusy();
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
			catch { }
		}

		~BaseViewModel()
		{
			Dispose(false);
		}

		protected virtual void DisposeManagedObjects()
		{ }

		protected virtual void DisposeUnmanagedObjects()
		{ }

		#endregion Dispose
	}
}
