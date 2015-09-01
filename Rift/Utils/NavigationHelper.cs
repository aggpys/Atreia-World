using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Rift.Utils
{
    /// <summary>
    /// Represents an URI navigation helper.
    /// </summary>
    public static class NavigationHelper
    {
        /// <summary>
        /// Navigates to the specified URI.
        /// </summary>
        /// <param name="uri">An URI to navigate to.</param>
        public static void NavigateTo(string uri)
        {
            if (string.IsNullOrEmpty(uri))
                return;

            try
            {
                var process = Process.Start(uri);

                if (process != null)
                    process.Dispose();
            }
            catch (FileNotFoundException) { }
            catch (ObjectDisposedException) { }
            catch (Win32Exception) { }
        }
    }
}