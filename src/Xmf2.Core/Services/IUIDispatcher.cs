using System;
namespace Xmf2.Core.Services
{
	public interface IUIDispatcher
	{
		void OnMainThread(Action action);
	}
}
