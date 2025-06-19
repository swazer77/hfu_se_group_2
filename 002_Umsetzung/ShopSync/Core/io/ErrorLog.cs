namespace Core.io
{
    public static class ErrorLog
    {
        private static readonly string ProjectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        private static readonly string LogPath = Path.Combine(ProjectRoot, "output", "error.log");

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

        public static List<string> GetErrors()
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
    }
}
