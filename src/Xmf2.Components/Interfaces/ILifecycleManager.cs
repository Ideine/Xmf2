using System.Threading.Tasks;

namespace Xmf2.Components.Interfaces
{
	public interface ILifecycleManager
	{
		Task WaitForInitialization();

		void Initialize();

		void Start();

		void Resume();

		void Pause();

		void Stop();
	}
}