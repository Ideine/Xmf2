using System;
using System.Threading.Tasks;
using Xmf2.Commons.ErrorManagers;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public abstract class BaseErrorManager : IErrorManager
	{
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
			{
				callbackAction.Invoke();
			}

			return tcs.Task;
		}

		/// <returns>Indique si un dialog est en cours (la callbackAction est en attente</returns>
		protected virtual bool InternalTreatError(Exception e, bool promptErrorMessageToUser, Action callbackAction)
		{
			if (e is AccessDataException ade)
			{
				LogAccessDataException(ade);
				ade.IsLogged = true;

				if (!ade.IsUserShown && promptErrorMessageToUser)
				{
					ade.IsUserShown = true;
					return ShowMessageForAccessDataException(ade, callbackAction);
				}

				return false;
			}

			if (e is ManagedException me)
			{
				LogManagedException(me);
				me.IsLogged = true;

				if (!me.IsUserShown && promptErrorMessageToUser)
				{
					me.IsUserShown = true;
					return ShowMessageForManagedException(me, callbackAction);
				}

				return false;
			}

			LogException(e);

			if (promptErrorMessageToUser)
			{
				return ShowMessageForException(e, callbackAction);
			}

			return false;
		}

		protected virtual void LogAccessDataException(AccessDataException ade)
		{
			System.Diagnostics.Debug.WriteLine(ade);
		}

		protected virtual void LogManagedException(ManagedException me)
		{
			System.Diagnostics.Debug.WriteLine(me);
		}

		protected virtual void LogException(Exception e)
		{
			System.Diagnostics.Debug.WriteLine(e);
		}

		protected abstract bool ShowMessageForAccessDataException(AccessDataException ade, Action dialogCallbackAction);

		protected abstract bool ShowMessageForManagedException(ManagedException me, Action dialogCallbackAction);

		protected abstract bool ShowMessageForException(Exception e, Action dialogCallbackAction);
	}
}