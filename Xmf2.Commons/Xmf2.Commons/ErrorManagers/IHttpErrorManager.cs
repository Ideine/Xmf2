using System;
using System.Threading.Tasks;

namespace Xmf2.Commons.ErrorManagers
{
    public interface IHttpErrorManager
    {
        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
        Task ExecuteAsync(Func<Task> action);
    }
}
