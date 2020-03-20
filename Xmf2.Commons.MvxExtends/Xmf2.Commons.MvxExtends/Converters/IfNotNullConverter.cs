using System;
using MvvmCross.Platform.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IfNotNullConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value != null ? parameter : null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
