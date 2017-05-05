using System;
using MvvmCross.Platform.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class ToUpperConverter : MvxValueConverter<string, string>
	{
		private static ToUpperConverter _instance;
		public static ToUpperConverter Instance => _instance ?? (_instance = new ToUpperConverter());

		protected override string Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value?.ToUpper();
		}
	}
}
