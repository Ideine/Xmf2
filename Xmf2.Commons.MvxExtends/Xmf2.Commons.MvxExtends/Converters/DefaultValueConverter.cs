using System;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class DefaultValueConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
	        return value switch
	        {
		        null => parameter,
		        string str when string.IsNullOrEmpty(str) => parameter,
		        _ => value
	        };
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
