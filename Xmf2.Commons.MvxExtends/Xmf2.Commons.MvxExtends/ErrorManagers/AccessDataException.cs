using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
    public class AccessDataException : ManagedException
    {
        public enum ErrorType
        {
            Unknown = 0,
            NoInternetConnexion = 1,
            UnAuthorized = 2,
            Timeout = 3
        }

        public ErrorType Type { get; private set; }

        public AccessDataException(ErrorType type)
            : base("Data access error")
        {
            this.Type = type;
        }

        public AccessDataException(ErrorType type, Exception innerException)
            : base("Unknown data access error", innerException)
        {
            this.Type = type;
        }
    }
}
