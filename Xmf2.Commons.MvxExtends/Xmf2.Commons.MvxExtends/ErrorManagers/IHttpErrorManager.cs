using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
    public interface IHttpErrorManager
    {
        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
        Task ExecuteAsync(Func<Task> action);
    }
}
