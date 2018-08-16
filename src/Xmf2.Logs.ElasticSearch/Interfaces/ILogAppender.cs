namespace Xmf2.Logs.ElasticSearch.Interfaces
{
	public interface ILogAppender
	{
		void Append(IObjectWriter logEntry);
	}
}