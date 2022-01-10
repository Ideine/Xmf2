using MvvmCross.Converters;
using System;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsStringNotNullOrEmptyConverter : MvxValueConverter
	{
		private static IsStringNotNullOrEmptyConverter _instance;
		public static IsStringNotNullOrEmptyConverter Instance => _instance ?? (_instance = new IsStringNotNullOrEmptyConverter());

		public const string Name = "IsStringNotNullOrEmpty";

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !String.IsNullOrEmpty(value as string);
		}
	}
}
