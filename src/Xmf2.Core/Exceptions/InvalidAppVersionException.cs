using System;

namespace Xmf2.Core.Exceptions
{
	public class InvalidAppVersionException : Exception
	{
		public InvalidAppVersionException()
			: base() { }

		public InvalidAppVersionException(string message)
			: base(message) { }

		public InvalidAppVersionException(string message, Exception innerException)
			: base(message, innerException) { }
	}
}