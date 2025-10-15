using System.Globalization;
using System.Collections.Generic;

namespace System
{
	public static class StringExtensions
	{
		public static uint HexToColorUint(this string hexString)
		{
			return uint.Parse(hexString.PadLeft(8, 'F'), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
		}
	}
}