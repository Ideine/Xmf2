using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class PercentageFormatConverter : MvxValueConverter<int, string>
	{
		protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value < 0)
			{
				return string.Empty;
			}
			else
			{
				return $"{value}%";
			}
		}
	}
}