using System;
using Foundation;

namespace Xmf2.Core.iOS.Extensions
{
	public static class NSDateExtension
	{
		private static readonly DateTime _reference = new DateTime(2001, 1, 1, 0, 0, 0);
		private static readonly DateTime _referenceForNSDate = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Convert NSDate to DateTime
		/// </summary>
		public static DateTime ToDateTime(this NSDate date)
		{
			return _reference.AddSeconds(date.SecondsSinceReferenceDate).ToLocalTime();
		}

		public static NSDate ToNSDate(this DateTime date)
		{
			return NSDate.FromTimeIntervalSinceReferenceDate((date.ToUniversalTime() - _referenceForNSDate).TotalSeconds);
		}
	}
}
