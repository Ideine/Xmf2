using UIKit;

namespace Xmf2.Core.iOS.Extensions
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
			return UIColor.FromRGBA(
				((hexValue & 0xFF0000) >> 16) / 255.0f,
				((hexValue & 0xFF00) >> 8) / 255.0f,
				((hexValue & 0xFF)) / 255.0f,
				((hexValue & 0xFF000000) >> 24) / 255.0f
			);
		}

		public static string HtmlColorFromHex(this uint color)
		{
			return PutAlphaCanalToRightByte(color).ToString("X8");

			uint PutAlphaCanalToRightByte(uint pColor)
				=> (pColor << 8)   // r+g+b moved to left so 4 bytes looks like (r, g, b, <empty>)
				 | (pColor >> 24); // alpha canal moved to right so 4 bytes looks like (<empty>, <empty>, <empty>, alpha)
		}
	}
}
