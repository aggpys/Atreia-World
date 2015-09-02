using System;
using System.Windows.Forms;
using Rift.Properties;
using Rift.Utils;

namespace Rift.Forms
{
    /// <summary>
    /// Represents the Windows flow layout panel control.
    /// This class cannot be inherited. 
    /// </summary>
    public sealed class FlowLayoutPanel : System.Windows.Forms.FlowLayoutPanel
    {
        private int childCount;

        /// <summary>
        /// Gets or sets the panel child count.
        /// </summary>
        public int ChildCount
        {
            get { return childCount; }
            set
            {
                var changed = childCount != value;
                childCount = value;

                if (changed)
                    Invalidate();
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Forms.FlowLayoutPanel"/> class.
        /// </summary>
        public FlowLayoutPanel()
        {
            childCount = 0;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.UserPaint, 
                true);
        }
        
        protected override CreateParams CreateParams
        {
            get
            {
                var temp = base.CreateParams;
                temp.ExStyle |= Win32.WS_EX_COMPOSITED;
                return temp;
            }
        }
        
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            Focus();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Controls.Count == childCount) return;
            TextRenderer.DrawText(
                e.Graphics, 
                Resources.TextLoading, 
                Font, 
                ClientRectangle, 
                ForeColor, 
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}