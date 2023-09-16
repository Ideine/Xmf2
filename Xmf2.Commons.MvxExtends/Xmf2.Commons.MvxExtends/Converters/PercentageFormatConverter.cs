using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class PercentageFormatConverter : MvxValueConverter<int, string>
	{
		protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
		{
			return value < 0 ? string.Empty : $"{value}%";
		}
	}
}