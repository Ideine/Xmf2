using System.Net.Http;
using Xmf2.Logs.ElasticSearch.Appenders;
using Xmf2.Logs.ElasticSearch.Interfaces;
using Xmf2.Logs.ElasticSearch.Senders;

namespace Xmf2.Logs.ElasticSearch
{
	public static class LoggerFactory
	{
		public static IContextLogService Create(string index, string type, LogLevel minimalLogLevel, HttpClient client, string url, ILogBufferStorage storage)
		{
			return new ContextLogService(
				new LogService(
					new LogSender(client, url, storage),
					minimalLogLevel
				),
				index,
				type
			).WithAppender(new TimestampLogAppender());
		}
	}
}