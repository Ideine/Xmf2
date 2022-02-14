using System;
using System.Threading.Tasks;

namespace Xmf2.Commons.Errors
{
	public interface IErrorHandler
	{
		IObservable<TResult> Execute<TResult>(Func<TResult> action, CustomErrorHandler errorHandler = null);

		IObservable<TResult> Execute<TResult>(IObservable<TResult> source, CustomErrorHandler errorHandler = null);

		IObservable<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action, CustomErrorHandler errorHandler = null);
	}
}
