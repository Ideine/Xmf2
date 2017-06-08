using System;
namespace Xmf2.Commons.Logs
{
	public class DebugLogger : ILogger
	{
		public void Log(LogLevel level, Exception e = null, string message = null)
		{
			InternalLog(level, e, message);
		}

		public void LogCritical(Exception e = null, string message = null)
		{
			Log(LogLevel.Critical, e, message);
		}

		public void LogError(Exception e = null, string message = null)
		{
			Log(LogLevel.Error, e, message);
		}

		public void LogInfo(Exception e = null, string message = null)
		{
			Log(LogLevel.Info, e, message);
		}

		public void LogWarning(Exception e = null, string message = null)
		{
			Log(LogLevel.Warning, e, message);
		}

		private void InternalLog(LogLevel level, Exception e, string message)
		{
			System.Diagnostics.Debug.WriteLine($"LOG/{level}: {message ?? ""} {e.ToString() ?? ""}");
		}
	}
}
