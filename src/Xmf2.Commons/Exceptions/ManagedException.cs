using System;

namespace Xmf2.Commons.Exceptions
{
    //TODO: voir pour supprimer si non utilisée
	public class ManagedException : Exception
    {
        public ManagedException()
            : base()
        { }

        public ManagedException(string message)
            : base(message)
        { }

        public ManagedException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public bool IsLogged { get; set; }
        public bool IsUserShown { get; set; }
    }
}
