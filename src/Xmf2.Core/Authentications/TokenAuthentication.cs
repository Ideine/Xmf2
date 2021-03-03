using System;

namespace Xmf2.Core.Authentications
{
	public class TokenAuthentication
	{
		public TokenAuthentication(string token, DateTime expireDate)
		{
			Token = token;
			ExpireDate = expireDate;
		}

		public string Token { get; }

		public DateTime ExpireDate { get; }
	}
}