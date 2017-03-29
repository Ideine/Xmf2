using System;
using UIKit;

namespace UIKit
{
	public static class UIColorExtension
	{
		public static UIColor ColorFromHex(this int hexValue, float alpha = 1)
		{
			return UIColor.FromRGB(
				((hexValue & 0xFF0000) >> 16) / 255.0f,
				((hexValue & 0xFF00) >> 8) / 255.0f,
				(hexValue & 0xFF) / 255.0f
			).ColorWithAlpha(alpha);
		}
	}
}
