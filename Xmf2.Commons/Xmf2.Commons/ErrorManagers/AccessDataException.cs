using System;

namespace Xmf2.Commons.ErrorManagers
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

		public AccessDataException(ErrorType type) : base(GetDescriptionFor(type))
		{
			this.Type = type;
		}

		public AccessDataException(ErrorType type, Exception innerException) : base(GetDescriptionFor(type), innerException)
		{
			this.Type = type;
		}

		private static string GetDescriptionFor(ErrorType type)
		{
			switch (type)
			{
				case ErrorType.NoInternetConnexion: return "Access Data Exception : No Internet Connection";
				case ErrorType.Timeout: return "Access Data Exception : Timeout";
				case ErrorType.UnAuthorized: return "Access Data Exception : Unauthorized";
				default:
					return "Access Data Exception : Unknown data access error";
			}
		}
	}
}
