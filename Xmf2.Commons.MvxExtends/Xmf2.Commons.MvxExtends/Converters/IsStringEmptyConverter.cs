using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsStringEmptyConverter : MvxValueConverter<string, bool>
	{
		protected override bool Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return string.IsNullOrEmpty(value);
		}
	}
}