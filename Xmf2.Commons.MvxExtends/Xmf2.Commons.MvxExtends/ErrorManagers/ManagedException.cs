using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
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
