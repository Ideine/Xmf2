namespace System
{
	public static class EnumExtensions
	{
		public static NotSupportedException GetNotSupportedException(this Enum enumValue)
		{
			return new NotSupportedException(GetNotSupportedMessage(enumValue));
		}
		public static InvalidOperationException GetInvalidOperationException(this Enum enumValue)
		{
			return new InvalidOperationException(GetInvalidOperationMessage(enumValue));
		}

		public static string GetNotSupportedMessage(this Enum enumValue)
		{
			return string.Format("This {0} value is not supported yet. Value: {1}"
				, enumValue.GetType().FullName
				, enumValue);
		}

		public static string GetInvalidOperationMessage(this Enum enumValue)
		{
			return string.Format("This {0} value is not valid. Value: {1}"
				, enumValue.GetType().FullName
				, enumValue);
		}
	}
}