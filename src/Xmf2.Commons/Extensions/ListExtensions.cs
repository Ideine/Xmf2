namespace System.Collections.Generic
{
	public static class ListExtensions
	{
		public static void AddRange<T>(this List<T> list, params T[] items)
		{
			list.AddRange(items);
		}

		public static void SortDescending<T>(this List<T> list, IComparer<T> comparer)
		{
			list.Sort((x, y) => comparer.Compare(y, x)/*(x,y) inversion is on purpose*/);
		}
	}
}
