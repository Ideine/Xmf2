namespace Xmf2.Logs.ElasticSearch.Interfaces
{
	internal interface ILogSender
	{
		void Enqueue(ILogEntry entry);
	}
}