using System;
using System.IO;
using System.Threading.Tasks;

namespace MovingShapes.Logging
{
    internal class Logger : IDisposable
    {
        private static readonly string _filePath = "../../../Logging/Logs/ExceptionLogs.log";
        private static StreamWriter _fileStreamWriter;
        private bool _disposed;
        static Logger()
        {
            _fileStreamWriter = new StreamWriter(_filePath, true);
        }
        public static async Task LogExceptionAsync(string? message, DateTime dateTime)
        {
            await _fileStreamWriter.WriteAsync(message + " " + dateTime.ToString() + "\n");
            _fileStreamWriter.Flush();
        }

        protected void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _fileStreamWriter.Dispose();
            }
            
            _disposed = true;
        }

        public void Dispose()
        {
            if (_fileStreamWriter is null)
            {
                Dispose(false);
            }
            else
                Dispose(true);            
        }
    }
}
