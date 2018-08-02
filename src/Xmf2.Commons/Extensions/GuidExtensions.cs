namespace System
{
	public static class GuidExtensions
	{
		public static Guid AsGuid(this string input)
		{
			return Guid.Parse(input);
		}
	}
}
