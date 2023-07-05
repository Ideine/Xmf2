using System;

namespace Xmf2.Core.Exceptions
{
	public class AccessDataException : Exception
	{
		public enum ErrorType
		{
			//Unknown = 0,
			NoInternetConnexion = 1,
			UnAuthorized = 2,
			Timeout = 3,
			InvalidAppVersion = 4,
			NotFound = 5,
			Forbidden = 6
		}

		public ErrorType Type { get; }

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
			ErrorType.Forbidden => "Access Data Exception : Forbidden",
			_ => "Access Data Exception : Unknown data access error"
		};
	}
}