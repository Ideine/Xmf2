﻿using MvvmCross.Platform.Converters;
using System;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class IsStringEmptyConverter : MvxValueConverter<string, bool>
	{
        protected override bool Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ("Trim".Equals(parameter as string))
            {
                return string.IsNullOrWhiteSpace(value);
            }
            else
            {
                return string.IsNullOrEmpty(value);
            }
        }
	}
}
