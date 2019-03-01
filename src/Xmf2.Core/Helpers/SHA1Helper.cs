using System;
using System.Security.Cryptography;
using System.Text;

namespace Xmf2.Core.Helpers
{
	public class SHA1Helper
	{
		public static Guid Hash(string id)
		{
			using (SHA1 algorithm = SHA1.Create())
			{
				string x = ToHexString(algorithm.ComputeHash(Encoding.UTF8.GetBytes(id)));
				return Guid.Parse(x.Substring(0, 32));
			}
		}

		private static string ToHexString(byte[] input)
		{
			StringBuilder formatted = new StringBuilder(2 * input.Length);
			foreach (byte b in input)
			{
				formatted.AppendFormat("{0:X2}", b);
			}

			return formatted.ToString();
		}
	}
}
