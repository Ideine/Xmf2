using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xmf2.Commons.MvxExtends.Logs
{
    public enum LogLevel
    {
        Critical = 0,
        Error = 1,
        Warning = 2,
        Info = 3
    }
    public interface ILogger
    {
        void Log(LogLevel level, Exception e = null, string message = null);
        void LogCritical(Exception e = null, string message = null);
        void LogError(Exception e = null, string message = null);
        void LogWarning(Exception e = null, string message = null);
        void LogInfo(Exception e = null, string message = null);
    }
}
