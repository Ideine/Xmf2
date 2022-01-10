using System;
using System.Collections.Generic;
using System.Linq;

namespace Xmf2.Commons.Extensions
{
	public static class StringExtensions
	{
		/// <summary>
		/// Retourne une valeur qui indique si l'objet System.String spécifié apparaît dans cette chaîne.
		/// </summary>
		/// <param name="str">String sur laquelle effectuer la recherche.</param>
		/// <param name="value">Chaîne à rechercher.</param>
		/// <param name="comparisonType">Une des valeurs d'énumération qui spécifie le mode de comparaison des chaînes.</param>
		/// <returns>true si le paramètre value apparaît dans cette chaîne, ou si value est la chaîne vide ("") ; sinon, false.</returns>
		/// <exception cref="System.ArgumentNullException"><paramref name="Value"/> ou <paramref name="str"/> a la valeur null</exception>
		public static bool Contains(this string str, string value, StringComparison comparisonType)
		{
			if (str == null)
			{
				throw new ArgumentNullException(nameof(str));
			}
			return (str.IndexOf(value, comparisonType) != -1);
		}

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
