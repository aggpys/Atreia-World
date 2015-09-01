using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Rift.Utils;

namespace Rift.Forms
{
    /// <summary>
    /// Represents the base class for the application buttons.
    /// </summary>
    public class RiftButtonBase : Control
    {
        /// <summary>
        /// Occurs when the control is used.
        /// </summary>
        public event EventHandler Action;

        /// <summary>
        /// Gets the current button state.
        /// </summary>
        [Browsable(false)]
        public ButtonState State { get; private set; }

        /// <summary>
        /// Gets or sets an accept keyboard key.
        /// </summary>
        [Category("Behaviour")]
        [Description("Accept keyboard key to set.")]
        [DefaultValue(typeof (Keys), "Space")]
        public Keys AcceptKey { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Forms.RiftButtonBase"/> class.
        /// </summary>
        public RiftButtonBase()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor,
                true);

            AcceptKey = Keys.Space;
            State = ButtonState.Inactive;
        }

        protected override Cursor DefaultCursor
        {
            get
            {
                var handCursor = Win32.LoadCursor(IntPtr.Zero, Win32.IDC_HAND);
                var temp = new Cursor(handCursor);

                Win32.DestroyCursor(handCursor);

                return temp;
            }
        }

        protected virtual void OnPaintFocus(PaintEventArgs e) { }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (Focused && State == ButtonState.Inactive)
                OnPaintFocus(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            
            if (State == ButtonState.Pressed)
                return;

            if (!IsMouseOver(e.Location))
            {
                var changed = State != ButtonState.Inactive;
                State = ButtonState.Inactive;

                if (changed)
                    Invalidate();
            }
            else
            {
                var changed = State != ButtonState.Active;
                State = ButtonState.Active;

                if (changed)
                    Invalidate();
            }
        }

        protected virtual bool IsMouseOver(Point p)
        {
            return ClientRectangle.Contains(p);
        }

        protected virtual void OnAction(EventArgs e)
        {
            if (Action != null)
                Action(this, e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            var changed = State != ButtonState.Inactive;
            State = ButtonState.Inactive;

            if (changed)
                Invalidate();
        }
        
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (!IsMouseOver(e.Location))
                return;

            var changed = State != ButtonState.Pressed;
            State = ButtonState.Pressed;

            if (changed)
                Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!ClientRectangle.Contains(e.Location))
                return;

            if (e.Button != MouseButtons.Left)
            {
                base.OnMouseUp(e);
                return;
            }

            var changed = State != ButtonState.Active;
            State = ButtonState.Active;

            if (changed)
                Invalidate();

            OnAction(EventArgs.Empty);

            base.OnMouseUp(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            State = ButtonState.Inactive;
            Invalidate();
        }
        
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == AcceptKey)
            {
                var changed = State != ButtonState.Pressed;
                State = ButtonState.Pressed;

                if (changed)
                    Invalidate();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (State == ButtonState.Inactive)
                return;

            if (e.KeyCode == AcceptKey)
            {
                var changed = State != ButtonState.Active;
                State = ButtonState.Active;

                if (changed)
                    Invalidate();

                OnAction(EventArgs.Empty);
            }
        }
    }
}