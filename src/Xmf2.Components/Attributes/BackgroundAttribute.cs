using System;

namespace Xmf2.Components.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class BackgroundAttribute : Attribute
	{
		public static BackgroundAttribute Default => new BackgroundAttribute(0xFFFFFF);
		
		public int Color { get; }//TODO: il faut que le background attribute soit un uint avec l'alpha en 1er comme pour la génération de couleur.
		
		public BackgroundAttribute(int color)
		{
			Color = color;
		}
	}
}