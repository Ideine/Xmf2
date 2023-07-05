using System;
using Xmf2.Core.Exceptions;

namespace Xmf2.Core.Errors
{
	public interface IHttpErrorInterpreter
	{
		bool TryInterpretException(Exception ex, out AccessDataException intepretedException);
	}
}