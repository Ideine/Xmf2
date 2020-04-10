namespace Xmf2.Core.iOS.Extensions
{
	public static class UIColorExtension
	{
		public static string HtmlColorFromHex(this uint color)
		{
			return PutAlphaCanalToRightByte(color).ToString("X8");

			uint PutAlphaCanalToRightByte(uint pColor)
				=> (pColor << 8)   // r+g+b moved to left so 4 bytes looks like (r, g, b, <empty>)
				 | (pColor >> 24); // alpha canal moved to right so 4 bytes looks like (<empty>, <empty>, <empty>, alpha)
		}
	}
}
