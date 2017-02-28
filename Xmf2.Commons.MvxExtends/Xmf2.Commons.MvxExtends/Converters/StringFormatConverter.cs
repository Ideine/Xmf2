using MvvmCross.Platform.Converters;
using System;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class StringFormatConverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.Format((string)parameter, value);
		}
	}
}
