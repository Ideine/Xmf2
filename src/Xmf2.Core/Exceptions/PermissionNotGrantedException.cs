using System;

namespace Xmf2.Core.Exceptions
{
	public class PermissionNotGrantedException : Exception
	{
		public PermissionNotGrantedException(string message) : base(message) { }

		public PermissionNotGrantedException(string message, Exception innerException) : base(message, innerException) { }
	}
}