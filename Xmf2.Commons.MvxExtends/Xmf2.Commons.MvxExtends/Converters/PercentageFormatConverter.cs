using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.CrossCore.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
    public class PercentageFormatConverter : MvxValueConverter<int, string>
    {
        protected override string Convert(int value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value < 0)
                return string.Empty;
            else
                return string.Format("{0}%", value);
        }
    }
}