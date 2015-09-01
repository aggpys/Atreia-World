using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Rift.Properties;
using Rift.Utils;

namespace Rift
{
    /// <summary>
    /// Represents an application. Defines it's entry point.
    /// </summary>
    public static class App
    {
        public static RiftContext CurrentContext { get; private set; }

        private static readonly Mutex instanceSync; // Single-instance application pattern.
        private static readonly bool syncOwned;

        private static bool restart;

        static App()
        {
            var assembly = Assembly.GetEntryAssembly();
            var id = string.Concat(@"Global\", assembly.GetType().GUID.ToString("D"));

            instanceSync = new Mutex(true, id, out syncOwned);
            restart = false;
        }

        private static void HandleException(object e)
        {
            var message = Resources.ExceptionUnknown;

            var exception = e as Exception;

            if (exception != null)
            {
                var temp = exception;
                message = string.IsNullOrEmpty(temp.Message) ? e.ToString() : temp.Message;
            }
            else if (e != null)
                message = string.Format(Resources.NotExceptionFormat, e.GetType());
            
            using (var dialog = new MessageDialog(MessageType.Error, message))
            {
                dialog.Icon = Resources.RiftIcon; // Taskbar icon fix.

                var result = dialog.ShowDialog();
                restart = result == DialogResult.Retry;
            }
        }

        // The main entry point.
        [STAThread] static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException += (o, e) => HandleException(e.ExceptionObject);
            Application.ThreadException += (o, e) => HandleException(e.Exception);

            try
            {
                if (syncOwned) // If this is a first executed instance of an application.
                {
                    CurrentContext = new RiftContext();
                    
                    if (CurrentContext.Initialized)
                        Application.Run(CurrentContext);
                    else
                        throw new ContextException(Resources.ExceptionBadClient);
                }
                else
                {
                    // Single-instance application only: broadcast WM_NCSTART custom window message.
                    // This action shows (brings to front) the hidden main form of the first executed instance.
                    Win32.SendMessage(Win32.HWND_BROADCAST, Win32.WM_NCSTART, 0, 0);
                }
            }
            catch (Exception e) // Handles the unexpected exceptions.
            {
                HandleException(e);
            }
            finally
            {
                if (CurrentContext != null)
                {
                    CurrentContext.WriteSettings();
                    CurrentContext.Dispose();
                }

                Settings.Default.Save();

                if (syncOwned && instanceSync != null)
                {
                    instanceSync.ReleaseMutex();
                    instanceSync.Dispose();
                }

                if (restart)
                    Process.Start(Application.ExecutablePath);
            }
        }
    }
}