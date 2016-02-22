namespace Repeater.Interfaces
{
    public interface ILoggerWrap
    {
        /// <summary>
        /// Write error to log
        /// </summary>
        /// <param name="message"></param>
        void WriteError(string message);
        /// <summary>
        /// Write info to log
        /// </summary>
        /// <param name="message"></param>
        void WriteInfo(string message);
        /// <summary>
        /// Write trace to log
        /// </summary>
        /// <param name="message"></param>
        void WriteTrace(string message);
        /// <summary>
        /// Write debug to log
        /// </summary>
        /// <param name="message"></param>
        void WriteDebug(string message);
    }
}
