using MvvmCross.Platform.Converters;
using System;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsStringEmptyConverter : MvxValueConverter<string, bool>
	{
		protected override bool Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.IsNullOrEmpty(value);
		}
	}
}
