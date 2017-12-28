using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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