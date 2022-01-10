using MvvmCross.Converters;
using System;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class StringFormatConverter : MvxValueConverter
	{
		private static StringFormatConverter _instance;
		public static StringFormatConverter Instance => _instance ?? (_instance = new StringFormatConverter());

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.Format((string)parameter, value);
		}
	}
}
