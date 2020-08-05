using System;
using System.Collections;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsListEmptyConverter : MvxValueConverter<ICollection, bool>
	{
		protected override bool Convert(ICollection value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Count == 0;
		}
	}
}