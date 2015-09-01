using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Rift.Properties;
using Rift.Utils;

namespace Rift.Forms
{
    /// <summary>
    /// Represents the common application window or dialog
    /// that makes up a user interface.
    /// </summary>
    public class RiftForm : Form
    {
        private bool caption;
        private bool thinBorder;

        private Color activeBorderColor;
        private Color inactiveBorderColor;
        private Color activeCaptionTextColor;
        private Color inactiveCaptionTextColor;

        private SpriteButton minimizeButton;
        private SpriteButton closeButton;

        private readonly StringFormat captionStringFormat;

        /// <summary>
        /// Gets or sets the form text.
        /// </summary>
        [Browsable(true)]
        [Description(@"Window caption text to set.")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                var changed = !string.Equals(base.Text, value);
                base.Text = value;

                if (changed)
                    Invalidate(); 
            }
        }

        /// <summary>
        /// Gets the system border style of the form.
        /// </summary>
        [Browsable(false)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the Maximize button
        /// is displayed in the caption bar of the form.
        /// </summary>
        [Browsable(false)]
        public new bool MaximizeBox
        {
            get { return base.MaximizeBox; }
            set { base.MaximizeBox = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether the Minimize button is displayed
        /// in the caption bar of the form.
        /// </summary>
        public new bool MinimizeBox
        {
            get { return base.MinimizeBox; }
            set
            {
                base.MinimizeBox = value;
                UpdateControlBox();
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether a control box is displayed
        /// in the caption bar of the form.
        /// </summary>
        public new bool ControlBox
        {
            get { return base.ControlBox; }
            set
            {
                base.ControlBox = value;
                UpdateControlBox();
            }
        }

        /// <summary>
        /// Indicates whether the default form closing behavior is enabled.
        /// </summary>
        [Category("Appearance")]
        [Description("Enables or disables the default form closing behavior.")]
        [DefaultValue(true)]
        public bool ClosingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the active border color of the form.
        /// </summary>
        [Category("Appearance")]
        [Description("The form active border color to set.")]
        [DefaultValue(typeof (Color), "ActiveBorder")]
        public Color BorderColorActive
        {
            get { return activeBorderColor; }
            set
            {
                var changed = activeBorderColor != value;
                activeBorderColor = value;

                if (changed &&
                    thinBorder)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the inactive border color of the form.
        /// </summary>
        [Category("Appearance")]
        [Description("The form inactive border color to set.")]
        [DefaultValue(typeof (Color), "InactiveBorder")]
        public Color BorderColorInactive
        {
            get { return inactiveBorderColor; }
            set
            {
                var changed = inactiveBorderColor != value;
                inactiveBorderColor = value;

                if (changed &&
                    thinBorder)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether
        /// the form thin border is displayed to the user.
        /// </summary>
        [Category("Appearance")]
        [Description("Enables or disables thin border of the form.")]
        [DefaultValue(true)]
        public bool ThinBorderEnabled
        {
            get { return thinBorder; }
            set
            {
                var changed = thinBorder != value;
                thinBorder = value;

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the active caption text color of the form.
        /// </summary>
        [Category("Appearance")]
        [Description("The form active caption text color to set.")]
        [DefaultValue(typeof(Color), "ActiveCaptionText")]
        public Color CaptionTextColorActive
        {
            get { return activeCaptionTextColor; }
            set
            {
                var changed = activeCaptionTextColor != value;
                activeCaptionTextColor = value;

                if (changed &&
                    caption)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the inactive caption text color of the form.
        /// </summary>
        [Category("Appearance")]
        [Description("The form inactive caption text color to set.")]
        [DefaultValue(typeof(Color), "InactiveCaptionText")]
        public Color CaptionTextColorInactive
        {
            get { return inactiveCaptionTextColor; }
            set
            {
                var changed = inactiveCaptionTextColor != value;
                inactiveCaptionTextColor = value;

                if (changed &&
                    caption)
                    Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether
        /// the form caption is displayed for the user.
        /// </summary>
        [Category("Appearance")]
        [Description("Enables or disables the form caption.")]
        [DefaultValue(false)]
        public bool CaptionEnabled
        {
            get { return caption; }
            set
            {
                var changed = caption != value;
                caption = value;

                if (changed)
                    Invalidate();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                // Applies the 'minimize' form behaviour.
                cp.Style |= Win32.WS_MINIMIZEBOX;
                cp.ClassStyle |= Win32.CS_DBLCLKS;

                // Applies the drop shadow effect (Windows XP and above).
                if (Environment.OSVersion.Version.Major < 6)
                    cp.ClassStyle |= Win32.CS_DROPSHADOW;
                
                return cp;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RiftForm"/> class.
        /// </summary>
        public RiftForm()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            InitializeControlBox();
            
            base.Font = SystemFonts.MessageBoxFont;
            base.FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            AutoScaleMode = AutoScaleMode.None;
            Icon = Resources.RiftIcon;
            MaximizeBox = false;
            MinimizeBox = false;
            ControlBox = false;

            ClosingEnabled = true;

            inactiveBorderColor = SystemColors.InactiveBorder;
            activeBorderColor = SystemColors.ActiveBorder;
            inactiveCaptionTextColor = SystemColors.InactiveCaptionText;
            activeCaptionTextColor = SystemColors.ActiveCaptionText;
            thinBorder = true;

            captionStringFormat = new StringFormat(StringFormatFlags.NoWrap)
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (closeButton != null)
                {
                    closeButton.Action -= closeButton_Action;
                    closeButton.Dispose();
                }

                if (minimizeButton != null)
                {
                    minimizeButton.Action -= minimizeButton_Action;
                    minimizeButton.Dispose();
                }
            }

            base.Dispose(disposing);
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateControlBox();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateControlBox();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            // Applies DWM drop shadow effect to the window (Windows Vista +).
            if (Environment.OSVersion.Version.Major >= 6)
            {
                try
                {
                    var n = 2;
                    var result = Win32.DwmSetWindowAttribute(Handle, 2, ref n, 4);

                    if (result == 0)
                    {
                        var m = new Win32.MARGINS(1, 1, 1, 1); // 1px form border (do NOT use #000000 color).
                        Win32.DwmExtendFrameIntoClientArea(Handle, ref m);
                    }
                }
                catch (DllNotFoundException) { }
            }

            base.OnHandleCreated(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left) return;

            var dragAllowed = true;

            if (caption)
            {
                var captionArea = GetCaptionArea();
                dragAllowed = captionArea.Contains(e.Location);
            }

            if (!dragAllowed) return;

            Win32.ReleaseCapture();
            Win32.SendMessage(Handle, Win32.WM_NCLBUTTONDOWN, Win32.HTCAPTION, 0);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Invalidate();
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);
            Invalidate();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!ClosingEnabled)
            {
                e.Cancel = true;
                Hide();
            }

            base.OnClosing(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            PaintBackground(e.Graphics);
            PaintBorder(e.Graphics);
            PaintCaption(e.Graphics);
        }

        /// <summary>
        /// Displays the form to the user.
        /// </summary>
        public void ShowAtFront()
        {
            Show();
            
            var topMost = TopMost;
            TopMost = true;
            TopMost = topMost;
        }

        private void InitializeControlBox()
        {
            minimizeButton = new SpriteButton();
            closeButton = new SpriteButton();

            minimizeButton.BackgroundImage = Resources.ImageSpriteMinimize;
            minimizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            minimizeButton.Action += minimizeButton_Action;
            minimizeButton.BringToFront();
            minimizeButton.TabIndex = 0;
            minimizeButton.TabStop = false;
            closeButton.BackgroundImage = Resources.ImageSpriteClose;
            closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeButton.Action += closeButton_Action;
            closeButton.BringToFront();
            closeButton.TabIndex = 0;
            closeButton.TabStop = false;

            Controls.Add(closeButton);
            Controls.Add(minimizeButton);
        }

        private void minimizeButton_Action(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void closeButton_Action(object sender, EventArgs e)
        {
            Close();
        }
        
        private void UpdateControlBox()
        {
            SuspendLayout();

            closeButton.Location = new Point(ClientSize.Width - closeButton.Width, 0);
            minimizeButton.Location = new Point(closeButton.Left - minimizeButton.Width, 0);

            closeButton.Visible = minimizeButton.Visible = ControlBox;
            minimizeButton.Visible = ControlBox && MinimizeBox;
            
            ResumeLayout();
        }

        private Rectangle GetCaptionArea()
        {
            var height = ControlBox ? closeButton.Height : SystemFonts.CaptionFont.Height + 6;
            var width = ClientSize.Width;

            if (ControlBox) width -= closeButton.Width;
            if (MinimizeBox) width -= minimizeButton.Width;

            return new Rectangle(0, 0, width, height);
        }
        
        protected override Size DefaultSize { get { return new Size(523, 253); } }

        protected virtual void PaintBackground(Graphics g) { }

        protected virtual void PaintCaption(Graphics g)
        {
            if (!caption) return;

            var captionRect = GetCaptionArea();
            var captionFillRect = new Rectangle(0, 0, ClientSize.Width, captionRect.Height);

            using (var brush = new SolidBrush(ContainsFocus ? activeBorderColor : inactiveBorderColor))
                g.FillRectangle(brush, captionFillRect);

            if (string.IsNullOrEmpty(Text)) return;

            using (var brush = new SolidBrush(ContainsFocus ? activeCaptionTextColor : inactiveCaptionTextColor))
                g.DrawString(Text, SystemFonts.CaptionFont, brush, captionRect, captionStringFormat);
        }

        protected virtual void PaintBorder(Graphics g)
        {
            if (!thinBorder) return;

            var borderRight = new Rectangle(
                ClientSize.Width - 1, ClientSize.Height/3,
                1, ClientSize.Height - ClientSize.Height/3);

            var borderBottom = new Rectangle(
                ClientSize.Width/3, ClientSize.Height - 1,
                ClientSize.Width - ClientSize.Width/3, 1);

            var borderLeft = new Rectangle(0, 0, 1, ClientSize.Height/3);

            var borderTop = new Rectangle(0, 0, ClientSize.Width/3, 1);

            var color = ContainsFocus ? activeBorderColor : inactiveBorderColor;

            using (var rightBrush = new LinearGradientBrush(
                borderRight,
                Color.Transparent,
                color,
                LinearGradientMode.Vertical))
            using (var bottomBrush = new LinearGradientBrush(
                borderBottom,
                Color.Transparent,
                color,
                LinearGradientMode.Horizontal))
            using (var leftBrush = new LinearGradientBrush(
                borderLeft,
                color,
                Color.Transparent,
                LinearGradientMode.Vertical))
            using (var topBrush = new LinearGradientBrush(
                borderTop,
                color,
                Color.Transparent,
                LinearGradientMode.Horizontal))
            {
                g.FillRectangle(rightBrush, borderRight);
                g.FillRectangle(bottomBrush, borderBottom);
                g.FillRectangle(leftBrush, borderLeft);
                g.FillRectangle(topBrush, borderTop);
            }
        }
    }
}