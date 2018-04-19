using NLog;

namespace Library.Logs
{
    /// <summary>
    ///
    /// </summary>
    public static class Log
    {
        public static Logger log;

        static Log()
        {
             log = LogManager.GetCurrentClassLogger();
        }
    }
}