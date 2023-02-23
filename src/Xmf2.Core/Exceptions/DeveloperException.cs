using System;

namespace Xmf2.Core.Exceptions
{
	public class DeveloperException : Exception
	{
		public bool HideFromUser { get; set; } = true;

		public DeveloperException() { }
		public DeveloperException(string message) : base(message) { }
		public DeveloperException(string message, Exception inner) : base(message, inner) { }
	}
}