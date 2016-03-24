using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;
using Android.Graphics;
using Android.Graphics.Drawables;

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