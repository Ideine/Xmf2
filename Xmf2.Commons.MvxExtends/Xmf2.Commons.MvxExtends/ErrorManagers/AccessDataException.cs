using System;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public class AccessDataException : ManagedException
	{
		public enum ErrorType
		{
			Unknown = 0,
			NoInternetConnexion = 1,
			UnAuthorized = 2,
			Timeout = 3
		}

		public ErrorType Type { get; private set; }

		public AccessDataException(ErrorType type) : base("Data access error")
		{
			Type = type;
		}

		public AccessDataException(ErrorType type, Exception innerException) : base("Unknown data access error", innerException)
		{
			Type = type;
		}
	}
}