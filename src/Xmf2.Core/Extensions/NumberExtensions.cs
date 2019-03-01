namespace Xmf2.Core.Extensions
{
	public static class NumberExtensions
	{
		public static float Clamp(this float value, float min, float max)
		{
			return value < min ? min : value > max ? max : value;
		}

		public static double Clamp(this double value, double min, double max)
		{
			return value < min ? min : value > max ? max : value;
		}

		public static int Clamp(this int value, int min, int max)
		{
			return value < min ? min : value > max ? max : value;
		}

		public static decimal Clamp(this decimal value, decimal min, decimal max)
		{
			return value < min ? min : value > max ? max : value;
		}
	}
}