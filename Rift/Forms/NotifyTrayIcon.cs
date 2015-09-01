using System.Drawing;
using System.Windows.Forms;
using Rift.Properties;

namespace Rift.Forms
{
    /// <summary>
    /// Represents the notify tray icon factory.
    /// </summary>
    public static class NotifyTrayIcon
    {
        /// <summary>
        /// Gets the resized notify tray icon for the current application.
        /// </summary>
        public static Icon Application
        {
            get { return CreateFrom(Resources.RiftIcon); }
        }
        
        /// <summary>
        /// Creates a new <see cref="System.Drawing.Icon"/> with a small size
        /// based on operating system and the current DPI value.
        /// </summary>
        /// <param name="icon">An <see cref="System.Drawing.Icon"/> source.</param>
        public static Icon CreateFrom(Icon icon)
        {
            return new Icon(icon, SystemInformation.SmallIconSize);
        }
    }
}