using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Rift.Forms
{
    /// <summary>
    /// Represents a panel Windows control that contains a text field.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class FieldPanel : Panel
    {
        private Color borderColor;
        private Color borderActiveColor;

        /// <summary>
        /// Indicates the border style for the control.
        /// </summary>
        [Browsable(false)]
        public new BorderStyle BorderStyle
        {
            get
            {
                return base.BorderStyle;
            }
        }

        /// <summary>
        /// Gets or sets a control border color.
        /// </summary>
        [Category("Appearance")]
        [Description("Control border color to set.")]
        [DefaultValue(typeof (Color), "Control")]
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
        /// Creates a new instance of the <see cref="Rift.Forms.FieldPanel"/> class.
        /// </summary>
        public FieldPanel()
        {
            base.BorderStyle = BorderStyle.None;
            borderColor = SystemColors.Control;
            borderActiveColor = SystemColors.Highlight;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (Controls.Count > 0)
                Controls[0].Focus();
        }
        
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (Controls.Count != 1) return;

            e.Control.LostFocus += Control_LostFocus;
            e.Control.GotFocus += Control_GotFocus;
        }

        private void Control_GotFocus(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Control_LostFocus(object sender, EventArgs e)
        {
            Invalidate();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var color = ContainsFocus ? BorderActiveColor : BorderColor;

            using (var pen = new Pen(color))
                e.Graphics.DrawRectangle(
                    pen,
                    0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing &&
                Controls.Count > 0)
            {
                Controls[0].LostFocus -= Control_LostFocus;
                Controls[0].GotFocus -= Control_GotFocus;
            }
            
            base.Dispose(disposing);
        }
    }
}