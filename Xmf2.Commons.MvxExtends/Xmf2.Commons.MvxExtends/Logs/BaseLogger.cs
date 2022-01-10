using System;
using System.Text;
using Xmf2.Commons.Logs;

namespace Xmf2.Commons.MvxExtends.Logs
{
    public class BaseLogger : ILogger
	{
		public virtual void Log(LogLevel level, Exception e = null, string message = null)
		{
			if (e == null && message == null)
				return;

			message = (message != null)
					? message.Replace("{", "{{").Replace("}", "}}")
					: String.Empty;
			System.Diagnostics.Debug.WriteLine(level, this.FormatException(e, message));
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
