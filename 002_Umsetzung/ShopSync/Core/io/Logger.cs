namespace Core.io
{
    public static class Logger
    {
        private static readonly string ProjectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        private static readonly string LogPath = Path.Combine(ProjectRoot, "output", "logger.log");

        public static void LogError(string message, Exception? ex = null)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
                string newEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}{Environment.NewLine}";

                if (ex != null)
                {
                    newEntry += $"Exception: {ex.Message}{Environment.NewLine}";
                }

                string existingContent = File.Exists(LogPath) ? File.ReadAllText(LogPath) : string.Empty;
                File.WriteAllText(LogPath, newEntry + existingContent);
            }
            catch
            {
                //
            }
        }

        public static void LogInfo(string message)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
                string newEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] INFO: {message}{Environment.NewLine}";
                string existingContent = File.Exists(LogPath) ? File.ReadAllText(LogPath) : string.Empty;
                File.WriteAllText(LogPath, newEntry + existingContent);
            }
            catch
            {
                //
            }
        }

        public static List<string> GetLogs()
        {
            List<string> errors = new List<string>();
            try
            {
                if (File.Exists(LogPath)) 
                    errors.AddRange(File.ReadAllLines(LogPath));
            }
            catch
            {
                //
            }
            return errors;
        }

        public static void LogEnd()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(LogPath)!);
            string newEntry = new string('-', 60) + Environment.NewLine;
            string existingContent = File.Exists(LogPath) ? File.ReadAllText(LogPath) : string.Empty;
            File.WriteAllText(LogPath, newEntry + existingContent);
        }
    }
}
