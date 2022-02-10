using System;
using System.Diagnostics;
using Xmf2.Commons.Logs;

namespace Xmf2.Commons.MvxExtends.Logs
{
    public class BaseLogger : ILogger
    {
        public void Log(LogLevel level, Exception e = null, string message = null)
        {
            Debug.WriteLine($"{message} : {e}");
        }

        public virtual void LogCritical(Exception e = null, string message = null)
        {
            Debug.WriteLine($"{message} : {e}");
        }

        public virtual void LogError(Exception e = null, string message = null)
        {
            Debug.WriteLine($"{message} : {e}");
        }

        public virtual void LogWarning(Exception e = null, string message = null)
        {
            Debug.WriteLine($"{message} : {e}");
        }

        public virtual void LogInfo(Exception e = null, string message = null)
        {
            Debug.WriteLine($"{message} : {e}");
        }
    }
}