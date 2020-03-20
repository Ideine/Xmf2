using System;
using MvvmCross.Platform.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IfEnumEqualsConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			return Enum.GetName(value.GetType(), value) == (string)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
