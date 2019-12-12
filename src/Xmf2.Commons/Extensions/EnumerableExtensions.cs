using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmf2.Commons.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
			return source ?? Enumerable.Empty<T>();
        }

		/// <summary>
		/// Détermine si la séquence passée est <c>null</c> ou vide.
		/// </summary>
		/// <typeparam name="T">Type des éléments de source.</typeparam>
		/// <param name="source">IEnumerable à vérifier pour savoir si des éléments y sont présents. La valeur peut-être <c>null</c></param>
		/// <returns>false si la séquence source contient est vide ou est null; sinon, true.</returns>
		public static bool NullOrNone<T>(this IEnumerable<T> source)
		{
			return source == null || source.None();
		}

		/// <summary>
		/// Détermine si la séquence passée est vide.
		/// </summary>
		/// <typeparam name="T">Type des éléments de source.</typeparam>
		/// <param name="source">IEnumerable à vérifier pour savoir si des éléments y sont présents.</param>
		/// <returns>false si la séquence source contient est vide; sinon, true.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> a la valeur null</exception>
		public static bool None<T>(this IEnumerable<T> source)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}
			return !source.Any();
		}

		/// <summary>
		/// Détermine si la séquence passée est vide.
		/// </summary>
		/// <typeparam name="T">Type des éléments de source.</typeparam>
		/// <param name="source">IEnumerable à vérifier pour savoir si des éléments y sont présents.</param>
		/// <returns>false si la séquence source contient est vide; sinon, true.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> a la valeur null</exception>
		public static bool None<T>(this IEnumerable<T> source, Func<T,bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}
			return !source.Any(predicate);
		}

		public static IEnumerable<TOutput> SelectManySafe<TInput, TOutput>(this IEnumerable<TInput> source, Func<TInput, IEnumerable<TOutput>> func)
		{
			return source.SelectMany(x => func(x) ?? new List<TOutput>(0));
		}
	}
}
