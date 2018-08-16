using Xmf2.Logs.ElasticSearch.Interfaces;

namespace Xmf2.Logs.ElasticSearch.Internals
{
	internal class LogEntry : ILogEntry
	{
		public LogEntry(string index, string type, string content)
		{
			Index = index;
			Type = type;
			Content = content;
		}

		public string Index { get; }

		public string Type { get; }

		public string Content { get; }
	}
}