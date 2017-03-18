using System.Collections.Generic;
using System.IO;

namespace Daves.WordamentSolver.UI.Helpers
{
    public static class FileHelper
    {
        public static string[] ReadAllLines(string filePath)
            => File.ReadAllLines(filePath);

        public static void WriteAllLines(string filePath, IEnumerable<string> lines)
            => File.WriteAllLines(filePath, lines);
    }
}
