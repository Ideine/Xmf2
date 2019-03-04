using System;

namespace Xmf2.Components.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class BackgroundAttribute : Attribute
	{
		public static BackgroundAttribute Default => new BackgroundAttribute(0xFFFFFF);
		
		public int Color { get; }
		
		public BackgroundAttribute(int color)
		{
			Color = color;
		}
	}
}