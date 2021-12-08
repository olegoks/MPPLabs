using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public interface ILogger
    {
        string GetCurrentDirectory();
        string GetCurrentFile();
        string GetDirectoryByLoggerName(string loggerName);

    }

    //class DefaultLogger : ILogger
    //{
    //    public string GetCurrentFile()
    //    {
    //        return "C:\\log.txt";
    //    }

    //    string ILogger.GetCurrentDirectory()
    //    {
    //        return "C:\\";
    //    }
    //}
}
