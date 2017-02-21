using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmf2.Commons.Extensions
{
	public static class EnumExtensions
	{
		public static NotSupportedException GetNotSupportedException(this Enum enumValue)
		{
			return new NotSupportedException(string.Format("This {0} value is not supported yet. Value: {1}"
														 , enumValue.GetType().FullName
														 , enumValue));
		}
	}
}
