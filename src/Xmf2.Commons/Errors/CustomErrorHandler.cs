using System;

namespace Xmf2.Commons.Errors
{
	/// <summary>
	/// CustomErrorHandler delegate type
	/// If the exception is handled by the custom error handler, it should return true and call the asyncCallback at the end, otherwise false
	/// </summary>
	public delegate bool CustomErrorHandler(Exception ex, Action asyncCallback);
}