using Foundation;

namespace Xmf2.Core.iOS.Extensions
{
	public static class CollectionExtensions
	{
		public static bool ContainsKey<T>(this NSDictionary source, string key)
		{
			using (var nsKey = new NSString(key))
			{
				return source.ContainsKey(nsKey);
			}
		}

		public static bool TryGet<T>(this NSDictionary source, NSString key, out T value) where T : class
		{
			if (source.ContainsKey(key) && source[key] is T result)
			{
				value = result;
				return value != null;
			}
			value = default;
			return false;
		}

		public static bool TryGet<T>(this NSDictionary source, string key, out T value) where T : class
		{
			using (var nsKey = new NSString(key))
			{
				if (source.ContainsKey(nsKey) && source[nsKey] is T result)
				{
					value = result;
					return true;
				}
			}
			value = default;
			return false;
		}
	}
}