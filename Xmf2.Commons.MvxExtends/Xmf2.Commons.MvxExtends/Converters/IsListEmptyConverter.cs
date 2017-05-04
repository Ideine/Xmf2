using MvvmCross.Platform.Converters;
using System;
using System.Collections;

namespace Xmf2.Commons.MvxExtends.Converters
{
    public class IsListEmptyConverter : MvxValueConverter<ICollection, bool>
    {

        public const string ConverterName = "IsListEmpty";

        public const string ReverseParameter = "Not";

        protected override bool Convert(ICollection value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool returnValue;
            if (value == null)
            {
                returnValue = true;
            }
            else
            {
                returnValue = value.Count == 0;
            }
            if (ReverseParameter.Equals(parameter as string))
            {
                returnValue = !returnValue;
            }
            return returnValue;
        }
    }
}
