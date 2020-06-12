using System;
using MvvmCross.Platform.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class PercentageFormatConverter : MvxValueConverter<int, string>
	{
		protected override string Convert(int value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value < 0 ? string.Empty : $"{value}%";
		}
	}
}