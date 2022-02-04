using System;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IfNotNullConverter : MvxValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value == null ? null : parameter;
		}
	}
}
