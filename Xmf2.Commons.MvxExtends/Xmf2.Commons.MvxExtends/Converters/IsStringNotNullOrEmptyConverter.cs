using MvvmCross.Platform.Converters;
using System;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsStringNotNullOrEmptyConverter : MvxValueConverter
	{
		public const string Name = "IsStringNotNullOrEmpty";

		public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !String.IsNullOrEmpty(value as string);
		}
	}
}
