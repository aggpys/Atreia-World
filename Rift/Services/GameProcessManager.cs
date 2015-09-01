using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Rift.Data;
using Rift.Utils;

namespace Rift.Services
{
    /// <summary>
    /// Represents the game client process manager.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    public sealed class GameProcessManager : IDisposable
    {
        /// <summary>
        /// Occurs when the game process exited.
        /// </summary>
        public event EventHandler<GameExitedEventArgs> ProcessExited;

        /// <summary>
        /// Specifies the invalid PID.
        /// </summary>
        public const int InvalidProcessId = -1;

        private readonly string binPath;
        private readonly Dictionary<int, Process> processes;
        private readonly Dictionary<int, GameAccount> accounts; 

        /// <summary>
        /// Gets the game process count.
        /// </summary>
        public int ProcessCount
        {
            get { return processes.Count; }
        }
        
        /// <summary>
        /// Gets a collection of the user accounts
        /// that currently in-use.
        /// </summary>
        public IEnumerable<GameAccount> ActiveAccounts
        {
            get { return accounts.Values; }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.GameProcessManager"/>.
        /// </summary>
        /// <param name="path">A string path to the game executable files.</param>
        public GameProcessManager(string path)
        {
            binPath = path;
            processes = new Dictionary<int, Process>();
            accounts = new Dictionary<int, GameAccount>();

            InitializeProcesses();
        }

        /// <summary>
        /// Starts the new game client process
        /// </summary>
        /// <param name="startParams">A game process start parameters.</param>
        /// <exception cref="System.IO.FileNotFoundException"/>
        public int StartClient(GameProcessParameters startParams)
        {
            var startInfo = new ProcessStartInfo(binPath)
            {
                //Arguments = startParams.ToString(),
                UseShellExecute = false
            };

            var process = Process.Start(startInfo);
            
            if (process == null)
                return InvalidProcessId;

            process.EnableRaisingEvents = true;
            process.Exited += OnProcessExited;
            processes.Add(process.Id, process);
            accounts.Add(process.Id, startParams.Account);
            
            return process.Id;
        }

        /// <summary>
        /// Terminates the game client process by the specified process identifier.
        /// </summary>
        /// <param name="pid">A process identifier (PID) to terminate.</param>
        public void TerminateClient(int pid)
        {
            if (!processes.ContainsKey(pid))
                return;

            var process = processes[pid];

            try
            {
                process.Kill();
            }
            finally
            {
                process.Exited -= OnProcessExited;
                process.Dispose();
                processes.Remove(pid);
                accounts.Remove(pid);
            }
        }

        /// <summary>
        /// Enumerates the game process identifiers (PID).
        /// </summary>
        public IEnumerable<int> EnumerateProcessId()
        {
            foreach (var key in processes.Keys)
                yield return key;
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Rift.Services.GameProcessManager"/>.
        /// </summary>
        public void Dispose()
        {
            foreach (var process in processes.Values)
            {
                process.Exited -= OnProcessExited;
                process.Dispose();
            }

            processes.Clear();
            accounts.Clear();
        }

        private void InitializeProcesses()
        {
            var currentProcesses = Process.GetProcesses();
            var executablePath = PathExtensions.Normalize(binPath);

            foreach (var process in currentProcesses)
            {
                try
                {
                    var temp = PathExtensions.Normalize(process.MainModule.FileName);

                    if (string.Equals(temp, executablePath, StringComparison.OrdinalIgnoreCase))
                    {
                        process.EnableRaisingEvents = true;
                        process.Exited += OnProcessExited;
                        processes.Add(process.Id, process);
                    }
                    else
                        process.Dispose(); // Releases all unused processes.
                }
                catch (Win32Exception) { } // Access denied situation.
                catch (InvalidOperationException) { } // Process already exited (old record).
            }
        }

        private void OnProcessExited(object sender, EventArgs e)
        {
            var indexes = new List<int>();

            foreach (var processInfo in processes)
            {
                if (processInfo.Value.HasExited)
                    indexes.Add(processInfo.Key);
            }

            foreach (var index in indexes)
            {
                processes[index].Exited -= OnProcessExited;
                processes[index].Dispose();
                processes.Remove(index);
                accounts.Remove(index);

                if (ProcessExited != null)
                    ProcessExited(this, new GameExitedEventArgs(index));
            }
        }
    }
}