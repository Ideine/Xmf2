using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmf2.Commons.ErrorManagers
{

	public class InvalidAppVersionException : ManagedException
	{
		public InvalidAppVersionException()
			: base()
		{ }

		public InvalidAppVersionException(string message)
			: base(message)
		{ }

		public InvalidAppVersionException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
