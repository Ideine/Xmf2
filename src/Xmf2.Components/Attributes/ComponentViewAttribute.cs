using System;

namespace Xmf2.Components.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ComponentViewAttribute : Attribute
	{
		public static ComponentViewAttribute Default => new ComponentViewAttribute(ComponentSize.MatchParent, ComponentSize.AutoSize)
		{
			MarginLeft = 16,
			MarginRight = 16,
			MarginTop = 8,
			MarginBottom = 8,
		};

		public ComponentSize WidthSpec { get; }
		
		public ComponentSize HeightSpec { get; }
		
		public int MarginLeft { get; set; } = int.MinValue;
		
		public int MarginTop { get; set; } = int.MinValue;
		
		public int MarginRight { get; set; } = int.MinValue;
		
		public int MarginBottom { get; set; } = int.MinValue;
		
		public int Width { get; set; }
		
		public int Height { get; set; }
		
		public ComponentViewAttribute(ComponentSize widthSpec, ComponentSize heightSpec)
		{
			WidthSpec = widthSpec;
			HeightSpec = heightSpec;
		}
	}
}