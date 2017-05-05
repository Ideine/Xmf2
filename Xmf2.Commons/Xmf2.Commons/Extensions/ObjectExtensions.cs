using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ObjectExtensions
{
	public static T CastTo<T>(this Object value)
	{
		return (T)value;
	}
}
