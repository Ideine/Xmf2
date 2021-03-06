﻿using System;

namespace Xmf2.Core.Exceptions
{
	public class AccessDataException : Exception
	{
		public enum ErrorType
		{
			Unknown = 0,
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

		private static string GetDescriptionFor(ErrorType type)
		{
			switch (type)
			{
				case ErrorType.NoInternetConnexion: return "Access Data Exception : No Internet Connection";
				case ErrorType.Timeout: return "Access Data Exception : Timeout";
				case ErrorType.UnAuthorized: return "Access Data Exception : Unauthorized";
				case ErrorType.InvalidAppVersion: return "Access Data Exception : Invalid app version";
				case ErrorType.NotFound: return "Access Data Exception : Not found";
				case ErrorType.Forbidden: return "Access Data Exception : Forbidden";
				default:
					return "Access Data Exception : Unknown data access error";
			}
		}
	}
}