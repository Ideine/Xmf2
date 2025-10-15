using System;
namespace Xmf2.Core.Helpers
{
	public static class DateHelpers
	{
		public static (int years, int months, int days) Substract(DateTime d1, DateTime d2)
		{
			DateTime fromDate;
			DateTime toDate;

			int years = 0, months = 0, days = 0;

			if (d1 > d2)
			{
				fromDate = d2;
				toDate = d1;
			}
			else
			{
				fromDate = d1;
				toDate = d2;
			}

			if (fromDate.Year != toDate.Year)
			{
				years = toDate.Year - fromDate.Year - 1;
				fromDate = fromDate.AddYears(years);

				if (fromDate.AddYears(1) <= toDate)
				{
					++years;
					fromDate = fromDate.AddYears(1);
				}
			}

			if(fromDate.Year != toDate.Year || fromDate.Month != toDate.Month)
			{
				if(fromDate.Year != toDate.Year)
				{
					months = 12 - fromDate.Month + toDate.Month - 1;
				}
				else
				{
					months = toDate.Month - fromDate.Month - 1;
				}

				fromDate = fromDate.AddMonths(months);

				if (fromDate.AddMonths(1) <= toDate)
				{
					++months;
					fromDate = fromDate.AddMonths(1);
				}
			}

			if(fromDate.Month != toDate.Month || fromDate.Day != toDate.Day)
			{
				if(fromDate.Month != toDate.Month)
				{
					int daysInMonth = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
					days = daysInMonth - fromDate.Day + toDate.Day;
				}
				else
				{
					days = toDate.Day - fromDate.Day;
				}
			}

			return (years, months, days);
		}
	}
}
