using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Droid.Converters
{
    public class ToColorDrawableConverter : MvxValueConverter<Color, ColorDrawable>
    {
        protected override ColorDrawable Convert(Color value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new ColorDrawable(value);
        }

        protected override Color ConvertBack(ColorDrawable value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Color;
        }
    }
}