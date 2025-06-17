using System;
namespace Core.io
{
    public static class ErrorLog
    {
        private static readonly string logFilePath = Path.Combine("output", "error.log");

        public static void LogError(string message, Exception? ex = null)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(logFilePath, append: true);
                writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}");
                if (ex != null)
                {
                    writer.WriteLine($"Exception: {ex.Message}");
                    writer.WriteLine($"StackTrace: {ex.StackTrace}");
                }
                writer.WriteLine(new string('-', 60));
            }
            catch
            {
                // Avoid crashing on logging failure
            }
        }
    }
}
