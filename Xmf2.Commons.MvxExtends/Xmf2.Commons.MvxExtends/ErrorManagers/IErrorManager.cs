using System;

namespace Xmf2.Commons.MvxExtends.ErrorManagers
{
	public interface IErrorManager
	{
		void TreatError(Exception e);
	}
}