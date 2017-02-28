using MvvmCross.Platform.Converters;
using System;
using System.Collections;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsListEmptyConverter : MvxValueConverter<ICollection, bool>
	{
		protected override bool Convert(ICollection value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.Count == 0;
		}
	}
}
