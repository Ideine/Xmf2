using System;

public static class ObjectExtensions
{
	public static T CastTo<T>(this Object value)
	{
		return (T)value;
	}
}
