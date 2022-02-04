using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	/// <summary>
	/// Permet l'appel aux méthodes a la méthode <see cref="IFormattable.ToString(string, IFormatProvider)"/>.
	/// </summary>
	public class FormattableValueConverter :  MvxValueConverter<IFormattable, string>
	{
		public const string ConverterName = "Formattable";

		protected override string Convert(IFormattable value, Type targetType, object parameter, CultureInfo culture)
		{
			var format = parameter as string ?? String.Empty;
			return value?.ToString(format, formatProvider: null);
		}
	}
}
