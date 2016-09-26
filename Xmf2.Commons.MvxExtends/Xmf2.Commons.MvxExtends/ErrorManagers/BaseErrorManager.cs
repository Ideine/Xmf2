using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform;
using Xmf2.Commons.MvxExtends.Logs;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
    public abstract class BaseErrorManager : IErrorManager
    {
        ILogger _logger;

        public BaseErrorManager()
        {
            Mvx.TryResolve<ILogger>(out _logger);
        }

        public virtual void TreatError(Exception e, bool promptErrorMessageToUser)
        {
            var ade = e as AccessDataException;
            if (ade != null)
            {
                this.LogAccessDataException(ade);
                ade.IsLogged = true;

                if (!ade.IsUserShown && promptErrorMessageToUser)
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

                if (!me.IsUserShown && promptErrorMessageToUser)
                {
                    this.ShowMessageForManagedException(me);
                    me.IsUserShown = true;
                }
                return;
            }

            this.LogException(e);

            if (promptErrorMessageToUser)
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
