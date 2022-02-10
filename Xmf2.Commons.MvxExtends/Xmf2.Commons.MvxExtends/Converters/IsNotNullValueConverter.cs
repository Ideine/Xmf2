using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsNotNullValueConverter : IMvxValueConverter
	{
		public const string Name = "IsNotNull";

		private static IsNotNullValueConverter _instance;
		public static IsNotNullValueConverter Instance => _instance ??= new IsNotNullValueConverter();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
