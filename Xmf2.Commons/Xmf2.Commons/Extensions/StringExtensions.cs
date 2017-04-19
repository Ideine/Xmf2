using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		/// <summary>
		/// Concatène les membres d'une collection System.Collections.Generic.IEnumerable`1
		/// construite de type System.String, en utilisant le séparateur spécifié entre chaque
		/// membre.
		/// </summary>
		/// <param name="values">Collection qui contient les chaînes à concaténer.</param>
		/// <param name="separator">Chaîne à utiliser comme séparateur. separator est inclus dans la chaîne retournée  uniquement si values contient plusieurs éléments.</param>
		public static string Join(this IEnumerable<string> values, string separator)
		{
			return String.Join(separator, values);
		}
    }
}
