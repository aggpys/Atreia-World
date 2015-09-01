using System;
using System.IO;

namespace Rift.Utils
{
    /// <summary>
    /// Specifies the extension methods for the string path.
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        /// Normalizes the specified string path.
        /// </summary>
        /// <param name="path">A string path to normalize.</param>
        public static string Normalize(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            try
            {
                var temp = Path.GetFullPath(new Uri(path).LocalPath)
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .ToUpperInvariant();

                return temp;
            }
            catch (UriFormatException)
            {
                return path;
            }
        }
    }
}