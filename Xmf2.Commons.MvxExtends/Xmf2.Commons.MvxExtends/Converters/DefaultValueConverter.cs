using MvvmCross.Platform.Converters;
using System;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class DefaultValueConverter : MvxValueConverter
	{
		private static DefaultValueConverter _instance;
		public static DefaultValueConverter Instance => _instance ?? (_instance = new DefaultValueConverter());

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
