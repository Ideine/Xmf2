using System;

namespace Xmf2.Commons.Logs
{
	public static class ILoggerExtensions
	{
		public static void Log(this ILogger logger, LogLevel level, string message)
		{
			logger.Log(level, null, message);
		}
		public static void Log(this ILogger logger, LogLevel level, Exception e)
		{
			logger.Log(level, e, null);
		}
		public static void LogCritical(this ILogger logger, string message)
		{
			logger.LogCritical(null, message);
		}
		public static void LogCritical(this ILogger logger, Exception exception)
		{
			logger.LogCritical(exception, null);
		}
		public static void LogError(this ILogger logger, string message)
		{
			logger.LogError(null, message);
		}
		public static void LogError(this ILogger logger, Exception exception)
		{
			logger.LogError(exception, null);
		}
		public static void LogInfo(this ILogger logger, string message)
		{
			logger.LogInfo(null, message);
		}
		public static void LogInfo(this ILogger logger, Exception exception)
		{
			logger.LogInfo(exception, null);
		}
		public static void LogWarning(this ILogger logger, string message)
		{
			logger.LogWarning(null, message);
		}
		public static void LogWarning(this ILogger logger, Exception exception)
		{
			logger.LogWarning(exception, null);
		}
	}
}
