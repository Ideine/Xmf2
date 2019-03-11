namespace Xmf2.Authentications.OAuth2.Authentication
{
	public interface IUserStorageService
	{
		void Store(AuthenticationDetailStorageModel detail);

		bool Has();

		AuthenticationDetailStorageModel Get();

		void Delete();
	}
}