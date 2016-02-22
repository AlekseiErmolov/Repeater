using NLog;
using Repeater.Interfaces;

namespace Repeater.Classes
{
    public class NLogWrap : ILoggerWrap
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void WriteDebug(string message)
        {
            logger.Debug(message);
        }

        public void WriteError(string message)
        {
            logger.Error(message);
        }

        public void WriteInfo(string message)
        {
            logger.Info(message);
        }

        public void WriteTrace(string message)
        {
            logger.Trace(message);
        }
    }
}
