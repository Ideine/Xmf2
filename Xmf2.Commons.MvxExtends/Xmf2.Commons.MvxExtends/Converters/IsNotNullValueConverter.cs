using MvvmCross.Platform.Converters;
using System;
using System.Globalization;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsNotNullValueConverter : IMvxValueConverter
	{
		public const string Name = "IsNotNull";

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
