using System;
namespace System
{
	public static class XMF2TupleExtensions
	{
		public static (T1, T2) FuncToTuple<T1, T2>(T1 arg1, T2 arg2)
		{
			return (arg1, arg2);
		}

		public static (T1, T2, T3) FuncToTuple<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
		{
			return (arg1, arg2, arg3);
		}
	}
}
