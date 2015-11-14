using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Rift.Data;
using Rift.Forms;
using Rift.Properties;
using Rift.Services;
using Rift.Utils;

namespace Rift
{
    /// <summary>
    /// Represents an in-game shop window that makes up a user interface.
    /// This class cannot be inherited.
    /// </summary>
    public partial class ShopDialog : RiftForm
    {
        private readonly GameShop shop;
        private readonly GameAccount account;

        private readonly Dictionary<int, ShopItemPanel> itemPanels;
        private readonly Queue<string> characters;

        private volatile bool onLoading;
        private volatile bool updateEnabled;

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.ShopDialog"/> class
        /// from the specified user account and shop items data
        /// </summary>
        /// <param name="account">A user account data.</param>
        /// <param name="shop">An in-game shop items data.</param>
        public ShopDialog(GameAccount account, GameShop shop)
        {
            InitializeComponent();

            this.account = account;
            this.shop = shop;

            onLoading = false;
            updateEnabled = true;

            characters = new Queue<string>();
            itemPanels = new Dictionary<int, ShopItemPanel>();
            comboBoxCategory.Items.Add(Resources.ShopCategorySearch);
            
            if (account != null)
                labelUser.Text = account.Name;
            else
            {
                labelUser.Visible = false;
                labelPoints.Visible = false;
            }
            
            if (shop == null) return;

            foreach (var category in shop.CategoryNames)
                comboBoxCategory.Items.Add(category);

            flowPanel.ChildCount = shop.ItemsCount;
            comboBoxCategory.SelectedIndex = comboBoxCategory.Items.Count > 1 ? 1 : 0;
        }
        
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            App.CurrentContext.ShopManager.ItemBought += ShopManager_ItemBought;
            App.CurrentContext.ShopManager.PointsReceived += ShopManager_PointsReceived;
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = onLoading;

            base.OnClosing(e);
        }

        private void DisposeComponents()
        {
            App.CurrentContext.ShopManager.ItemBought -= ShopManager_ItemBought;
            App.CurrentContext.ShopManager.PointsReceived -= ShopManager_PointsReceived;

            flowPanel.SuspendLayout();
            flowPanel.Controls.Clear();
            flowPanel.ResumeLayout();

            foreach (var control in itemPanels.Values)
            {
                control.Action -= itemPanel_Action;
                control.Dispose();
            }
            
            if (contentWorker.WorkerSupportsCancellation ||
                contentWorker.IsBusy)
                contentWorker.CancelAsync();

            contentWorker.Dispose();
            itemPanels.Clear();
        }

        private void UpdatePoints(int p)
        {
            panelTop.SuspendLayout();

            if (p < 0)
                using (var dialog = new MessageDialog(MessageType.Warning, Resources.WarningUpdatePointsCount))
                    dialog.ShowDialog(this);
            else
                labelPoints.Text = string.Format("{0}@", p);
            
            panelTop.ResumeLayout();
            contentWorker.RunWorkerAsync();
        }

        private void HandleItemOperation(ItemBoughtResult result)
        {
            if ((result & ItemBoughtResult.Failure) == ItemBoughtResult.Failure)
            {
                var temp = new StringBuilder();

                if ((result & ItemBoughtResult.IdMismatch) == ItemBoughtResult.IdMismatch)
                    temp.Append(Resources.ItemBoughtResultId).Append(Environment.NewLine);

                if ((result & ItemBoughtResult.CharacterMismatch) == ItemBoughtResult.CharacterMismatch)
                    temp.Append(Resources.ItemBoughtResultCharacter).Append(Environment.NewLine);

                if ((result & ItemBoughtResult.ServiceUnavailable) == ItemBoughtResult.ServiceUnavailable)
                    temp.Append(Resources.ItemBoughtResultService).Append(Environment.NewLine);

                if ((result & ItemBoughtResult.NotEnoughPoints) == ItemBoughtResult.NotEnoughPoints)
                    temp.Append(Resources.ItemBoughtResultPoints).Append(Environment.NewLine);

                var message = string.Format(Resources.WarningItemBoughtFailureFormat, temp);

                using (var dialog = new MessageDialog(MessageType.Warning, message))
                    dialog.ShowDialog(this);

                return;
            }

            var successMessage = string.Format(Resources.MessageItemBoughtSuccessFormat, characters.Dequeue());

            using (var dialog = new MessageDialog(MessageType.Info, successMessage))
                dialog.ShowDialog(this);

            if (account != null)
                App.CurrentContext.ShopManager.UpdatePointsAsync(account);
        }

        private void ShopManager_ItemBought(object sender, ItemBoughtEventArgs e)
        {
            this.InvokeAction(() => HandleItemOperation(e.Result));
        }

        private void ShopManager_PointsReceived(object sender, PointsReceivedEventArgs e)
        {
            this.InvokeAction(() => UpdatePoints(e.Points));
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (itemPanels.Count == 0) return;
            if (comboBoxCategory.SelectedIndex == 0) return;

            flowPanel.SuspendLayout();

            foreach (var control in itemPanels.Values)
                control.Visible = false;

            if (shop != null)
                foreach (var item in shop[(string) comboBoxCategory.SelectedItem])
                {
                    var code = item.GetHashCode();
                    
                    if (itemPanels.ContainsKey(code))
                        itemPanels[code].Visible = true;
                }

            flowPanel.ResumeLayout();

            flowPanel.Focus();
        }

        private void itemPanel_Action(object sender, EventArgs e)
        {
            var itemPanel = sender as ShopItemPanel;
            if (itemPanel == null) return;

            var item = itemPanel.ContainedItem;

            using (var dialog = new ItemDialog(item, account))
            {
                var result = dialog.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    var count = dialog.Count;
                    var character = dialog.CharacterName;
                    characters.Enqueue(character);
                    App.CurrentContext.ShopManager.BuyItemAsync(account, item, character, count);
                }
            }
        }

        private void contentWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            onLoading = true;
            this.InvokeAction(() => EnableControlBox(false));

            foreach (var category in shop)
            {
                var visible = false;
                this.InvokeAction(() => { visible = category.Name.Equals(comboBoxCategory.SelectedItem); });

                foreach (var item in category)
                    this.InvokeAction(() => AddItemPanel(item, visible));
            }

            onLoading = false;
            this.InvokeAction((() => EnableControlBox(true)));
        }

        private void AddItemPanel(ShopItem item, bool visible)
        {
            var control = new ShopItemPanel(item)
            {
                Visible = visible
            };
            
            control.Action += itemPanel_Action;
            itemPanels.Add(item.GetHashCode(), control);
            flowPanel.SuspendLayout();
            flowPanel.Controls.Add(control);
            flowPanel.ResumeLayout();
        }
        
        private void timerSearch_Tick(object sender, EventArgs e)
        {
            var searchSubstring = textBoxSearch.Text.ToLower();
            comboBoxCategory.SelectedIndex = 0;
            
            flowPanel.SuspendLayout();
            
            foreach (var item in shop.Items)
            {
                var title = item.Title.ToLower();
                var hash = item.GetHashCode();

                if (itemPanels.ContainsKey(hash))
                    itemPanels[item.GetHashCode()].Visible = title.Contains(searchSubstring);
            }

            flowPanel.ResumeLayout();

            timerSearch.Stop();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (updateEnabled &&
                account != null)
            {
                updateEnabled = false;
                App.CurrentContext.ShopManager.UpdatePointsAsync(account);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxSearch.Text)) return;
            if (textBoxSearch.Text.Length < 3) return;

            timerSearch.Start();
        }
    }
}
