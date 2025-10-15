using System;

namespace Xmf2.Core.Helpers
{
	public static class AngleHelper
	{
		public static double DegreeToRadian(this double angle) => Math.PI * angle / 180.0;
		public static double DegreeToRadian(this int angle) => Math.PI * angle / 180.0;

		public static double RadianToDegree(this double angle) => angle * (180.0 / Math.PI);
		public static double RadianToDegree(this int angle) => angle * (180.0 / Math.PI);
	}
}
