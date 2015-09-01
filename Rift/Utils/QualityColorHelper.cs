using System.Collections.Generic;
using System.Drawing;
using Rift.Data;

namespace Rift.Utils
{
    /// <summary>
    /// Provides color data for the <see cref="Rift.Data.ItemQuality"/>.
    /// </summary>
    public static class QualityColorHelper
    {
        private struct ColorData
        {
            public Color BackColor; // Background primary color.
            public Color ForeColor; // Foreground color (text color).
        }

        private static readonly Dictionary<ItemQuality, ColorData> colors;

        static QualityColorHelper()
        {
            colors = new Dictionary<ItemQuality, ColorData>
            {
                { ItemQuality.Junk, new ColorData { ForeColor = Color.FromArgb(60, 60, 60), BackColor = Color.FromArgb(230, 230, 230) } },
                { ItemQuality.Common, new ColorData { ForeColor = Color.FromArgb(30, 30, 30), BackColor = Color.FromArgb(200, 200, 210) } },
                { ItemQuality.Rare, new ColorData { ForeColor = Color.FromArgb(105, 225, 94), BackColor = Color.FromArgb(182, 224, 178) } },
                { ItemQuality.Legend, new ColorData { ForeColor = Color.FromArgb(76, 197, 208), BackColor = Color.FromArgb(204, 239, 255) } },
                { ItemQuality.Unique, new ColorData { ForeColor = Color.FromArgb(240, 183, 28), BackColor = Color.FromArgb(240, 227, 192) } },
                { ItemQuality.Epic, new ColorData { ForeColor = Color.FromArgb(240, 128, 51), BackColor = Color.FromArgb(240, 211, 192) } },
                { ItemQuality.Mythic, new ColorData { ForeColor = Color.FromArgb(112, 54, 206), BackColor = Color.FromArgb(157, 131, 163) } }
            };
        }

        /// <summary>
        /// Returns the foreground (text) <see cref="System.Drawing.Color"/> for the specified <see cref="Rift.Data.ItemQuality"/>
        /// </summary>
        /// <param name="quality">A <see cref="Rift.Data.ItemQuality"/> to colorize.</param>
        public static Color GetForeColor(ItemQuality quality)
        {
            return colors[quality].ForeColor;
        }

        /// <summary>
        /// Returns the background <see cref="System.Drawing.Color"/> for the specified <see cref="Rift.Data.ItemQuality"/>
        /// </summary>
        /// <param name="quality">A <see cref="Rift.Data.ItemQuality"/> to colorize.</param>
        public static Color GetBackColor(ItemQuality quality)
        {
            return colors[quality].BackColor;
        }
    }
}