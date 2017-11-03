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

		public static UIColor ColorFromHex(this uint hexValue)
		{
			//TODO : value with alpa 0 don't work
			if (hexValue > 0xFFFFFF)
			{
				return UIColor.FromRGBA(
					((hexValue & 0xFF0000) >> 16) / 255.0f,
					((hexValue & 0xFF00) >> 8) / 255.0f,
					(hexValue & 0xFF) / 255.0f,
					((hexValue & 0xFF000000) >> 24) / 255.0f
				);
			}
			else
			{
				return ((int)hexValue).ColorFromHex();
			}
		}
	}
}
