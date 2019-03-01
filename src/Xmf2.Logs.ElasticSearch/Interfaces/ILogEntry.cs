namespace Xmf2.Logs.ElasticSearch.Interfaces
{
	internal interface ILogEntry
	{
		string Index { get; }
		string Type { get; }
		string Content { get; }
	}
}