using System.Drawing;
using System.Windows.Forms;
using Rift.Forms;
using Rift.Properties;

namespace Rift
{
    /// <summary>
    /// Specifies a message type.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Information (neutral).
        /// </summary>
        Info,
        /// <summary>
        /// Warning message.
        /// </summary>
        Warning,
        /// <summary>
        /// Error message, exception.
        /// </summary>
        Error
    }

    /// <summary>
    /// Represents an application error dialog box.
    /// This class cannot be inherited.
    /// </summary>
    public sealed partial class MessageDialog : RiftForm
    {
        private static readonly Color[] messageColors;

        static MessageDialog()
        {
            messageColors = new[]
            {
                Color.DodgerBlue,
                Color.DeepSkyBlue,
                Color.DarkOrange,
                Color.Orange,
                Color.Firebrick,
                Color.IndianRed
            };
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.MessageDialog"/> class
        /// with a specified error message to shop and optional button to
        /// restart the game client.
        /// </summary>
        /// <param name="type">A message type.</param>
        /// <param name="message">A text error message to show.</param>
        public MessageDialog(MessageType type, string message)
        {
            InitializeComponent();

            var typeTitle = Resources.MessageDialogInfo;

            switch (type)
            {
                case MessageType.Warning:
                    typeTitle = Resources.MessageDialogWarning;
                    BorderColorActive = messageColors[2];
                    labelTitle.Text = Resources.MessageDialogTitleWarning;
                    buttonOk.FlatAppearance.BorderColor = messageColors[2];
                    buttonOk.FlatAppearance.MouseDownBackColor = messageColors[2];
                    buttonOk.FlatAppearance.MouseOverBackColor = messageColors[3];
                    break;
                case MessageType.Error:
                    typeTitle = Resources.MessageDialogError;
                    BorderColorActive = messageColors[4];
                    labelTitle.Text = Resources.MessageDialogTitleError;
                    buttonOk.FlatAppearance.BorderColor = messageColors[4];
                    buttonOk.FlatAppearance.MouseDownBackColor = messageColors[4];
                    buttonOk.FlatAppearance.MouseOverBackColor = messageColors[5];
                    break;
                default:
                    BorderColorActive = messageColors[0];
                    buttonOk.FlatAppearance.BorderColor = messageColors[0];
                    buttonOk.FlatAppearance.MouseDownBackColor = messageColors[0];
                    buttonOk.FlatAppearance.MouseOverBackColor = messageColors[1];
                    break;
            }

            Text = string.Format("{0} - {1}", Text, typeTitle);
            textBoxError.Text = message;

            var textSize = TextRenderer.MeasureText(message, textBoxError.Font, textBoxError.ClientSize, TextFormatFlags.WordBreak);
            var delta = textSize.Height - textBoxError.Height;

            textBoxError.Height += delta;
            Height += delta;
        }
    }
}
