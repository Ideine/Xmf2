using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System.Linq
{
	public static class IEnumerableExtensions
	{
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
		public static bool None<T>(this IEnumerable<T> source, Func<T, bool> predicate)
		{
			if (source == null)
			{
				throw new ArgumentNullException(nameof(source));
			}
			return !source.Any(predicate);
		}

		public static IEnumerable<T> Except<T, TCompared>(this IEnumerable<T> source, T element, Func<T, TCompared> comparedBy)
		{
			return Except(source, new T[] { element }, comparedBy, comparedBy);
		}

		public static IEnumerable<TFirst> Except<TFirst, TSecond, TCompared>(
			this IEnumerable<TFirst> first,
			IEnumerable<TSecond> second,
			Func<TFirst, TCompared> firstSelect,
			Func<TSecond, TCompared> secondSelect)
		{
			return Except(first, second, firstSelect, secondSelect, EqualityComparer<TCompared>.Default);
		}

		public static IEnumerable<TFirst> Except<TFirst, TSecond, TCompared>(
			this IEnumerable<TFirst> first,
			IEnumerable<TSecond> second,
			Func<TFirst, TCompared> firstSelect,
			Func<TSecond, TCompared> secondSelect,
			IEqualityComparer<TCompared> comparer)
		{
			if (first == null)
			{
				throw new ArgumentNullException("first");
			}

			if (second == null)
			{
				throw new ArgumentNullException("second");
			}

			return ExceptIterator<TFirst, TSecond, TCompared>(first, second, firstSelect, secondSelect, comparer);
		}

		private static IEnumerable<TFirst> ExceptIterator<TFirst, TSecond, TCompared>(
			IEnumerable<TFirst> first,
			IEnumerable<TSecond> second,
			Func<TFirst, TCompared> firstSelect,
			Func<TSecond, TCompared> secondSelect,
			IEqualityComparer<TCompared> comparer)
		{
			HashSet<TCompared> set = new HashSet<TCompared>(second.Select(secondSelect), comparer);
			foreach (TFirst tSource1 in first)
			{
				if (set.Add(firstSelect(tSource1)))
				{
					yield return tSource1;
				}
			}
		}

		public static bool IsNullOrEmpty<T>(this IEnumerable<T> source) => source is null || source.None();
		public static bool NotNullOrEmpty<T>(this IEnumerable<T> source) => !source.IsNullOrEmpty();

		public static async Task<IEnumerable<T>> Where<T>(this IEnumerable<T> source, Func<T, Task<bool>> predicate)
		{
			var itemTaskList = source.Select(item => new { Item = item, KeepItem = predicate.Invoke(item) });
			await Task.WhenAll(itemTaskList.Select(x => x.KeepItem));
			return itemTaskList.Where(x => x.KeepItem.Result)
							   .Select(x => x.Item);
		}

		public static IEnumerable<T> Traverse<T>(this T v, Func<T, IEnumerable<T>> bySelector)
		{
			yield return v;
			foreach (var selected in bySelector(v))
			{
				foreach (var subSelected in selected.Traverse(bySelector))
				{
					yield return subSelected;
				}
			}
		}
		public static IEnumerable<T> Traverse<T>(this T v, Func<T, T> bySelector)
		{
			T nextNode = v;
			while (nextNode != null)
			{
				yield return nextNode;
				nextNode = bySelector(nextNode);
			}
		}

		public static bool TryFindIndex<T>(this List<T> list, Predicate<T> match, out int index)
		{
			return (index = list.FindIndex(match)) != -1;
		}
		public static bool TryFindIndex<T>(this List<T> list, int startIndex, Predicate<T> match, out int index)
		{
			if (startIndex >= list.Count)
			{
				index = -1;
				return false;
			}
			else
			{
				return (index = list.FindIndex(startIndex, match)) != -1;
			}
		}
		public static bool TryFindByIndex<T>(this List<T> list, Predicate<T> match, out T found)
		{
			if (list.TryFindIndex(match, out int index))
			{
				found = list[index];
				return true;
			}
			else
			{
				found = default(T);
				return false;
			}
		}
		public static bool TryFindByIndex<T>(this List<T> list, int startIndex, Predicate<T> match, out T found)
		{
			if (list.TryFindIndex(startIndex, match, out int index))
			{
				found = list[index];
				return true;
			}
			else
			{
				found = default(T);
				return false;
			}
		}
	}
}
