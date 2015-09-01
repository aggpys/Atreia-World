using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rift.Data;
using Rift.Properties;
using Rift.Utils;

namespace Rift.Forms
{
    /// <summary>
    /// Represents panel Windows control that displays
    /// a <see cref="Rift.Data.ShopItem"/> object.
    /// </summary>
    public sealed class ShopItemPanel : RiftButtonBase
    {
        private const TextFormatFlags DefaultTextFormat =
            TextFormatFlags.WordBreak |
            TextFormatFlags.WordEllipsis |
            TextFormatFlags.VerticalCenter |
            TextFormatFlags.HorizontalCenter;

        private const TextFormatFlags CountTextFormat =
            TextFormatFlags.Bottom |
            TextFormatFlags.Right;

        private const int DefaultIconSize = 40;
        private const int BackgroundAlpha = 50;
        private const int MixinAlpha = 20;

        private Image itemIcon;
        private readonly string priceText;

        private readonly Color titleColor;
        private readonly Color backgroundColor;

        /// <summary>
        /// Gets a <see cref="Rift.Data.ShopItem"/> associated
        /// with this control.
        /// </summary>
        public ShopItem ContainedItem { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Forms.ShopItemPanel"/> class
        /// from the specified <see cref="Rift.Data.ShopItem"/>.
        /// </summary>
        /// <param name="item">A <see cref="Rift.Data.ShopItem"/> to view.</param>
        public ShopItemPanel(ShopItem item)
        {
            ContainedItem = item;
            itemIcon = null;
            priceText = string.Format("{0}@", item.Price);

            titleColor = QualityColorHelper.GetForeColor(item.Quality);
            backgroundColor = Color.FromArgb(BackgroundAlpha, QualityColorHelper.GetBackColor(item.Quality));
            
            if(!string.IsNullOrEmpty(item.IconUri))
                App.CurrentContext.Cache.GetImageAsync(IconPathResolver.ExpandUri(item.IconUri), HandleImage);
        }

        private void HandleImage(Image icon)
        {
            itemIcon = icon;

            if (itemIcon != null)
                Invalidate();
        }

        protected override Size DefaultSize
        {
            get { return new Size(144, 100); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (itemIcon != null)
                    itemIcon.Dispose();
            }
            
            base.Dispose(disposing);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (e.ClipRectangle.IsEmpty)
                return;

            base.OnPaintBackground(e);
            
            if (!Enabled) return;

            using (var brush = new SolidBrush(backgroundColor))
                e.Graphics.FillRectangle(brush, ClientRectangle);

            var mixinColor = Color.Transparent;

            switch (State)
            {
                case ButtonState.Active:
                    mixinColor = Color.FromArgb(MixinAlpha, Color.White);
                    break;
                case ButtonState.Pressed:
                    mixinColor = Color.FromArgb(MixinAlpha, Color.Black);
                    break;
            }

            if (State != ButtonState.Inactive)
                using (var brush = new SolidBrush(mixinColor))
                    e.Graphics.FillRectangle(brush, ClientRectangle);

            if (!Focused) return;

            var path = new GraphicsPath();

            path.StartFigure();
            path.AddPolygon(new []
            {
                new Point(ClientSize.Width - ClientSize.Width/8, ClientSize.Height),
                new Point(ClientSize.Width, ClientSize.Height - ClientSize.Width/8),
                new Point(ClientSize.Width, ClientSize.Height) 
            });
            path.CloseFigure();

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new SolidBrush(ForeColor))
                e.Graphics.FillPath(brush, path);

            e.Graphics.SmoothingMode = SmoothingMode.Default;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (e.ClipRectangle.IsEmpty)
                return;

            base.OnPaint(e);

            var fontSize = TextRenderer.MeasureText(ContainedItem.Title, Font);

            var priceRect = new Rectangle(
                0,
                ClientSize.Height - fontSize.Height - 1,
                ClientSize.Width,
                fontSize.Height);
            var titleRect = new Rectangle(
                0,
                priceRect.Top - 2*fontSize.Height - 1,
                ClientSize.Width,
                2*fontSize.Height);
            var destRect = new Rectangle(
                    ClientSize.Width / 2 - DefaultIconSize / 2,
                    titleRect.Top / 2 - DefaultIconSize / 2,
                    DefaultIconSize,
                    DefaultIconSize);

            TextRenderer.DrawText(e.Graphics, ContainedItem.Title, Font, titleRect, titleColor, DefaultTextFormat);
            TextRenderer.DrawText(e.Graphics, priceText, Font, priceRect, ForeColor, DefaultTextFormat);

            if (!DesignMode)
            {
                var srcRect = Rectangle.Empty;
                
                if (ContainedItem.Restriction != ItemRaceRestriction.Universal)
                {
                    /*
                    var color = ContainedItem.Restriction == ItemRaceRestriction.Asmodians
                        ? Color.CadetBlue
                        : Color.OliveDrab;

                    var racePath = new GraphicsPath();

                    racePath.StartFigure();
                    racePath.AddPolygon(new[]
                    {
                    new Point(0, 0),
                    new Point(ClientSize.Width/8, 0),
                    new Point(0, ClientSize.Width/8)
                    });
                    racePath.CloseFigure();

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    using (var brush = new SolidBrush(color))
                        e.Graphics.FillPath(brush, racePath);

                    e.Graphics.SmoothingMode = SmoothingMode.Default;
                    */

                    using (var raceImage = ContainedItem.Restriction == ItemRaceRestriction.Elyos
                            ? Resources.ImageElyos
                            : Resources.ImageAsmodians)
                        e.Graphics.DrawImage(raceImage, 6, 6);
                }

                if (itemIcon != null)
                {
                    srcRect.Size = itemIcon.Size;
                    e.Graphics.DrawImage(itemIcon, destRect, srcRect, GraphicsUnit.Pixel);
                }
                else
                {
                    using (var unknownIcon = Resources.ImageUnknownItem)
                    {
                        srcRect.Size = unknownIcon.Size;
                        e.Graphics.DrawImage(unknownIcon, destRect, srcRect, GraphicsUnit.Pixel);
                    }
                }

                if (ContainedItem.Count > 1)
                {
                    var countText = string.Format(@"×{0}", ContainedItem.Count);
                    var countRect = new Rectangle(
                        destRect.Left + 2,
                        destRect.Top + 2,
                        destRect.Width - 4,
                        destRect.Height - 4);

                    using (var brush = new LinearGradientBrush(
                        countRect,
                        Color.Transparent,
                        Color.FromArgb(200, Color.Black),
                        45.0f))
                    {
                        e.Graphics.FillRectangle(brush, destRect);
                        TextRenderer.DrawText(e.Graphics, countText, Font, countRect, Color.White, CountTextFormat);
                    }
                }
            }

            var opacity = State == ButtonState.Active ? BackgroundAlpha : MixinAlpha;
            var borderColor = Color.FromArgb(opacity, titleColor);

            using (var pen = new Pen(borderColor))
                e.Graphics.DrawRectangle(
                    pen,
                    0,
                    0,
                    ClientSize.Width - 1,
                    ClientSize.Height - 1);
        }
    }
}