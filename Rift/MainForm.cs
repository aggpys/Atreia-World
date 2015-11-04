using System;
using System.Linq;
using System.Windows.Forms;
using Rift.Data;
using Rift.Forms;
using Rift.Properties;
using Rift.Services;
using Rift.Utils;

namespace Rift
{
    /// <summary>
    /// Represents the main application window that makes up a user interface.
    /// This class cannot be inherited.
    /// </summary>
    public sealed partial class MainForm : RiftForm
    {
        private GameShop shop;

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            shop = null;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateLoginForm();

            App.CurrentContext.ServerManager.StatusUpdated += ServerManager_StatusUpdated;
            App.CurrentContext.ShopManager.PriceListLoading += ShopManager_PriceListLoading;
            App.CurrentContext.ShopManager.PriceListReady += ShopManager_PriceListReady;

            App.CurrentContext.ShopManager.GetPriceListAsync();
        }
        
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!Visible) return;

            if (!string.IsNullOrEmpty(comboBoxName.Text) &&
                !string.IsNullOrEmpty(textBoxPassword.Text))
                actionButtonPlay.Focus();

            App.CurrentContext.ServerManager.UpdateStatusAsync();
        }

        private void UpdateStatus(ServerStatus status)
        {
            actionButtonPlay.Enabled = status == ServerStatus.Online;

            if (status == ServerStatus.Online) return;

            using (var dialog = new MessageDialog(MessageType.Warning, Resources.WarningConnectionFailed))
                dialog.ShowDialog(this);
        }

        private void UpdateShopButton(int value, bool enabled)
        {
            actionButtonShop.Enabled = enabled;
            actionButtonShop.ProgressValue = value;
        }

        private void DisposeComponents()
        {
            App.CurrentContext.ServerManager.StatusUpdated -= ServerManager_StatusUpdated;
            App.CurrentContext.ShopManager.PriceListLoading -= ShopManager_PriceListLoading;
            App.CurrentContext.ShopManager.PriceListReady -= ShopManager_PriceListReady;
        }

        private void UpdateLoginForm()
        {
            SuspendLayout();

            comboBoxName.Items.Clear();
            comboBoxName.AutoCompleteCustomSource.Clear();
            comboBoxName.Text = string.Empty;
            textBoxPassword.Text = string.Empty;

            if (!App.CurrentContext.AccountManager.HasAccounts)
                return;

            var selected = App.CurrentContext.AccountManager.SelectedAccount;

            foreach (var account in App.CurrentContext.AccountManager.Accounts)
            {
                comboBoxName.Items.Add(account);
                comboBoxName.AutoCompleteCustomSource.Add(account.Name);
            }

            comboBoxName.SelectedItem = selected;
            textBoxPassword.Text = selected.Password;

            ResumeLayout();
        }

        private void SaveUserInput()
        {
            if (comboBoxName.SelectedIndex < 0 &&
                !string.IsNullOrEmpty(comboBoxName.Text) &&
                !string.IsNullOrEmpty(textBoxPassword.Text))
            {
                var name = comboBoxName.Text.Trim();
                var password = textBoxPassword.Text.Trim();
                var account = new GameAccount(name, password);

                App.CurrentContext.AccountManager.AddAccount(account);
            }
        }

        private bool IsUserInputValid()
        {
            var validationStatus = true;

            if (comboBoxName.SelectedIndex < 0 &&
                (string.IsNullOrEmpty(comboBoxName.Text) || string.IsNullOrEmpty(textBoxPassword.Text)))
            {
                using (var dialog = new MessageDialog(MessageType.Warning, Resources.WarningLoginInfo))
                    dialog.ShowDialog();
                
                validationStatus = false;
            }
            else
            {
                var account = comboBoxName.SelectedIndex >= 0
                    ? comboBoxName.SelectedItem as GameAccount
                    : new GameAccount(comboBoxName.Text, textBoxPassword.Text);

                if (App.CurrentContext.GameProcessManager.ActiveAccounts.Contains(account))
                {
                    using (var dialog = new MessageDialog(MessageType.Warning, Resources.WarningEntered))
                        dialog.ShowDialog();
                    
                    validationStatus = false;
                }
            }

            if (validationStatus) return true;

            if (string.IsNullOrEmpty(comboBoxName.Text))
                comboBoxName.Focus();
            else if (string.IsNullOrEmpty(textBoxPassword.Text))
                textBoxPassword.Focus();

            return false;
        }

        #region Buttons
        
        private void spriteButtonWebPage_Action(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationWebUri);
        }

        private void spriteButtonForum_Action(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationForumUri);
        }

        private void spriteButtonSocial_Action(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationSocialUri);
        }

        private void spriteButtonVote_Action(object sender, EventArgs e)
        {
            NavigationHelper.NavigateTo(Resources.NavigationVoteUri);
        }

        private void spriteButtonDelete_Action(object sender, EventArgs e)
        {
            if (comboBoxName.SelectedIndex < 0)
            {
                comboBoxName.Text = string.Empty;
                textBoxPassword.Clear();

                return;
            }

            App.CurrentContext.AccountManager.RemoveSelected();
            UpdateLoginForm();
        }

        #endregion

        // Fixes the text selection for the password text box.
        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action) textBoxPassword.SelectAll);
        }

        // Handles the server status updates.
        // In most cases this method is called from the non-UI thread.
        private void ServerManager_StatusUpdated(object sender, ServerStatusEventArgs e)
        {
            this.InvokeAction(() => UpdateStatus(e.Status));
        }

        private void ShopManager_PriceListReady(object sender, PriceListReadyEventArgs e)
        {
            shop = e.PriceList;

            this.InvokeAction(() => UpdateShopButton(0, shop != null));
        }

        private void ShopManager_PriceListLoading(object sender, PriceListLoadingEventArgs e)
        {
            var ratio = (float) e.BytesReceived/e.TotalBytes;
            var value = Convert.ToInt32(Math.Round(ratio*100));

            if (value > 100)
                value = 100;

            this.InvokeAction(() => UpdateShopButton(value, false));
        }
        
        private void comboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = comboBoxName.SelectedIndex;

            if (index < 0)
                return;

            var account = App.CurrentContext.AccountManager.Select(index);
            textBoxPassword.Text = account.Password;
        }

        // Starts the another instance of the game process.
        private void actionButtonPlay_Action(object sender, EventArgs e)
        {
            if (!IsUserInputValid()) return;

            SaveUserInput();

            var args = new GameProcessParameters(
                Settings.Default.ServerAddress,
                Settings.Default.ServerPort,
                Settings.Default.GameCountryCode,
                App.CurrentContext.AccountManager.SelectedAccount);

            App.CurrentContext.StartGame(args);
            Close();
        }

        // Shows the shop form.
        private void actionButtonShop_Action(object sender, EventArgs e)
        {
            if (!IsUserInputValid()) return;

            SaveUserInput();

            using (var dialog = new ShopDialog(App.CurrentContext.AccountManager.SelectedAccount, shop))
            {
                dialog.ShowDialog();
            }
        }
    }
}
