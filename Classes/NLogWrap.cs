using NLog;
using Repeater.Interfaces;

namespace Repeater.Classes
{
    public class NLogWrap : ILoggerWrap
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public NLogWrap()
        {

        }

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
