using System;
using System.Diagnostics;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public abstract class BaseErrorManager : IErrorManager
	{
		public virtual void TreatError(Exception e)
		{
			if (e is AccessDataException ade)
			{
				LogAccessDataException(ade);
				ade.IsLogged = true;

				if (!ade.IsUserShown)
				{
					ShowMessageForAccessDataException(ade);
					ade.IsUserShown = true;
				}

				return;
			}

			if (e is ManagedException me)
			{
				LogManagedException(me);
				me.IsLogged = true;

				if (!me.IsUserShown)
				{
					ShowMessageForManagedException(me);
					me.IsUserShown = true;
				}

				return;
			}

			LogException(e);
			ShowMessageForException(e);
		}

		protected virtual void LogAccessDataException(AccessDataException ade)
		{
			Debug.WriteLine(ade);
		}

		protected virtual void LogManagedException(ManagedException me)
		{
			Debug.WriteLine(me);
		}

		protected virtual void LogException(Exception e)
		{
			Debug.WriteLine(e);
		}

		protected abstract void ShowMessageForAccessDataException(AccessDataException ade);

		protected abstract void ShowMessageForManagedException(ManagedException me);

		protected abstract void ShowMessageForException(Exception e);
	}
}