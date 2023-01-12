using System;
using System.IO;
using System.Threading.Tasks;

namespace MovingShapes.Logging
{
    internal class Logger
    {
        private static readonly string _filePath = "../../../Logging/Logs/ExceptionLogs.log";
        public static async Task LogExceptionAsync(string? message, DateTime dateTime)
        {
            using (StreamWriter FileStreamWriter = new StreamWriter(_filePath, true))
            {
                await FileStreamWriter.WriteAsync(message + " " + dateTime.ToString() + "\n");
                FileStreamWriter.Flush();
            }
        }
    }
}
