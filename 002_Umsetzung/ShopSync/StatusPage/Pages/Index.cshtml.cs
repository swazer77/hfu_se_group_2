using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Core.io;

public class IndexModel : PageModel
{
    public List<string> Logs { get; private set; }

    public void OnGet()
    {
        Logs = GetLogs();
    }

    private static readonly string ProjectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..",".."));
    private static readonly string LogPath = Path.Combine(ProjectRoot, "Core", "output", "logger.log");

    private List<string> GetLogs()
    {
        if (System.IO.File.Exists(LogPath))
        {
            return new List<string>(System.IO.File.ReadAllLines(LogPath));
        }
        else
        {
            return new List<string> { "Log file not found: " + LogPath };
        }
    }
}