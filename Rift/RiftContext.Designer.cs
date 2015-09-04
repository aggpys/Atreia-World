using System;
using System.Windows.Forms;
using Rift.Forms;
using Rift.Properties;
using Rift.Services;
using Rift.Utils;

namespace Rift
{
    // This part of RiftContext class defines
    // an application notify icon user interface.
    public sealed partial class RiftContext : ApplicationContext
    {
        private MessageProvider messageProvider;    // Handles the window messages (WM).

        private NotifyIcon trayApp;                 // Tray icon element.
        private ContextMenu trayMenu;               // Tray icon context menu.

        // Initializes an application user interface.
        private void InitializeContext()
        {
            messageProvider = new MessageProvider(HandleMessage);

            trayMenu = new ContextMenu();
            
            var menuItemNew = new MenuItem(Resources.TrayMenuStart, OnStartNew)
            {
                DefaultItem = true
            };

            var menuItemResources = new MenuItem(Resources.TrayMenuResources);

            menuItemResources.MenuItems.Add(Resources.TrayMenuWeb, OnWebSiteShow);
            menuItemResources.MenuItems.Add(Resources.TrayMenuForum, OnForumPageShow);
            menuItemResources.MenuItems.Add(Resources.TrayMenuSocial, OnSocialPageShow);
            menuItemResources.MenuItems.Add(Resources.TrayMenuDatabase, OnDatabaseShow);
            
            trayMenu.MenuItems.Add(menuItemNew);
            trayMenu.MenuItems.Add(Resources.TrayMenuExit, OnExit);
            trayMenu.MenuItems.Add(Resources.TrayMenuSeparator);
            trayMenu.MenuItems.Add(Resources.TrayMenuVote, OnVote);
            trayMenu.MenuItems.Add(Resources.TrayMenuScreenshots, OnViewScreenshots);
            trayMenu.MenuItems.Add(menuItemResources);
            
            trayApp = new NotifyIcon
            {
                Text = Resources.TrayAppName,
                Icon = NotifyTrayIcon.Application,
                ContextMenu = trayMenu,
                Visible = true
            };

            trayMenu.Popup += OnPopupMenu;
            trayApp.DoubleClick += OnStartNew;
            gameProcessManager.ProcessExited += OnProcessExited;

            if (gameProcessManager.ProcessCount > 0)
            {
                trayMenu.MenuItems.Add(Resources.TrayMenuSeparator);

                foreach (var pid in gameProcessManager.EnumerateProcessId())
                {
                    var menuItemProcess = new MenuItem(string.Format(Resources.TrayMenuKillFormat, pid), OnProcessKill)
                    {
                        Tag = pid
                    };

                    trayMenu.MenuItems.Add(menuItemProcess);
                }
            }
        }

        // Updates a tray icon context menu.
        private void OnPopupMenu(object sender, EventArgs e)
        {
            foreach (MenuItem item in trayMenu.MenuItems)
                if (item.DefaultItem)
                {
                    item.Enabled = !MainForm.Visible;
                }
        }

        // Starts a new game client process,
        // updates list of processes that was started.
        private void OnStartNew(object sender, EventArgs e)
        {
            var form = MainForm as RiftForm;

            if (form != null)
                form.ShowAtFront(); // Shows the main form over the other windows.
        }

        // Navigates to the project web-site root.
        private void OnWebSiteShow(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationWebUri);
        }

        // Navigates to the project forum.
        private void OnForumPageShow(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationForumUri);
        }

        // Navigates to the project group in social network.
        private void OnSocialPageShow(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationSocialUri);
        }

        // Navigates to the online database resource.
        private void OnDatabaseShow(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationDatabaseUri);
        }

        // Navigates to the game screenshots location. 
        private void OnViewScreenshots(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(gameClientManager.GetFolderPath(ClientSpecialFolder.Screenshot));
        }

        // Navigates to the vote page.
        private void OnVote(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationVoteUri);
        }

        // Kills the selected game client process,
        // updates list of processes that is running.
        private void OnProcessKill(object sender, EventArgs e)
        {
            var item = sender as MenuItem;

            if (item == null) return;

            var pid = item.Tag != null ? (int) item.Tag : 0;

            if (pid > 0)
            {
                trayMenu.MenuItems.Remove(item);
                gameProcessManager.TerminateClient(pid);

                if (gameProcessManager.ProcessCount == 0)
                    trayMenu.MenuItems.RemoveAt(trayMenu.MenuItems.Count - 1);
            }
        }

        // Updates list of processes that is running,
        // if one of these processes exited.
        private void OnProcessExited(object sender, GameExitedEventArgs e)
        {
            MenuItem foundItem = null;

            foreach (MenuItem item in trayMenu.MenuItems)
                if (item.Tag != null && (int) item.Tag == e.ProcessId)
                {
                    foundItem = item;
                    break;
                }

            if (foundItem == null) return;

            foundItem.Click -= OnProcessKill;
            trayMenu.MenuItems.Remove(foundItem);

            if (gameProcessManager.ProcessCount == 0)
                trayMenu.MenuItems.RemoveAt(trayMenu.MenuItems.Count - 1);
        }
        
        // Exits an application.
        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Handles a window message.
        private void HandleMessage(ref Message m)
        {
            if (m.Msg == Win32.WM_NCSTART)          // Custom window message WM_NCSTART
                OnStartNew(this, EventArgs.Empty);  // shows the main form if it hidden.
        }
        
        // Releases an all resources used by the context components.
        private void DisposeComponents()
        {
            if (messageProvider != null)
                messageProvider.Dispose();

            if (trayMenu != null)
            {
                trayApp.DoubleClick -= OnStartNew;
                trayApp.ContextMenu = null;
                trayMenu.Dispose();
            }

            if (trayApp != null)
                trayApp.Dispose();

            if (gameProcessManager != null)
                gameProcessManager.ProcessExited -= OnProcessExited;
        }
    }
}