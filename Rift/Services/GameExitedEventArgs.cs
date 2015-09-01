using System;

namespace Rift.Services
{
    /// <summary>
    /// Provides data for the <see cref="Rift.Services.GameProcessManager.ProcessExited"/> event.
    /// This class cannot be inherited.
    /// </summary>
    public class GameExitedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the exited game process identifier (PID).
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rift.Services.GameExitedEventArgs"/> class
        /// from the specified game process identifier (PID).
        /// </summary>
        /// <param name="pid">An exited game process identifier (PID).</param>
        public GameExitedEventArgs(int pid)
        {
            ProcessId = pid;
        }
    }
}