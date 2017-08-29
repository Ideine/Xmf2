using System;
namespace Xmf2.Commons.Services
{
	public interface IUIDispatcher
	{
		void OnMainThread(Action action);
	}
}
