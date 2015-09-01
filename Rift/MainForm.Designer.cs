namespace Rift
{
    partial class MainForm
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

            if (disposing)
                DisposeComponents();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.spriteButtonWebPage = new Rift.Forms.SpriteButton();
            this.spriteButtonForum = new Rift.Forms.SpriteButton();
            this.spriteButtonSocial = new Rift.Forms.SpriteButton();
            this.spriteButtonVote = new Rift.Forms.SpriteButton();
            this.actionButtonShop = new Rift.Forms.ActionButton();
            this.spriteButtonDelete = new Rift.Forms.SpriteButton();
            this.panelName = new Rift.Forms.FieldPanel();
            this.comboBoxName = new System.Windows.Forms.ComboBox();
            this.panelPassword = new Rift.Forms.FieldPanel();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.actionButtonPlay = new Rift.Forms.ActionButton();
            this.panelName.SuspendLayout();
            this.panelPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // spriteButtonWebPage
            // 
            this.spriteButtonWebPage.BackColor = System.Drawing.Color.Transparent;
            this.spriteButtonWebPage.BackgroundImage = global::Rift.Properties.Resources.ImageSpriteWebPage;
            this.spriteButtonWebPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.spriteButtonWebPage.Location = new System.Drawing.Point(357, 201);
            this.spriteButtonWebPage.MinimumSize = new System.Drawing.Size(8, 8);
            this.spriteButtonWebPage.Name = "spriteButtonWebPage";
            this.spriteButtonWebPage.Size = new System.Drawing.Size(32, 32);
            this.spriteButtonWebPage.TabIndex = 6;
            this.toolTip.SetToolTip(this.spriteButtonWebPage, "Web-страница проекта");
            this.spriteButtonWebPage.Action += new System.EventHandler(this.spriteButtonWebPage_Action);
            // 
            // spriteButtonForum
            // 
            this.spriteButtonForum.BackColor = System.Drawing.Color.Transparent;
            this.spriteButtonForum.BackgroundImage = global::Rift.Properties.Resources.ImageSpriteForum;
            this.spriteButtonForum.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.spriteButtonForum.Location = new System.Drawing.Point(395, 201);
            this.spriteButtonForum.MinimumSize = new System.Drawing.Size(8, 8);
            this.spriteButtonForum.Name = "spriteButtonForum";
            this.spriteButtonForum.Size = new System.Drawing.Size(32, 32);
            this.spriteButtonForum.TabIndex = 7;
            this.toolTip.SetToolTip(this.spriteButtonForum, "Форум");
            this.spriteButtonForum.Action += new System.EventHandler(this.spriteButtonForum_Action);
            // 
            // spriteButtonSocial
            // 
            this.spriteButtonSocial.BackColor = System.Drawing.Color.Transparent;
            this.spriteButtonSocial.BackgroundImage = global::Rift.Properties.Resources.ImageSpriteSocial;
            this.spriteButtonSocial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.spriteButtonSocial.Location = new System.Drawing.Point(433, 201);
            this.spriteButtonSocial.MinimumSize = new System.Drawing.Size(8, 8);
            this.spriteButtonSocial.Name = "spriteButtonSocial";
            this.spriteButtonSocial.Size = new System.Drawing.Size(32, 32);
            this.spriteButtonSocial.TabIndex = 8;
            this.toolTip.SetToolTip(this.spriteButtonSocial, "Группа ВКонтакте");
            this.spriteButtonSocial.Action += new System.EventHandler(this.spriteButtonSocial_Action);
            // 
            // spriteButtonVote
            // 
            this.spriteButtonVote.BackColor = System.Drawing.Color.Transparent;
            this.spriteButtonVote.BackgroundImage = global::Rift.Properties.Resources.ImageSpriteVote;
            this.spriteButtonVote.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.spriteButtonVote.Location = new System.Drawing.Point(471, 201);
            this.spriteButtonVote.MinimumSize = new System.Drawing.Size(8, 8);
            this.spriteButtonVote.Name = "spriteButtonVote";
            this.spriteButtonVote.Size = new System.Drawing.Size(32, 32);
            this.spriteButtonVote.TabIndex = 9;
            this.toolTip.SetToolTip(this.spriteButtonVote, "Голосовать за проект");
            this.spriteButtonVote.Action += new System.EventHandler(this.spriteButtonVote_Action);
            // 
            // actionButtonShop
            // 
            this.actionButtonShop.BackColor = System.Drawing.Color.Transparent;
            this.actionButtonShop.BackgroundImage = global::Rift.Properties.Resources.ImageSpriteShop;
            this.actionButtonShop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.actionButtonShop.BorderActiveColor = System.Drawing.Color.DimGray;
            this.actionButtonShop.BorderColor = System.Drawing.Color.Gray;
            this.actionButtonShop.Enabled = false;
            this.actionButtonShop.Location = new System.Drawing.Point(112, 193);
            this.actionButtonShop.Name = "actionButtonShop";
            this.actionButtonShop.ProgressColor = System.Drawing.Color.Azure;
            this.actionButtonShop.Size = new System.Drawing.Size(48, 48);
            this.actionButtonShop.TabIndex = 2;
            this.actionButtonShop.Text = null;
            this.toolTip.SetToolTip(this.actionButtonShop, "Игровой магазин");
            this.actionButtonShop.Action += new System.EventHandler(this.actionButtonShop_Action);
            // 
            // spriteButtonDelete
            // 
            this.spriteButtonDelete.BackColor = System.Drawing.Color.Transparent;
            this.spriteButtonDelete.BackgroundImage = global::Rift.Properties.Resources.ImageSpriteDelete;
            this.spriteButtonDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.spriteButtonDelete.Location = new System.Drawing.Point(137, 123);
            this.spriteButtonDelete.MinimumSize = new System.Drawing.Size(8, 8);
            this.spriteButtonDelete.Name = "spriteButtonDelete";
            this.spriteButtonDelete.Size = new System.Drawing.Size(23, 28);
            this.spriteButtonDelete.TabIndex = 10;
            this.spriteButtonDelete.TabStop = false;
            this.toolTip.SetToolTip(this.spriteButtonDelete, "Забыть выбранный аккаунт");
            this.spriteButtonDelete.Action += new System.EventHandler(this.spriteButtonDelete_Action);
            // 
            // panelName
            // 
            this.panelName.BackColor = System.Drawing.Color.Gray;
            this.panelName.BorderActiveColor = System.Drawing.Color.DodgerBlue;
            this.panelName.BorderColor = System.Drawing.Color.DimGray;
            this.panelName.Controls.Add(this.comboBoxName);
            this.panelName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.panelName.Location = new System.Drawing.Point(12, 123);
            this.panelName.Name = "panelName";
            this.panelName.Padding = new System.Windows.Forms.Padding(1);
            this.panelName.Size = new System.Drawing.Size(120, 28);
            this.panelName.TabIndex = 3;
            // 
            // comboBoxName
            // 
            this.comboBoxName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.comboBoxName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.comboBoxName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxName.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBoxName.ForeColor = System.Drawing.Color.DimGray;
            this.comboBoxName.FormattingEnabled = true;
            this.comboBoxName.Location = new System.Drawing.Point(1, 1);
            this.comboBoxName.Name = "comboBoxName";
            this.comboBoxName.Size = new System.Drawing.Size(118, 26);
            this.comboBoxName.TabIndex = 0;
            this.comboBoxName.SelectedIndexChanged += new System.EventHandler(this.comboBoxName_SelectedIndexChanged);
            // 
            // panelPassword
            // 
            this.panelPassword.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelPassword.BorderActiveColor = System.Drawing.Color.DodgerBlue;
            this.panelPassword.BorderColor = System.Drawing.Color.DimGray;
            this.panelPassword.Controls.Add(this.textBoxPassword);
            this.panelPassword.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.panelPassword.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelPassword.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.panelPassword.Location = new System.Drawing.Point(12, 159);
            this.panelPassword.Name = "panelPassword";
            this.panelPassword.Padding = new System.Windows.Forms.Padding(4, 4, 1, 2);
            this.panelPassword.Size = new System.Drawing.Size(148, 28);
            this.panelPassword.TabIndex = 4;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPassword.ForeColor = System.Drawing.Color.DimGray;
            this.textBoxPassword.Location = new System.Drawing.Point(4, 4);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(143, 19);
            this.textBoxPassword.TabIndex = 0;
            this.textBoxPassword.UseSystemPasswordChar = true;
            this.textBoxPassword.Enter += new System.EventHandler(this.textBoxPassword_Enter);
            // 
            // actionButtonPlay
            // 
            this.actionButtonPlay.BackColor = System.Drawing.Color.Transparent;
            this.actionButtonPlay.BackgroundImage = global::Rift.Properties.Resources.ImageSpritePlay;
            this.actionButtonPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.actionButtonPlay.Enabled = false;
            this.actionButtonPlay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.actionButtonPlay.ForeColor = System.Drawing.Color.White;
            this.actionButtonPlay.Location = new System.Drawing.Point(12, 193);
            this.actionButtonPlay.Name = "actionButtonPlay";
            this.actionButtonPlay.ProgressColor = System.Drawing.Color.Coral;
            this.actionButtonPlay.Size = new System.Drawing.Size(94, 48);
            this.actionButtonPlay.SpriteOrientation = Rift.Forms.SpriteOrientation.Vertical;
            this.actionButtonPlay.TabIndex = 1;
            this.actionButtonPlay.Text = "ИГРАТЬ";
            this.actionButtonPlay.Action += new System.EventHandler(this.actionButtonPlay_Action);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Rift.Properties.Resources.ImageBackgroundL0;
            this.BorderColorActive = System.Drawing.Color.DeepSkyBlue;
            this.BorderColorInactive = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(523, 253);
            this.ClosingEnabled = false;
            this.ControlBox = true;
            this.Controls.Add(this.actionButtonShop);
            this.Controls.Add(this.actionButtonPlay);
            this.Controls.Add(this.panelPassword);
            this.Controls.Add(this.panelName);
            this.Controls.Add(this.spriteButtonDelete);
            this.Controls.Add(this.spriteButtonVote);
            this.Controls.Add(this.spriteButtonSocial);
            this.Controls.Add(this.spriteButtonWebPage);
            this.Controls.Add(this.spriteButtonForum);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.MinimizeBox = true;
            this.Name = "MainForm";
            this.Text = "Atreia World";
            this.Controls.SetChildIndex(this.spriteButtonForum, 0);
            this.Controls.SetChildIndex(this.spriteButtonWebPage, 0);
            this.Controls.SetChildIndex(this.spriteButtonSocial, 0);
            this.Controls.SetChildIndex(this.spriteButtonVote, 0);
            this.Controls.SetChildIndex(this.spriteButtonDelete, 0);
            this.Controls.SetChildIndex(this.panelName, 0);
            this.Controls.SetChildIndex(this.panelPassword, 0);
            this.Controls.SetChildIndex(this.actionButtonPlay, 0);
            this.Controls.SetChildIndex(this.actionButtonShop, 0);
            this.panelName.ResumeLayout(false);
            this.panelPassword.ResumeLayout(false);
            this.panelPassword.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private Forms.SpriteButton spriteButtonWebPage;
        private Forms.SpriteButton spriteButtonForum;
        private Forms.SpriteButton spriteButtonSocial;
        private Forms.SpriteButton spriteButtonVote;
        private Forms.SpriteButton spriteButtonDelete;
        private Forms.FieldPanel panelName;
        private System.Windows.Forms.ComboBox comboBoxName;
        private Forms.FieldPanel panelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private Forms.ActionButton actionButtonPlay;
        private Forms.ActionButton actionButtonShop;
    }
}