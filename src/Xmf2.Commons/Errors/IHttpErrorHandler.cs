﻿using System;
using System.Threading.Tasks;

namespace Xmf2.Commons.Errors
{
	public interface IHttpErrorHandler
	{
		IObservable<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);

		IObservable<TResult> ExecuteAsync<TResult>(IObservable<TResult> source);
	}
}
