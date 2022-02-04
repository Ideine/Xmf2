using System;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsStringEmptyConverter : MvxValueConverter<string, bool>
	{
		private static IsStringEmptyConverter _instance;
		public static IsStringEmptyConverter Instance => _instance ?? (_instance = new IsStringEmptyConverter());

		public const string Name = "IsStringEmpty";

		protected override bool Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.IsNullOrEmpty(value);
		}
	}
}
