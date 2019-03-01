using System;
using System.Threading.Tasks;

namespace Xmf2.Core.Errors
{
	/// <summary>
	/// CustomErrorHandler delegate type
	/// If the exception is handled by the custom error handler, it should return true and call the asyncCallback at the end, otherwise false
	/// </summary>
	public delegate Task<bool> CustomErrorHandler(Exception ex);
}