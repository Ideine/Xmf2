using System;
using MvvmCross.Platform.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{

    public class LambdaConverter<TSource, TDest> : MvxValueConverter<TSource, TDest>
    {
        private readonly Func<TSource, object, TDest> _convert;
        private readonly Func<TDest, object, TSource> _convertBack;

        public LambdaConverter(Func<TSource, object, TDest> convert, Func<TDest, object, TSource> convertBack = null)
        {
            _convert = convert;
            _convertBack = convertBack;
        }

        public LambdaConverter(Func<TSource, TDest> convert, Func<TDest, TSource> convertBack = null)
        {
            if (convert != null)
            {
                _convert = (source, param) => convert(source);
            }

            if (convertBack != null)
            {
                _convertBack = (dest, param) => convertBack(dest);
            }
        }

        protected override TDest Convert(TSource value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (_convert != null)
            {
                return _convert(value, parameter);
            }

            return base.Convert(value, targetType, parameter, culture);
        }

        protected override TSource ConvertBack(TDest value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (_convertBack != null)
            {
                return _convertBack(value, parameter);
            }

            return base.ConvertBack(value, targetType, parameter, culture);
        }
    }
}
