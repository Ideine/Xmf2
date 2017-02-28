using System;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Xmf2.Commons.ErrorManagers;
using Xmf2.Commons.Logs;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public abstract class BaseErrorManager : IErrorManager
	{
		protected ILogger _logger;

		public BaseErrorManager()
		{
			Mvx.TryResolve<ILogger>(out _logger);
		}

		public virtual void TreatError(Exception e, bool promptErrorMessageToUser)
		{
			InternalTreatError(e, promptErrorMessageToUser, null);
		}

		public virtual Task TreatErrorAsync(Exception e, bool promptErrorMessageToUser)
		{
			var tcs = new TaskCompletionSource<object>();
			Action callbackAction = () =>
			{
				tcs.SetResult(null);
			};

			bool dialogInProgress = InternalTreatError(e, promptErrorMessageToUser, callbackAction);

			if (!dialogInProgress)
				callbackAction.Invoke();

			return tcs.Task;
		}

		/// <returns>Indique si un dialog est en cours (la callbackAction est en attente</returns>
		protected virtual bool InternalTreatError(Exception e, bool promptErrorMessageToUser, Action callbackAction)
		{
			var ade = e as AccessDataException;
			if (ade != null)
			{
				this.LogAccessDataException(ade);
				ade.IsLogged = true;

				if (!ade.IsUserShown && promptErrorMessageToUser)
				{
					ade.IsUserShown = true;
					return this.ShowMessageForAccessDataException(ade, callbackAction);
				}
				return false;
			}

			var me = e as ManagedException;
			if (me != null)
			{
				this.LogManagedException(me);
				me.IsLogged = true;

				if (!me.IsUserShown && promptErrorMessageToUser)
				{
					me.IsUserShown = true;
					return ShowMessageForManagedException(me, callbackAction);
				}
				return false;
			}

			this.LogException(e);

			if (promptErrorMessageToUser)
				return this.ShowMessageForException(e, callbackAction);

			return false;
		}

		protected virtual void LogAccessDataException(AccessDataException ade)
		{
			_logger?.LogError(ade);
		}

		protected virtual void LogManagedException(ManagedException me)
		{
			_logger?.LogError(me);
		}

		protected virtual void LogException(Exception e)
		{
			_logger?.LogCritical();
		}

		protected abstract bool ShowMessageForAccessDataException(AccessDataException ade, Action dialogCallbackAction);

		protected abstract bool ShowMessageForManagedException(ManagedException me, Action dialogCallbackAction);

		protected abstract bool ShowMessageForException(Exception e, Action dialogCallbackAction);
	}
}
