using System;

namespace Foundation
{
	public static class NSDateExtension
	{
		private static readonly DateTime _reference = new DateTime(2001, 1, 1, 0, 0, 0);

		/// <summary>
		/// Convert NSDate to DateTime
		/// </summary>
		public static DateTime ToDateTime(this NSDate date)
		{
			return _reference.AddSeconds(date.SecondsSinceReferenceDate).ToLocalTime();
		}
	}
}