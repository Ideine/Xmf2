using System;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class StringFormatConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Format((string)parameter, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
