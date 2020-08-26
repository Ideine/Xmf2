using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class DefaultValueConverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return parameter;
			}

			if (value is string str && string.IsNullOrEmpty(str))
			{
				return parameter;
			}

			return value;
		}
	}
}