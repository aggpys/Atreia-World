using System;
using System.Net;
using Rift.Properties;

namespace Rift.Services
{
    /// <summary>
    /// Specifies the server statuses.
    /// </summary>
    [Flags]
    public enum ServerStatus
    {
        /// <summary>
        /// Server is offline.
        /// </summary>
        Offline = 0,
        /// <summary>
        /// Game server is online.
        /// </summary>
        GameOnline = 2,
        /// <summary>
        /// Login server is online.
        /// </summary>
        LoginOnline = 1,
        /// <summary>
        /// Server is online.
        /// </summary>
        Online = GameOnline | LoginOnline
    }

    /// <summary>
    /// Provides data for the <see cref="Rift.Services.ServerManager.OnStatusUpdated"/>.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ServerStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a server status.
        /// </summary>
        public ServerStatus Status { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.ServerStatusEventArgs"/> class
        /// from the specified server status.
        /// </summary>
        /// <param name="status">A server status to set.</param>
        public ServerStatusEventArgs(ServerStatus status)
        {
            Status = status;
        }
    }

    /// <summary>
    /// Represents a server manager.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class ServerManager : IDisposable
    {
        /// <summary>
        /// Occurs when the server status is updated.
        /// </summary>
        public event EventHandler<ServerStatusEventArgs> StatusUpdated;
        
        private readonly WebClient client;

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.Services.ServerManager"/> class.
        /// </summary>
        public ServerManager()
        {
            client = new WebClient();
            client.DownloadStringCompleted += client_DownloadStringCompleted;
        }

        private void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            int status;

            if (e.Error != null)
            {
                client.CancelAsync();
                OnStatusUpdated(ServerStatus.Offline);
                return;
            }

            var temp = int.TryParse(e.Result, out status);

            if (temp && status >= 0 && status <= 3)
                OnStatusUpdated((ServerStatus)status);
            else
                OnStatusUpdated(ServerStatus.Offline);
        }

        /// <summary>
        /// Updates the server status asynchronously. An <see cref="Rift.Services.ServerManager.OnStatusUpdated"/> event
        /// will be called.
        /// </summary>
        public void UpdateStatusAsync()
        {
            try
            {
                client.DownloadStringAsync(new Uri(Resources.NavigationServerStatusUri));
            }
            catch (WebException)
            {
                OnStatusUpdated(ServerStatus.Offline);
            }
        }
        
        private void OnStatusUpdated(ServerStatus status)
        {
            if (StatusUpdated != null)
                StatusUpdated(this, new ServerStatusEventArgs(status));
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Rift.Services.ServerManager"/>.
        /// </summary>
        public void Dispose()
        {
            if (client != null)
            {
                client.DownloadStringCompleted -= client_DownloadStringCompleted;

                if (client.IsBusy)
                    client.CancelAsync();
                
                client.Dispose();
            }
        }
    }
}