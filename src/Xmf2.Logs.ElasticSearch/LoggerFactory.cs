using System.Net.Http;
using Xmf2.Logs.ElasticSearch.Appenders;
using Xmf2.Logs.ElasticSearch.Interfaces;
using Xmf2.Logs.ElasticSearch.Senders;

namespace Xmf2.Logs.ElasticSearch
{
	public static class LoggerFactory
	{
		public static IContextLogService Create(string index, string type, LogLevel minimumLogLevel, HttpClient client, string url, ILogBufferStorage storage)
		{
			return new ContextLogService(
				new LogService(
					new LogSender(client, url, storage),
					minimumLogLevel
				),
				index,
				type
			).WithAppender(new TimestampLogAppender());
		}
	}
}