using System;
using System.Threading.Tasks;

namespace Xmf2.Core.Errors
{
	public interface IErrorHandler
	{
		Task<bool> Handle(Exception ex, CustomErrorHandler errorHandler = null);
	}
}