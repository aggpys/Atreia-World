using System.Drawing;
using System.Windows.Forms;
using Rift.Properties;

namespace Rift.Forms
{
    /// <summary>
    /// Represents the common application window or dialog
    /// that makes up a user interface.
    /// </summary>
    public class RiftDialog : Form
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Forms.RiftDialog"/> class.
        /// </summary>
        public RiftDialog()
        {
            base.Font = SystemFonts.MessageBoxFont;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.None;
            Icon = Resources.RiftIcon;
            MaximizeBox = false;
        }

        /// <returns>
        /// The default <see cref="System.Drawing.Size"/> of the control.
        /// </returns>
        protected override Size DefaultSize { get { return new Size(320, 240); } }
    }
}