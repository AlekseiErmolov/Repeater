using System;
using NLog;
using Repeater.Interfaces;

namespace Repeater.Classes.Logs
{
    public class NLogWrap : ILoggerWrap
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public void WriteDebug(string message)
        {
            Logger.Debug(message);
        }

        public void WriteError(Exception exception, string message)
        {
            Logger.Error(exception, message);
        }

        public void WriteInfo(string message)
        {
            Logger.Info(message);
        }

        public void WriteTrace(string message)
        {
            Logger.Trace(message);
        }
    }
}
