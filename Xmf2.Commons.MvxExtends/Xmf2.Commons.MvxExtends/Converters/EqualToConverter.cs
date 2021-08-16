using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class EqualToConverter : MvxValueConverter<int, bool>
	{
		protected override bool Convert(int value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter is int intParameter)
			{
				return value == intParameter;
			}
			else
			{
				return object.Equals(value, parameter);
			}
		}
	}
}