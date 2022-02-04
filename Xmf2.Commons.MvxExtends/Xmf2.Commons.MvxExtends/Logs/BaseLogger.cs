using System;
using System.Text;
using MvvmCross;
using Xmf2.Commons.Logs;

namespace Xmf2.Commons.MvxExtends.Logs
{
	public class BaseLogger : ILogger
	{
		public virtual void Log(LogLevel level, Exception e = null, string message = null)
		{
			if (e == null && message == null)
				return;

			//MvxTraceLevel traceLevel; todo
			switch (level)
			{
				case LogLevel.Critical:
				case LogLevel.Error:
					//traceLevel = MvxTraceLevel.Error;
					break;
				case LogLevel.Warning:
					//traceLevel = MvxTraceLevel.Warning;
					break;
				case LogLevel.Info:
				default:
					//traceLevel = MvxTraceLevel.Diagnostic;
					break;
			}

			message = (message != null)
					? message.Replace("{", "{{").Replace("}", "}}")
					: String.Empty;
			//Mvx.Trace(traceLevel, this.FormatException(e, message));
		}

		public virtual void LogCritical(Exception e = null, string message = null)
		{
			Log(LogLevel.Critical, e, message);
		}

		public virtual void LogError(Exception e = null, string message = null)
		{
			Log(LogLevel.Error, e, message);
		}

		public virtual void LogWarning(Exception e = null, string message = null)
		{
			Log(LogLevel.Warning, e, message);
		}

		public virtual void LogInfo(Exception e = null, string message = null)
		{
			Log(LogLevel.Info, e, message);
		}

		protected virtual string FormatException(Exception e = null, string message = null)
		{
			StringBuilder builder = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(message))
			{
				builder.AppendLine(message);
			}
			if (e != null)
			{
				builder.AppendLine(e.ToString());
			}
			return builder.ToString();
		}
	}
}
