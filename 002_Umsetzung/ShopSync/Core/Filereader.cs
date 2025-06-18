using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MyCoreLibrary          // Namespace deines Core‑Projekts
{
    public class FileReader
    {
        /// <summary>
        /// Liest eine Textdatei ein und gibt jede Zeile als List&lt;string&gt; zurück.
        /// Gesucht wird in
        ///   1. AppContext.BaseDirectory   (Ausführungsordner)
        ///   2. Verzeichnis der Assembly   (falls abweichend)
        ///   3. Unterordner "wwwroot"      (typisch bei ASP.NET‑Projekten)
        /// </summary>
        /// <param name="fileName">Dateiname oder relativer Pfad, z. B. "data.txt".</param>
        public List<string> ReadFileAsStrings(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName darf nicht leer sein.", nameof(fileName));

            try
            {
                // Mögliche Orte zusammenstellen
                var candidates = new List<string>
                {
                    Path.Combine(AppContext.BaseDirectory, fileName),
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty,
                        fileName),
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName)
                };

                // Erstes existierendes Vorkommen zurückgeben
                foreach (var path in candidates.Distinct())
                {
                    if (File.Exists(path))
                    {
                        Console.WriteLine($"Lese Datei von: {path}");
                        return File.ReadAllLines(path).ToList();
                    }
                }

                throw new FileNotFoundException(
                    $"Datei \"{fileName}\" wurde nicht gefunden. Gesuchte Pfade:\n{string.Join(Environment.NewLine, candidates)}");
            }
            catch (Exception ex)
            {
                // Hier könntest du in ein zentrales Error‑Log schreiben
                Console.WriteLine($"Fehler beim Lesen der Datei \"{fileName}\": {ex.Message}");
                return new List<string> { $"Fehler: {ex.Message}" };
            }
        }
    }
}