namespace Xmf2.Logs.ElasticSearch.Senders
{
	public interface ILogBufferStorage
	{
		string Load();

		void Save(string content);
	}
}