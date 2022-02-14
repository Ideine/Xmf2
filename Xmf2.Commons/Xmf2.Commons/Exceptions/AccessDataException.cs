using System;

namespace Xmf2.Commons.Exceptions
{
	public class AccessDataException : ManagedException
	{
		public enum ErrorType
		{
			Unknown = 0,
			NoInternetConnexion = 1,
			UnAuthorized = 2,
			Timeout = 3,
			InvalidAppVersion = 4,
			NotFound = 5
		}

		public ErrorType Type { get; private set; }

		public AccessDataException(ErrorType type) : base(GetDescriptionFor(type))
		{
			Type = type;
		}

		public AccessDataException(ErrorType type, Exception innerException) : base(GetDescriptionFor(type), innerException)
		{
			Type = type;
		}

		private static string GetDescriptionFor(ErrorType type) => type switch
		{
			ErrorType.NoInternetConnexion => "Access Data Exception : No Internet Connection",
			ErrorType.Timeout => "Access Data Exception : Timeout",
			ErrorType.UnAuthorized => "Access Data Exception : Unauthorized",
			ErrorType.InvalidAppVersion => "Access Data Exception : Invalid app version",
			ErrorType.NotFound => "Access Data Exception : Not found",
			_ => "Access Data Exception : Unknown data access error"
		};
	}
}