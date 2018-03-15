namespace System
{
	public static class EnumExtensions
	{
		public static NotSupportedException GetNotSupportedException(this Enum enumValue)
		{
			return new NotSupportedException(GetNotSupportedMessage(enumValue));
		}

		public static string GetNotSupportedMessage(this Enum enumValue)
		{
			return $"This {enumValue.GetType().FullName} value is not supported yet. Value: {enumValue}";
		}
	}
}