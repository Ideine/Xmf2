namespace Foundation
{
	public static class NSObjectExtensions
	{
		public static NSObject ValueForKey(this NSObject nsObject, string key)
		{
			return nsObject.ValueForKey(new NSString(key));
		}
	}
}
