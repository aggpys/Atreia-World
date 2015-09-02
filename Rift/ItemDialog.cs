using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Rift.Data;
using Rift.Forms;
using Rift.Properties;
using Rift.Utils;

namespace Rift
{
    /// <summary>
    /// Represents a buy item dialog that makes up a user interface.
    /// </summary>
    public partial class ItemDialog : RiftForm
    {
        private readonly ShopItem item;
        private readonly GameAccount account;
        private readonly Regex characterRegex;

        /// <summary>
        /// Gets the selected character.
        /// </summary>
        public string CharacterName {get { return textBoxCharacter.Text; } }

        /// <summary>
        /// Gets the items count to buy.
        /// </summary>
        public int Count { get { return comboBoxCount.SelectedIndex + 1; } }

        /// <summary>
        /// Creates a new instance of the <see cref="Rift.ItemDialog"/> class.
        /// </summary>
        public ItemDialog(ShopItem item, GameAccount account)
        {
            InitializeComponent();

            this.item = item;
            this.account = account;
            
            labelAccountValue.Text = account.Name;
            labelTitle.Text = item.Title;
            labelTitle.LinkColor = QualityColorHelper.GetForeColor(item.Quality);
            labelTitle.VisitedLinkColor = labelTitle.LinkColor;
            labelIdValue.Text = item.Identifier.ToString("D");
            labelCountBase.Text = string.Format("{0} ×", item.Count);
            labelPriceValue.Text = string.Format("{0}@", item.Price);
            
            characterRegex = new Regex(Resources.RegexCharacterName);
            
            App.CurrentContext.Cache.GetImageAsync(IconPathResolver.ExpandUri(item.IconUri), UpdateItemIcon);

            for (var i = 1; i <= 5; ++i)
                comboBoxCount.Items.Add(string.Format("{0}", i*item.Count));
            
            comboBoxCount.SelectedIndex = 0;
        }

        private void ValidateInput()
        {
            buttonBuy.Enabled = 
                string.Equals(textBoxPassword.Text, account.Password, StringComparison.Ordinal) &&
                !string.IsNullOrEmpty(textBoxCharacter.Text) &&
                !characterRegex.IsMatch(textBoxCharacter.Text);

            buttonBuy.ForeColor = buttonBuy.Enabled ? Color.Black : ForeColor;
        }

        private void UpdateItemIcon(Image icon)
        {
            pictureBoxIcon.BackgroundImage = icon;
        }

        private void UpdateTotalPriceLabel()
        {
            var total = (comboBoxCount.SelectedIndex + 1) * item.Price;
            labelTotalValue.Text = string.Format("{0}@", total);
        }

        private void comboBoxCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTotalPriceLabel();
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void textBoxCharacter_TextChanged(object sender, EventArgs e)
        {
            ValidateInput();
        }

        private void labelTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NavigationHelper.NavigateTo(string.Format(Resources.NavigationItemFormat, item.Identifier));
        }
    }
}
