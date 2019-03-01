using System;
using Xmf2.Core.Exceptions;

namespace Xmf2.Core.Errors
{
	public interface IHttpErrorInterpreter
	{
		AccessDataException InterpretException(Exception exception);
	}
}