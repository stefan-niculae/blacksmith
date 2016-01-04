using System;
using System.IO;
using System.Web;

namespace Blacksmith.Utilities
{
    public static class DebugLogger
    {
       static readonly string LOG_PATH = HttpContext.Current.Server.MapPath("~/Utilities/debuglog.txt");

        public static void Log(string message)
        {
            using (var writer = new StreamWriter(LOG_PATH, append: true))
                writer.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}