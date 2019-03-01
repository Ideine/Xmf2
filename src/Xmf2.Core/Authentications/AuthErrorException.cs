using System;

namespace Xmf2.Core.Authentications
{
	public class AuthErrorException : Exception
	{
		public AuthErrorReason ErrorType { get; }

		public AuthErrorException(string message, AuthErrorReason errorType) : base(message)
		{
			ErrorType = errorType;
		}

		public AuthErrorException(string message) : base(message) { }
	}
}