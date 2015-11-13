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
        private const int spritePower = 4;

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

            var size = BackgroundImage != null ? BackgroundImage.Size : Size.Empty;

            switch (spriteOrientation)
            {
                case SpriteOrientation.Horizontal:
                    Size = new Size(size.Width / spritePower, size.Height);
                    break;
                case SpriteOrientation.Vertical:
                    Size = new Size(size.Width, size.Height / spritePower);
                    break;
                default:
                    Size = size.Width > size.Height ?
                        new Size(size.Width / spritePower, size.Height) :
                        new Size(size.Width, size.Height / spritePower);
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

            var delta = Enabled ? (int)State : spritePower - 1;
            var destRect = Rectangle.Empty;
            var sourceRect = Rectangle.Empty;

            switch (spriteOrientation)
            {
                case SpriteOrientation.Horizontal:
                    destRect.Size = new Size(BackgroundImage.Width / spritePower, BackgroundImage.Height);
                    sourceRect.Location = new Point(delta * (BackgroundImage.Width / spritePower), 0);
                    break;
                case SpriteOrientation.Vertical:
                    destRect.Size = new Size(BackgroundImage.Width, BackgroundImage.Height / spritePower);
                    sourceRect.Location = new Point(0, delta * (BackgroundImage.Height / spritePower));
                    break;
                default:
                    if (BackgroundImage.Width > BackgroundImage.Height)
                    {
                        destRect.Size = new Size(BackgroundImage.Width / spritePower, BackgroundImage.Height);
                        sourceRect.Location = new Point(delta * (BackgroundImage.Width / spritePower), 0);
                    }
                    else
                    {
                        destRect.Size = new Size(BackgroundImage.Width, BackgroundImage.Height / spritePower);
                        sourceRect.Location = new Point(0, delta * (BackgroundImage.Height / spritePower));
                    }
                    break;
            }

            sourceRect.Size = destRect.Size;
            e.Graphics.DrawImage(BackgroundImage, destRect, sourceRect, GraphicsUnit.Pixel);
        }

        protected override void OnPaintFocus(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var pen = new Pen(SystemColors.Highlight, 2.0f))
                e.Graphics.DrawEllipse(pen, 1, 1, ClientSize.Width - spritePower, ClientSize.Height - spritePower);

            e.Graphics.SmoothingMode = SmoothingMode.Default;
        }
    }
}