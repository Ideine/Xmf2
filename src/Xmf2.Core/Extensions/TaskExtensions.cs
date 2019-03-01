using System;
using System.Threading.Tasks;
using Xmf2.Core.Errors;

namespace Xmf2.Core.Extensions
{
	public static class TaskExtensions
	{
		/// <summary>
		/// A utiliser au lieu de la création d'une méthode <c>async void</c>.
		/// </summary>
		/// <example>
		/// <code>
		///		public void MyMethod() { Task taskThatMayCrash = DoSomething(); } //Wrong because we don't handle error properly.
		///		public async void MyMethod() { await DoSomething(); } //Error would be rethrown on thread that called MyMethod, so be carefull (Do not use on UIThread)
		///		public void MyMethod() { DoSomething().FireAndForget(_someHandler); }//GOOD, even on UI Thread. :-)
		///		https://johnthiriet.com/removing-async-void/
		/// </code>
		/// </example>
		public static async void FireAndForget(this Task task, IErrorHandler handler = null)
		{
			try
			{
				await task;
			}
			catch (Exception ex)
			{
				if (handler != null)
				{
					await handler.Handle(ex);
				}
			}
		}

		public static Task<TResult> AsTask<TResult>(this TResult result) => Task.FromResult(result);
	}
}