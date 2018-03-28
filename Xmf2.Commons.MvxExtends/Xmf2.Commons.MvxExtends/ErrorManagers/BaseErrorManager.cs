using System;
using MvvmCross.Platform;
using Xmf2.Commons.MvxExtends.Logs;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public abstract class BaseErrorManager : IErrorManager	
    {
        protected ILogger _logger;

        public BaseErrorManager()
        {
            Mvx.TryResolve<ILogger>(out _logger);
        }

        public virtual void TreatError(Exception e)
        {
            var ade = e as AccessDataException;
            if (ade != null)
            {
                this.LogAccessDataException(ade);
                ade.IsLogged = true;

                if (!ade.IsUserShown)
                {
                    this.ShowMessageForAccessDataException(ade);
                    ade.IsUserShown = true;
                }
                return;
            }

            var me = e as ManagedException;
            if (me != null)
            {
                this.LogManagedException(me);
                me.IsLogged = true;

                if (!me.IsUserShown)
                {
                    this.ShowMessageForManagedException(me);
                    me.IsUserShown = true;
                }
                return;
            }

            this.LogException(e);
            this.ShowMessageForException(e);
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

        protected abstract void ShowMessageForAccessDataException(AccessDataException ade);

        protected abstract void ShowMessageForManagedException(ManagedException me);

        protected abstract void ShowMessageForException(Exception e);

    }
}
