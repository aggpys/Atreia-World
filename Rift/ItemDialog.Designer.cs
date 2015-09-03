namespace Rift
{
    partial class ItemDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemDialog));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonBuy = new System.Windows.Forms.Button();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.labelId = new System.Windows.Forms.Label();
            this.labelIdValue = new System.Windows.Forms.Label();
            this.labelPrice = new System.Windows.Forms.Label();
            this.labelPriceValue = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.panelUpDown = new Rift.Forms.FieldPanel();
            this.comboBoxCount = new System.Windows.Forms.ComboBox();
            this.labelSeparator = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.labelTotalValue = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelAccountValue = new System.Windows.Forms.Label();
            this.panelCharacter = new Rift.Forms.FieldPanel();
            this.textBoxCharacter = new System.Windows.Forms.TextBox();
            this.labelCharacter = new System.Windows.Forms.Label();
            this.panelConfirm = new Rift.Forms.FieldPanel();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelCountBase = new System.Windows.Forms.Label();
            this.labelTitle = new Rift.Forms.WebLabel();
            this.labelAbout = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.panelUpDown.SuspendLayout();
            this.panelCharacter.SuspendLayout();
            this.panelConfirm.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.buttonCancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSlateGray;
            this.buttonCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
            this.buttonCancel.ForeColor = System.Drawing.Color.DimGray;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonBuy
            // 
            resources.ApplyResources(this.buttonBuy, "buttonBuy");
            this.buttonBuy.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonBuy.FlatAppearance.BorderColor = System.Drawing.Color.LightSkyBlue;
            this.buttonBuy.FlatAppearance.MouseDownBackColor = System.Drawing.Color.LightSlateGray;
            this.buttonBuy.FlatAppearance.MouseOverBackColor = System.Drawing.Color.SkyBlue;
            this.buttonBuy.ForeColor = System.Drawing.Color.Gainsboro;
            this.buttonBuy.Name = "buttonBuy";
            this.buttonBuy.UseVisualStyleBackColor = true;
            // 
            // pictureBoxIcon
            // 
            resources.ApplyResources(this.pictureBoxIcon, "pictureBoxIcon");
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.TabStop = false;
            // 
            // labelId
            // 
            resources.ApplyResources(this.labelId, "labelId");
            this.labelId.Name = "labelId";
            // 
            // labelIdValue
            // 
            resources.ApplyResources(this.labelIdValue, "labelIdValue");
            this.labelIdValue.ForeColor = System.Drawing.Color.DarkGray;
            this.labelIdValue.Name = "labelIdValue";
            // 
            // labelPrice
            // 
            resources.ApplyResources(this.labelPrice, "labelPrice");
            this.labelPrice.Name = "labelPrice";
            // 
            // labelPriceValue
            // 
            resources.ApplyResources(this.labelPriceValue, "labelPriceValue");
            this.labelPriceValue.ForeColor = System.Drawing.Color.DodgerBlue;
            this.labelPriceValue.Name = "labelPriceValue";
            // 
            // labelCount
            // 
            resources.ApplyResources(this.labelCount, "labelCount");
            this.labelCount.Name = "labelCount";
            // 
            // panelUpDown
            // 
            this.panelUpDown.BorderActiveColor = System.Drawing.Color.LightSkyBlue;
            this.panelUpDown.BorderColor = System.Drawing.Color.DarkGray;
            this.panelUpDown.Controls.Add(this.comboBoxCount);
            resources.ApplyResources(this.panelUpDown, "panelUpDown");
            this.panelUpDown.Name = "panelUpDown";
            // 
            // comboBoxCount
            // 
            this.comboBoxCount.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.comboBoxCount, "comboBoxCount");
            this.comboBoxCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCount.FormattingEnabled = true;
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.SelectedIndexChanged += new System.EventHandler(this.comboBoxCount_SelectedIndexChanged);
            // 
            // labelSeparator
            // 
            resources.ApplyResources(this.labelSeparator, "labelSeparator");
            this.labelSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparator.Name = "labelSeparator";
            // 
            // labelTotal
            // 
            resources.ApplyResources(this.labelTotal, "labelTotal");
            this.labelTotal.Name = "labelTotal";
            // 
            // labelTotalValue
            // 
            resources.ApplyResources(this.labelTotalValue, "labelTotalValue");
            this.labelTotalValue.ForeColor = System.Drawing.Color.Firebrick;
            this.labelTotalValue.Name = "labelTotalValue";
            // 
            // labelName
            // 
            resources.ApplyResources(this.labelName, "labelName");
            this.labelName.Name = "labelName";
            // 
            // labelAccountValue
            // 
            resources.ApplyResources(this.labelAccountValue, "labelAccountValue");
            this.labelAccountValue.ForeColor = System.Drawing.Color.Black;
            this.labelAccountValue.Name = "labelAccountValue";
            // 
            // panelCharacter
            // 
            this.panelCharacter.BackColor = System.Drawing.Color.White;
            this.panelCharacter.BorderActiveColor = System.Drawing.Color.LightSkyBlue;
            this.panelCharacter.BorderColor = System.Drawing.Color.DarkGray;
            this.panelCharacter.Controls.Add(this.textBoxCharacter);
            resources.ApplyResources(this.panelCharacter, "panelCharacter");
            this.panelCharacter.Name = "panelCharacter";
            // 
            // textBoxCharacter
            // 
            this.textBoxCharacter.BackColor = System.Drawing.Color.White;
            this.textBoxCharacter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxCharacter, "textBoxCharacter");
            this.textBoxCharacter.Name = "textBoxCharacter";
            this.textBoxCharacter.TextChanged += new System.EventHandler(this.textBoxCharacter_TextChanged);
            // 
            // labelCharacter
            // 
            resources.ApplyResources(this.labelCharacter, "labelCharacter");
            this.labelCharacter.Name = "labelCharacter";
            // 
            // panelConfirm
            // 
            this.panelConfirm.BackColor = System.Drawing.Color.White;
            this.panelConfirm.BorderActiveColor = System.Drawing.Color.LightSkyBlue;
            this.panelConfirm.BorderColor = System.Drawing.Color.DarkGray;
            this.panelConfirm.Controls.Add(this.textBoxPassword);
            resources.ApplyResources(this.panelConfirm, "panelConfirm");
            this.panelConfirm.Name = "panelConfirm";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.BackColor = System.Drawing.Color.White;
            this.textBoxPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxPassword, "textBoxPassword");
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.UseSystemPasswordChar = true;
            this.textBoxPassword.TextChanged += new System.EventHandler(this.textBoxPassword_TextChanged);
            // 
            // labelPassword
            // 
            resources.ApplyResources(this.labelPassword, "labelPassword");
            this.labelPassword.Name = "labelPassword";
            // 
            // labelCountBase
            // 
            resources.ApplyResources(this.labelCountBase, "labelCountBase");
            this.labelCountBase.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.labelCountBase.Name = "labelCountBase";
            // 
            // labelTitle
            // 
            this.labelTitle.ActiveLinkColor = System.Drawing.Color.Gray;
            this.labelTitle.AutoEllipsis = true;
            this.labelTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            resources.ApplyResources(this.labelTitle, "labelTitle");
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labelTitle_LinkClicked);
            // 
            // labelAbout
            // 
            this.labelAbout.ForeColor = System.Drawing.Color.LightGray;
            resources.ApplyResources(this.labelAbout, "labelAbout");
            this.labelAbout.Name = "labelAbout";
            // 
            // ItemDialog
            // 
            this.AcceptButton = this.buttonBuy;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.BorderColorInactive = System.Drawing.Color.LightSkyBlue;
            this.CancelButton = this.buttonCancel;
            this.CaptionEnabled = true;
            this.CaptionTextColorActive = System.Drawing.Color.White;
            this.CaptionTextColorInactive = System.Drawing.Color.WhiteSmoke;
            this.ControlBox = true;
            this.Controls.Add(this.labelAbout);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelCountBase);
            this.Controls.Add(this.panelConfirm);
            this.Controls.Add(this.panelCharacter);
            this.Controls.Add(this.labelAccountValue);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelCharacter);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.labelTotalValue);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.labelSeparator);
            this.Controls.Add(this.panelUpDown);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonBuy);
            this.Controls.Add(this.labelPrice);
            this.Controls.Add(this.labelPriceValue);
            this.Controls.Add(this.labelIdValue);
            this.Controls.Add(this.pictureBoxIcon);
            this.Controls.Add(this.labelCount);
            this.Controls.Add(this.labelId);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.Name = "ItemDialog";
            this.Controls.SetChildIndex(this.labelId, 0);
            this.Controls.SetChildIndex(this.labelCount, 0);
            this.Controls.SetChildIndex(this.pictureBoxIcon, 0);
            this.Controls.SetChildIndex(this.labelIdValue, 0);
            this.Controls.SetChildIndex(this.labelPriceValue, 0);
            this.Controls.SetChildIndex(this.labelPrice, 0);
            this.Controls.SetChildIndex(this.buttonBuy, 0);
            this.Controls.SetChildIndex(this.buttonCancel, 0);
            this.Controls.SetChildIndex(this.panelUpDown, 0);
            this.Controls.SetChildIndex(this.labelSeparator, 0);
            this.Controls.SetChildIndex(this.labelTotal, 0);
            this.Controls.SetChildIndex(this.labelTotalValue, 0);
            this.Controls.SetChildIndex(this.labelName, 0);
            this.Controls.SetChildIndex(this.labelCharacter, 0);
            this.Controls.SetChildIndex(this.labelPassword, 0);
            this.Controls.SetChildIndex(this.labelAccountValue, 0);
            this.Controls.SetChildIndex(this.panelCharacter, 0);
            this.Controls.SetChildIndex(this.panelConfirm, 0);
            this.Controls.SetChildIndex(this.labelCountBase, 0);
            this.Controls.SetChildIndex(this.labelTitle, 0);
            this.Controls.SetChildIndex(this.labelAbout, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.panelUpDown.ResumeLayout(false);
            this.panelCharacter.ResumeLayout(false);
            this.panelCharacter.PerformLayout();
            this.panelConfirm.ResumeLayout(false);
            this.panelConfirm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonBuy;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Label labelIdValue;
        private System.Windows.Forms.Label labelPrice;
        private System.Windows.Forms.Label labelPriceValue;
        private System.Windows.Forms.Label labelCount;
        private Forms.FieldPanel panelUpDown;
        private System.Windows.Forms.ComboBox comboBoxCount;
        private System.Windows.Forms.Label labelSeparator;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label labelTotalValue;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelAccountValue;
        private Forms.FieldPanel panelCharacter;
        private System.Windows.Forms.TextBox textBoxCharacter;
        private System.Windows.Forms.Label labelCharacter;
        private Forms.FieldPanel panelConfirm;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelCountBase;
        private Forms.WebLabel labelTitle;
        private System.Windows.Forms.Label labelAbout;
    }
}