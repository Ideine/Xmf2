using System;

namespace Xmf2.Components.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ViewStateTypeAttribute : Attribute
	{
		public Type ViewStateType { get; }
		
		public ViewStateTypeAttribute(Type viewStateType)
		{
			ViewStateType = viewStateType;
		}
	}
}