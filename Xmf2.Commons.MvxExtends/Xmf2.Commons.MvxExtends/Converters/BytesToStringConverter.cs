using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Xmf2.Commons.MvxExtends.Converters
{
	public class BytesToStringConverter : MvxValueConverter<long, string>
	{
		protected override string Convert(long value, Type targetType, object parameter, CultureInfo culture)
		{
			string[] suf =
			{
				"B", "KB", "MB", "GB", "TB", "PB", "EB"
			}; //Longs run out around EB
			if (value == 0)
			{
				return "0" + suf[0];
			}

			long bytes = Math.Abs(value);
			int place = System.Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
			double num = Math.Round(bytes / Math.Pow(1024, place), 1);
			return (Math.Sign(value) * num) + suf[place];
		}
	}
}