using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rift.Forms
{
    /// <summary>
    /// Represents the command link button Windows control.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class CommandButton : RiftButtonBase
    {
        private const float DefaultTitleFontSize = 9.0f;

        private const TextFormatFlags TitleFormat =
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.Left |
            TextFormatFlags.WordEllipsis;

        private const TextFormatFlags TextFormat =
            TextFormatFlags.Top |
            TextFormatFlags.Left |
            TextFormatFlags.WordBreak |
            TextFormatFlags.EndEllipsis;

        private float titleFontSize;
        private Font titleFont;
        private Image image;
        private Color titleForeColor;
        private Color borderColor;
        private string title;

        /// <summary>
        /// Gets the background color for this control.
        /// </summary>
        [Browsable(false)]
        public override Color BackColor
        {
            get { return Color.Transparent; }
        }

        /// <summary>
        /// Gets the background image.
        /// </summary>
        [Browsable(false)]
        public override Image BackgroundImage { get { return null; } }

        /// <summary>
        /// Gets the background image layout type.
        /// </summary>
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout { get; set; }

        /// <summary>
        /// Gets or sets the command link button border color.
        /// </summary>
        [Category("Appearance")]
        [Description("Control border color to set.")]
        [DefaultValue(typeof(Color), "ActiveBorder")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                var changed = borderColor != value;
                borderColor = value;

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the command link button image.
        /// </summary>
        [Category("Appearance")]
        [Description("Command link button image to set.")]
        [DefaultValue(typeof (Image), "(none)")]
        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the title font size.
        /// </summary>
        [Category("Appearance")]
        [Description("Title font size to set.")]
        [DefaultValue(DefaultTitleFontSize)]
        public float TitleFontSize
        {
            get { return titleFontSize; }
            set
            {
                titleFontSize = value;
                InitializeTitleFont(false);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the command link button title text.
        /// </summary>
        [Category("Appearance")]
        [Description("Command link button title text to set.")]
        [DefaultValue(default (string))]
        public string Title
        {
            get { return title; }
            set
            {
                var changed = !string.Equals(title, value, StringComparison.Ordinal);
                title = value;

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the control text.
        /// </summary>
        [Category("Appearance")]
        [Description("Command link button text to set.")]
        [DefaultValue(default(string))]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                var changed = !string.Equals(base.Text, value, StringComparison.Ordinal);
                base.Text = value;

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the title text color.
        /// </summary>
        [Category("Appearance")]
        [Description("Title text color to set.")]
        [DefaultValue(typeof(Color), "ControlText")]
        public Color TitleForeColor
        {
            get { return titleForeColor; }
            set
            {
                var changed = titleForeColor != value;
                titleForeColor = value;

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Forms.CommandButton"/> class.
        /// </summary>
        public CommandButton()
        {
            base.BackColor = Color.Transparent;
            borderColor = SystemColors.ActiveBorder;
            title = base.Text;
            titleForeColor = ForeColor;
            titleFontSize = DefaultTitleFontSize;
            titleFont = new Font(Font.FontFamily, titleFontSize, Font.Style);
            image = null;
        }

        private void InitializeTitleFont(bool invalidate = true)
        {
            if (titleFont != null)
                titleFont.Dispose();

            titleFont = new Font(Font.FontFamily, titleFontSize, Font.Style);
            
            if (invalidate)
                Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            
            InitializeTitleFont();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (titleFont != null)
                {
                    titleFont.Dispose();
                    titleFont = null;
                }

                if (image != null)
                {
                    image.Dispose();
                    image = null;
                }
            }
            
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var borderRect = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);

            if (State == ButtonState.Active)
            {
                using (var pen = new Pen(borderColor))
                    e.Graphics.DrawRectangle(pen, borderRect);
            }
            else if (State == ButtonState.Pressed)
            {
                using (var brush = new SolidBrush(Color.FromArgb(120, borderColor)))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
            }
            
            var paddingRect = new Rectangle(
                Padding.Left,
                Padding.Top,
                ClientSize.Width - Padding.Left - Padding.Right,
                ClientSize.Height - Padding.Top - Padding.Bottom);

            var dx = 0;

            if (image != null && !DesignMode)
            {
                var srcRect = new Rectangle(0, 0, image.Width, image.Height);
                var destRect = new Rectangle(
                    paddingRect.X,
                    paddingRect.Y + paddingRect.Height / 4 - image.Height / 2,
                    image.Width,
                    image.Height);

                if (image.Height > paddingRect.Height)
                {
                    var ratio = (1.0f*image.Width)/image.Height;
                    destRect.Height = paddingRect.Height;
                    destRect.Width = Convert.ToInt32(ratio * paddingRect.Height);
                    destRect.Y = paddingRect.Y + paddingRect.Height / 4 - destRect.Height / 2;
                }

                if (destRect.Right < paddingRect.Width/2)
                {
                    e.Graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
                    dx = destRect.Width;
                }
            }

            var titleRect = new Rectangle(
                paddingRect.X + dx,
                paddingRect.Y,
                paddingRect.Width - dx,
                paddingRect.Height / 2);

            var textRect = new Rectangle(
                paddingRect.X + dx,
                paddingRect.Y + paddingRect.Height / 2,
                paddingRect.Width - dx,
                paddingRect.Height / 2);
            
            if (!string.IsNullOrEmpty(title))
                TextRenderer.DrawText(e.Graphics, title, titleFont, titleRect, titleForeColor, TitleFormat);

            if (!string.IsNullOrEmpty(Text))
                TextRenderer.DrawText(e.Graphics, Text, Font, textRect, ForeColor, TextFormat);
        }
    }
}