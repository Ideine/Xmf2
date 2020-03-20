using System.Linq;

namespace Xmf2.Commons.Extensions
{
	public static class StringExtensions
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool In(this string str, params string[] lst)
        {
            return lst.Contains(str);
        }
    }
}
