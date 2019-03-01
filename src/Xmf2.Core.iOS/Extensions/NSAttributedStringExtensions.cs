using System;
using System.Linq;
using UIKit;

namespace Foundation
{
	public static class NSAttributedStringExtensions
	{
		public static NSMutableAttributedString WithAttribute(this NSMutableAttributedString text, UIStringAttributes attribute, string onTextToSeek)
		{
			if (text.TryGetRangeOf(onTextToSeek, StringComparison.InvariantCultureIgnoreCase, out var range))
			{
				text.SetAttributes(attribute, range);
			}
			return text;
		}

		public static NSAttributedString Join(NSAttributedString separator, params NSAttributedString[] values)
		{
			var result = new NSMutableAttributedString();
			if (values.Any())
			{
				result.Append(values.First());
			}
			foreach (var value in values.Skip(1))
			{
				result.Append(separator);
				result.Append(value);
			}
			return result;
		}

		public static bool TryGetRangeOf(this NSAttributedString str, string value, StringComparison strComparison, out NSRange range)
		{
			int startIndex = str.Value.IndexOf(value, strComparison);
			if (startIndex <= 0)
			{
				range = default(NSRange);
				return false;
			}
			else
			{
				range = new NSRange(startIndex, value.Length);
				return true;
			}
		}
	}
}