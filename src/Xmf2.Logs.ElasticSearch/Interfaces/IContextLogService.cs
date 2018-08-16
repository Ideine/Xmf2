using System;

namespace Xmf2.Logs.ElasticSearch.Interfaces
{
	public interface IContextLogService
	{
		void Log(LogLevel level, Action<IObjectWriter> fillLogEntry);

		IContextLogService WithAppender(ILogAppender appender);
	}
}