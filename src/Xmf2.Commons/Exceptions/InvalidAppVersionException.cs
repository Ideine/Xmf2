using System;

namespace Xmf2.Commons.Exceptions
{
	public class InvalidAppVersionException : ManagedException
	{
		public InvalidAppVersionException() { }

		public InvalidAppVersionException(string message) : base(message) { }

		public InvalidAppVersionException(string message, Exception innerException) : base(message, innerException) { }
	}
}