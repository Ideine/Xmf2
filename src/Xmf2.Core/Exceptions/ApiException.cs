using System;

namespace Xmf2.Core.Exceptions
{
	public class ApiException : Exception
	{
		public ApiException(string errorCode, string errorMessage)
		{
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		public ApiException(string message, string errorCode, string errorMessage) : base(message)
		{
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		public ApiException(string message, Exception innerException, string errorCode, string errorMessage) : base(message, innerException)
		{
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		public string ErrorCode { get; }

		public string ErrorMessage { get; }
	}
}