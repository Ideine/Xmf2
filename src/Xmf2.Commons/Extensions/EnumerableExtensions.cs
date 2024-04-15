using System.Linq;

namespace System.Collections.Generic
{
	public static class EnumerableExtensions
    {
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source)
        {
			return source ?? Enumerable.Empty<T>();
        }

		public static bool AnyIsTrue(this IEnumerable<bool> source)
		{
			return source.Any(item => item);
		}

		public static bool NotNullOrEmpty<T>(this IEnumerable<T> source)
		{
			return source != null && source.Any();
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

		public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T exclude)
		{
			return source.Except(new T[] { exclude });
		}

		public static T Aggregate<T>(this IEnumerable<T> source, Func<T, T, T> func)
		{
			var enumerator = source.GetEnumerator();
			return enumerator.MoveNext()
				 ? Aggregate(enumerator, enumerator.Current, func)
				 : default(T);
		}

		public static HashSet<T> SymmetricExceptWith<T>(this IEnumerable<T> setA, IEnumerable<T> setB)
		{
			var result = new HashSet<T>(setA);
			result.SymmetricExceptWith(setA);
			return result;
		}

		private static T Aggregate<T>(this IEnumerator<T> enumerator, T acc, Func<T, T, T> func)
		{
			while (enumerator.MoveNext())
			{
				acc = func(acc, enumerator.Current);
			}
			return acc;
		}
	}
}
