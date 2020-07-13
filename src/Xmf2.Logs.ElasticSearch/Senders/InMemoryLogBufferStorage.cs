namespace Xmf2.Logs.ElasticSearch.Senders
{
	public class InMemoryLogBufferStorage : ILogBufferStorage
	{
		private string _content;

		public string Load() => _content;

		public void Save(string content) => _content = content;
	}
}