namespace System.Collections.Generic
{
	public static class ListExtensions
	{
		public static void AddRange<T>(this List<T> list, params T[] items)
		{
			list.AddRange(items);
		}
	}
}
