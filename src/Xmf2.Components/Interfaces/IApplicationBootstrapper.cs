namespace Xmf2.Components.Interfaces
{
	public interface IApplicationBootstrapper
	{
		IServiceLocator Services { get; }

		void Initialize();

		void ShowFirstView();
	}
}