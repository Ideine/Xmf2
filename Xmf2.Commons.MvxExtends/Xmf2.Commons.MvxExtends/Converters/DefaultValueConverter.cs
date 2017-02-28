using MvvmCross.Platform.Converters;
using System;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class DefaultValueConverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null || string.IsNullOrEmpty(value as string))
			{
				return parameter;
			}
			return value;
		}
	}
}
