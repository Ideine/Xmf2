using System.Threading.Tasks;

namespace Xmf2.Commons.Rx.ViewModels
{
	public interface IViewModelLifecycleManager
	{
		Task WaitForInitialization();

		void Initialize();

		void Start();

		void Resume();

		void Pause();

		void Stop();
	}
}
