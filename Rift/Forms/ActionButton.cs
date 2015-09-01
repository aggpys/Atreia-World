using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rift.Forms
{
    /// <summary>
    /// Represents an action button Windows control.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ActionButton : SpriteButton
    {
        private readonly Timer timer;
        private readonly StringFormat textFormat;

        private Color borderColor;
        private Color borderActiveColor;
        private Color progressColor;

        private int progressMin;
        private int progressMax;
        private int progressValue;
        private int progressPrintValue;

        private string text;

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        [Browsable(true)]
        [Localizable(true)]
        public override string Text
        {
            get { return text; }
            set
            {
                var changed = !string.Equals(text, value, StringComparison.OrdinalIgnoreCase);
                text = value;

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the progress bar value.
        /// </summary>
        [Category("Progress")]
        [Description("Progress bar value to set.")]
        [DefaultValue(0)]
        public int ProgressValue
        {
            get { return progressValue; }
            set
            {
                if (value < progressMin ||
                    value > progressMax)
                    return;

                var changed = progressValue != value;
                progressValue = value;

                if (changed && progressValue != progressPrintValue)
                {
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// Gets or sets the progress bar minimum value.
        /// </summary>
        [Category("Progress")]
        [Description("Progress bar minimum value to set.")]
        [DefaultValue(0)]
        public int ProgressMinimum
        {
            get { return progressMin; }
            set
            {
                if (value > progressMax)
                    return;

                var changed = progressMin != value;
                progressMin = value;

                if (progressValue < progressMin)
                {
                    ProgressValue = value;
                    return;
                }

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the progress bar maximum value.
        /// </summary>
        [Category("Progress")]
        [Description("Progress bar maximum value to set.")]
        [DefaultValue(100)]
        public int ProgressMaximum
        {
            get { return progressMax; }
            set
            {
                if (value < progressMin)
                    return;

                var changed = progressMax != value;
                progressMax = value;

                if (progressValue > progressMax)
                {
                    ProgressValue = value;
                    return;
                }

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a control border color.
        /// </summary>
        [Category("Appearance")]
        [Description("Control border color to set.")]
        [DefaultValue(typeof(Color), "Control")]
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
        /// Gets or sets a control active border color.
        /// </summary>
        [Category("Appearance")]
        [Description("Control active border color to set.")]
        [DefaultValue(typeof(Color), "Highlight")]
        public Color BorderActiveColor
        {
            get { return borderActiveColor; }
            set
            {
                var changed = borderActiveColor != value;
                borderActiveColor = value;

                if (changed)
                    Invalidate();
            }
        }
        
        /// <summary>
        /// Gets or sets a control progress line color.
        /// </summary>
        [Category("Appearance")]
        [Description("Control progress line color to set.")]
        [DefaultValue(typeof(Color), "Highlight")]
        public Color ProgressColor
        {
            get { return progressColor; }
            set
            {
                var changed = progressColor != value;
                progressColor = value;

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Forms.ActionButton"/> class.
        /// </summary>
        public ActionButton()
        {
            borderColor = SystemColors.Control;
            borderActiveColor = SystemColors.Highlight;
            progressColor = borderActiveColor;
            progressValue = 0;
            progressPrintValue = 0;
            progressMin = 0;
            progressMax = 100;

            textFormat = new StringFormat(StringFormatFlags.NoWrap)
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.Character
            };

            timer = new Timer
            {
                Interval = 10,
                Enabled = true
            };

            timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (progressValue == progressPrintValue)
            {
                timer.Stop();
                return;
            }

            if (progressValue == progressMin)
                progressPrintValue = progressMin;
            else
            {
                var d = progressValue > progressPrintValue ? 1 : -1;
                progressPrintValue += d;
            }

            if (progressPrintValue > progressMax)
                progressPrintValue = progressMax;

            if (progressPrintValue < progressMin)
                progressPrintValue = progressMin;

            Invalidate();
        }

        protected override void OnPaintBackContent(PaintEventArgs e)
        {
            if (progressPrintValue == 0) return;

            var ratio = (float) progressPrintValue/(progressMax - progressMin);
            var rect = new Rectangle(
                0, 
                0,
                Convert.ToInt32(ClientSize.Width*ratio),
                ClientSize.Height);

            using (var brush = new SolidBrush(ProgressColor))
                e.Graphics.FillRectangle(brush, rect);
        }

        protected override void OnPaintFocus(PaintEventArgs e)
        {
            using (var pen = new Pen(SystemColors.Highlight))
                e.Graphics.DrawRectangle(pen, 1, 1, ClientSize.Width - 3, ClientSize.Height - 3);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (string.IsNullOrEmpty(Text)) return;

            using (var brush = new SolidBrush(ForeColor))
                e.Graphics.DrawString(Text, Font, brush, ClientRectangle, textFormat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Tick -= timer_Tick;
                    timer.Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}