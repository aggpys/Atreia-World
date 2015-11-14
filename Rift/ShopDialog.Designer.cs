namespace Rift
{
    partial class ShopDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShopDialog));
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelSearch = new Rift.Forms.FieldPanel();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.labelUser = new System.Windows.Forms.Label();
            this.labelPoints = new System.Windows.Forms.Label();
            this.panelCategory = new Rift.Forms.FieldPanel();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.labelCategory = new System.Windows.Forms.Label();
            this.flowPanel = new Rift.Forms.FlowLayoutPanel();
            this.contentWorker = new System.ComponentModel.BackgroundWorker();
            this.timerSearch = new System.Windows.Forms.Timer(this.components);
            this.timerUpdate = new System.Windows.Forms.Timer(this.components);
            this.panelTop.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelCategory.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panelSearch);
            this.panelTop.Controls.Add(this.labelUser);
            this.panelTop.Controls.Add(this.labelPoints);
            this.panelTop.Controls.Add(this.panelCategory);
            this.panelTop.Controls.Add(this.labelCategory);
            resources.ApplyResources(this.panelTop, "panelTop");
            this.panelTop.Name = "panelTop";
            // 
            // panelSearch
            // 
            this.panelSearch.BorderActiveColor = System.Drawing.Color.Green;
            this.panelSearch.BorderColor = System.Drawing.Color.DimGray;
            this.panelSearch.Controls.Add(this.textBoxSearch);
            this.panelSearch.Cursor = System.Windows.Forms.Cursors.IBeam;
            resources.ApplyResources(this.panelSearch, "panelSearch");
            this.panelSearch.Name = "panelSearch";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.textBoxSearch, "textBoxSearch");
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // labelUser
            // 
            resources.ApplyResources(this.labelUser, "labelUser");
            this.labelUser.Name = "labelUser";
            // 
            // labelPoints
            // 
            resources.ApplyResources(this.labelPoints, "labelPoints");
            this.labelPoints.ForeColor = System.Drawing.Color.Green;
            this.labelPoints.Name = "labelPoints";
            // 
            // panelCategory
            // 
            this.panelCategory.BorderActiveColor = System.Drawing.Color.Green;
            this.panelCategory.BorderColor = System.Drawing.Color.DimGray;
            this.panelCategory.Controls.Add(this.comboBoxCategory);
            resources.ApplyResources(this.panelCategory, "panelCategory");
            this.panelCategory.Name = "panelCategory";
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.comboBoxCategory, "comboBoxCategory");
            this.comboBoxCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxCategory_SelectedIndexChanged);
            // 
            // labelCategory
            // 
            resources.ApplyResources(this.labelCategory, "labelCategory");
            this.labelCategory.Name = "labelCategory";
            // 
            // flowPanel
            // 
            resources.ApplyResources(this.flowPanel, "flowPanel");
            this.flowPanel.ChildCount = 0;
            this.flowPanel.ForeColor = System.Drawing.Color.Green;
            this.flowPanel.Name = "flowPanel";
            // 
            // contentWorker
            // 
            this.contentWorker.WorkerSupportsCancellation = true;
            this.contentWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.contentWorker_DoWork);
            // 
            // timerSearch
            // 
            this.timerSearch.Interval = 1000;
            this.timerSearch.Tick += new System.EventHandler(this.timerSearch_Tick);
            // 
            // timerUpdate
            // 
            this.timerUpdate.Enabled = true;
            this.timerUpdate.Interval = 2000;
            this.timerUpdate.Tick += new System.EventHandler(this.timerUpdate_Tick);
            // 
            // ShopDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BorderColorActive = System.Drawing.Color.SeaGreen;
            this.BorderColorInactive = System.Drawing.Color.MediumSeaGreen;
            this.CaptionEnabled = true;
            this.CaptionTextColorActive = System.Drawing.Color.White;
            this.CaptionTextColorInactive = System.Drawing.Color.WhiteSmoke;
            this.ControlBox = true;
            this.Controls.Add(this.flowPanel);
            this.Controls.Add(this.panelTop);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.MinimizeBox = true;
            this.Name = "ShopDialog";
            this.Controls.SetChildIndex(this.panelTop, 0);
            this.Controls.SetChildIndex(this.flowPanel, 0);
            this.panelTop.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelCategory.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelCategory;
        private Forms.FieldPanel panelCategory;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.Label labelPoints;
        private Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.Label labelUser;
        private System.ComponentModel.BackgroundWorker contentWorker;
        private Forms.FieldPanel panelSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.Timer timerSearch;
        private System.Windows.Forms.Timer timerUpdate;
    }
}