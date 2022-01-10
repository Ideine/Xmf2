using System;

namespace Xmf2.Commons.ErrorManagers
{
    public class InvalidAppVersionException : ManagedException
	{
		public InvalidAppVersionException()
			: base()
		{ }

		public InvalidAppVersionException(string message)
			: base(message)
		{ }

		public InvalidAppVersionException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
