using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Rift.Forms;
using Rift.Properties;
using Rift.Services;
using Rift.Utils;

namespace Rift
{
    /// <summary>
    /// Specifies the contextual information about an application thread.
    /// This class cannot be inherited.
    /// </summary>
    public sealed partial class RiftContext : IConfigurable
    {
        private readonly GameClientManager gameClientManager;   // Game client files, folders, checking.
        private readonly GameProcessManager gameProcessManager; // Start new, kill by PID, etc.
        private readonly AccountManager accountManager;         // Reads/writes an accounts from/to settings.
        private readonly ServerManager serverManager;           // Server status, files downloading.
        private readonly ShopManager shopManager;               // In-game shop data.

        private readonly ImageCache imageCache;                 // Image files caching.
        
        /// <summary>
        /// Returns a game client management object.
        /// </summary>
        public GameClientManager GameClientManager
        {
            get { return gameClientManager; }
        }

        /// <summary>
        /// Returns a game process management object.
        /// </summary>
        public GameProcessManager GameProcessManager
        {
            get { return gameProcessManager; }
        }

        /// <summary>
        /// Returns an account management object.
        /// </summary>
        public AccountManager AccountManager
        {
            get { return accountManager; }
        }

        /// <summary>
        /// Returns a server management object.
        /// </summary>
        public ServerManager ServerManager
        {
            get { return serverManager; }
        }

        /// <summary>
        /// Returns a shop management object.
        /// </summary>
        public ShopManager ShopManager
        {
            get { return shopManager; }
        }

        /// <summary>
        /// Returns an image files cache.
        /// </summary>
        public ImageCache Cache
        {
            get { return imageCache; }
        }

        /// <summary>
        /// Indicates whether an application context was successfully initialized.
        /// </summary>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.RiftContext"/> class.
        /// </summary>
        public RiftContext()
        {
            gameClientManager = new GameClientManager(Application.StartupPath);
            
#if DEBUG
            Initialized = true;
#else
            Initialized = gameClientManager.IsClientStructureValid();
#endif

            if (!Initialized) return;

            gameProcessManager = new GameProcessManager(gameClientManager.GetExecutablePath());
            accountManager = new AccountManager();
            serverManager = new ServerManager();
            shopManager = new ShopManager();
            imageCache = new ImageCache(Application.CommonAppDataPath); // This content is user-independent.

            MainForm = new MainForm();
            InitializeContext();
        }
        
        /// <summary>
        /// Writes an application settings section.
        /// </summary>
        public void WriteSettings()
        {
            if (Initialized)
                accountManager.WriteSettings();
        }

        /// <summary>
        /// Starts a new game client process with the specified parameters.
        /// </summary>
        /// <param name="parameters">A game client process start parameters.</param>
        public void StartGame(GameProcessParameters parameters)
        {
            if (!Initialized) return;

            var menuItemProcess = new MenuItem(string.Format(Resources.TrayMenuKillFormat, parameters.Account.Name), OnProcessKill)
            {
                Tag = gameProcessManager.StartClient(parameters)
            };

            if (gameProcessManager.ProcessCount == 1)
                trayMenu.MenuItems.Add(Resources.TrayMenuSeparator);

            trayMenu.MenuItems.Add(menuItemProcess);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeComponents(); // RiftContext.Designer.cs

                if (gameProcessManager != null)
                    gameProcessManager.Dispose();

                if (serverManager != null)
                    serverManager.Dispose();

                if (shopManager != null)
                    shopManager.Dispose();

                if (imageCache != null)
                    imageCache.Dispose();
            }

            base.Dispose(disposing);
        }
    }
    
    /// <summary>
    /// Represents errors that occur in the application context.
    /// <para>This class cannot be inherited.</para>
    /// </summary>
    [Serializable]
    public sealed class ContextException : Exception
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Rift.ContextException"/> class.
        /// </summary>
        public ContextException() { }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.ContextException"/> class
        /// with a specified error message.
        /// </summary>
        /// <param name="message">A error text message that describes the error.</param>
        public ContextException(string message) : base(message) { }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.ContextException"/> class
        /// with a specified error message and a reference to the inner exception
        /// that is cause of this exception.
        /// </summary>
        /// <param name="message">A error text message that describes the error.</param>
        /// <param name="innerException">The exception that is cause of the current exception.</param>
        public ContextException(string message, Exception innerException) : base(message, innerException) { }

        private ContextException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}