using System;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public class ManagedException : Exception
	{
		public bool IsLogged { get; set; }
		public bool IsUserShown { get; set; }
		
		public ManagedException() : base() { }

		public ManagedException(string message) : base(message) { }

		public ManagedException(string message, Exception innerException) : base(message, innerException) { }
	}
}