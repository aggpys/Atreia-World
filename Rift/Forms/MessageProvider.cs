using System;
using System.Windows.Forms;

namespace Rift.Forms
{
    /// <summary>
    /// Represents the method that will handle a window message.
    /// </summary>
    /// <param name="m">A reference to the window message to handle.</param>
    public delegate void MessageHandler(ref Message m);

    /// <summary>
    /// Provides access to the window messages queue.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class MessageProvider : NativeWindow, IDisposable
    {
        /// <summary>
        /// Occurs when the next window message was received.
        /// </summary>
        public event MessageHandler MessageReceived;

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Forms.MessageProvider"/> class.
        /// </summary>
        public MessageProvider(MessageHandler handler)
        {
            MessageReceived = handler;
            CreateHandle(new CreateParams());
        }
        
        protected override void WndProc(ref Message m)
        {
            if (MessageReceived != null)
                MessageReceived(ref m);
            
            base.WndProc(ref m);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Rift.Forms.MessageProvider"/>.
        /// </summary>
        public void Dispose()
        {
            DestroyHandle();
        }
    }
}