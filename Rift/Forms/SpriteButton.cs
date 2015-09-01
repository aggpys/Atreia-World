using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Rift.Forms
{
    /// <summary>
    /// Specifies an orientations of the sprite images.
    /// </summary>
    public enum SpriteOrientation
    {
        /// <summary>
        /// Auto orientation. The biggest side of the sprite image.
        /// </summary>
        Auto,
        /// <summary>
        /// Horizontal orientation.
        /// </summary>
        Horizontal,
        /// <summary>
        /// Vertical orientation.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Represents the button control with a sprite images background.
    /// </summary>
    public class SpriteButton : RiftButtonBase
    {
        private SpriteOrientation spriteOrientation;

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
        public override Image BackgroundImage
        {
            get { return base.BackgroundImage; }
            set
            {
                base.BackgroundImage = value;
                FitControlToSprite();
            }
        }

        /// <summary>
        /// Gets or sets the sprite image orientation.
        /// </summary>
        [Category("Appearance")]
        [Description("A sprite image orientation.")]
        [DefaultValue(typeof (SpriteOrientation), "Auto")]
        public SpriteOrientation SpriteOrientation
        {
            get { return spriteOrientation; }
            set
            {
                var changed = value != spriteOrientation;
                spriteOrientation = value;
                
                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets the background image layout.
        /// </summary>
        [Browsable(false)]
        public override ImageLayout BackgroundImageLayout { get { return ImageLayout.None; } }

        /// <summary>
        /// Gets the text associated with this control.
        /// </summary>
        [Browsable(false)]
        public override string Text { get { return string.Empty; } }

        protected override Size DefaultSize { get { return new Size(8, 8); } }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Forms.SpriteButton"/> class.
        /// </summary>
        public SpriteButton()
        {
            base.BackColor = Color.Transparent;
            base.Text = string.Empty;
            Size = new Size(48, 24);
            spriteOrientation = SpriteOrientation.Auto;
        }

        private void FitControlToSprite()
        {

            var size = BackgroundImage != null ? 
                BackgroundImage.Size :
                Size.Empty;

            switch (spriteOrientation)
            {
                case SpriteOrientation.Horizontal:
                    Size = new Size(size.Width / 3, size.Height);
                    break;
                case SpriteOrientation.Vertical:
                    Size = new Size(size.Width, size.Height / 3);
                    break;
                default:
                    Size = size.Width > size.Height ?
                        new Size(size.Width / 3, size.Height) :
                        new Size(size.Width, size.Height / 3);
                    break;
            }
        }

        protected virtual void OnPaintBackContent(PaintEventArgs e) { }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            OnPaintBackContent(e);

            if (BackgroundImage == null)
                return;

            var delta = (int)State;
            var destRect = Rectangle.Empty;
            var sourceRect = Rectangle.Empty;

            switch (spriteOrientation)
            {
                case SpriteOrientation.Horizontal:
                    destRect.Size = new Size(BackgroundImage.Width / 3, BackgroundImage.Height);
                    sourceRect.Location = new Point(delta * (BackgroundImage.Width / 3), 0);
                    break;
                case SpriteOrientation.Vertical:
                    destRect.Size = new Size(BackgroundImage.Width, BackgroundImage.Height / 3);
                    sourceRect.Location = new Point(0, delta * (BackgroundImage.Height / 3));
                    break;
                default:
                    if (BackgroundImage.Width > BackgroundImage.Height)
                    {
                        destRect.Size = new Size(BackgroundImage.Width / 3, BackgroundImage.Height);
                        sourceRect.Location = new Point(delta * (BackgroundImage.Width / 3), 0);
                    }
                    else
                    {
                        destRect.Size = new Size(BackgroundImage.Width, BackgroundImage.Height / 3);
                        sourceRect.Location = new Point(0, delta * (BackgroundImage.Height / 3));
                    }
                    break;
            }

            sourceRect.Size = destRect.Size;

            if (Enabled)
                e.Graphics.DrawImage(BackgroundImage, destRect, sourceRect, GraphicsUnit.Pixel);
            else
            {
                using (var tempBitmap = new Bitmap(sourceRect.Width, sourceRect.Height, PixelFormat.Format32bppArgb))
                {
                    using (var g = Graphics.FromImage(tempBitmap))
                        g.DrawImage(BackgroundImage, destRect, sourceRect, GraphicsUnit.Pixel);

                    ControlPaint.DrawImageDisabled(e.Graphics, tempBitmap, 0, 0, BackColor);
                }
            }
        }

        protected override void OnPaintFocus(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var pen = new Pen(SystemColors.Highlight, 2.0f))
                e.Graphics.DrawEllipse(pen, 1, 1, ClientSize.Width - 3, ClientSize.Height - 3);

            e.Graphics.SmoothingMode = SmoothingMode.Default;
        }
    }
}