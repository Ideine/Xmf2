using System;
using System.Threading.Tasks;

namespace Xmf2.Commons.ErrorManagers
{
	public interface IErrorManager
    {
        void TreatError(Exception e, bool promptErrorMessageToUser);
        Task TreatErrorAsync(Exception e, bool promptErrorMessageToUser);
    }
}
