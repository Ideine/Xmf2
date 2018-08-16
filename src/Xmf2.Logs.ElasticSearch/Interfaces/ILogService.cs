using System;

namespace Xmf2.Logs.ElasticSearch.Interfaces
{
	internal interface ILogService
	{
		void Log(LogLevel level, string index, string type, Action<IObjectWriter> fillLogEntry);

		ILogService WithAppender(ILogAppender appender);

		IContextLogService CreateContext(string index, string type);
	}
}