namespace Rift.Utils
{
    /// <summary>
    /// Helper class that provides methods to resolve icon image URI.
    /// </summary>
    public static class IconPathResolver
    {
        private const string IconPathFormat = @"http://aiondatabase.net/items/{0}.png";

        /// <summary>
        /// Returns the full string URI for the specified icon.
        /// </summary>
        /// <param name="iconName">An icon name to resolve URI.</param>
        public static string ExpandUri(string iconName)
        {
            return string.Format(IconPathFormat, iconName);
        }
    }
}