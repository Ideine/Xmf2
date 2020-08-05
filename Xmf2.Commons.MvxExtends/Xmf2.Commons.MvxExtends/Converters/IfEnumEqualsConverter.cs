using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IfEnumEqualsConverter : IMvxValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Enum.GetName(value.GetType(), value) == (string)parameter;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}