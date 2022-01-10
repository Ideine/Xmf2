using System.Collections.Generic;
using System.Linq;

namespace System
{
    public static class StringHelper
	{
		public static string Join(string separator, bool removeEmptyEntries, params string[] values)
		{
			return String.Join(separator, values.Where(s => !String.IsNullOrEmpty(s)));
		}
		public static string Join(string separator, bool removeEmptyEntries, IEnumerable<string> values)
		{
			return String.Join(separator, values.Where(s => !String.IsNullOrEmpty(s)));
		}
	}
}
