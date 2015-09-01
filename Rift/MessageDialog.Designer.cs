namespace Rift
{
    partial class MessageDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageDialog));
            this.labelTitle = new System.Windows.Forms.Label();
            this.buttonOk = new System.Windows.Forms.Button();
            this.textBoxError = new System.Windows.Forms.Label();
            this.pictureBoxTitle = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            this.labelTitle.ForeColor = System.Drawing.Color.DimGray;
            this.labelTitle.Name = "labelTitle";
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.FlatAppearance.BorderColor = System.Drawing.Color.Firebrick;
            this.buttonOk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Firebrick;
            this.buttonOk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.IndianRed;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // textBoxError
            // 
            this.textBoxError.ForeColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.textBoxError, "textBoxError");
            this.textBoxError.Name = "textBoxError";
            // 
            // pictureBoxTitle
            // 
            resources.ApplyResources(this.pictureBoxTitle, "pictureBoxTitle");
            this.pictureBoxTitle.BackgroundImage = global::Rift.Properties.Resources.ImageMessageDialog;
            this.pictureBoxTitle.Name = "pictureBoxTitle";
            this.pictureBoxTitle.TabStop = false;
            // 
            // MessageDialog
            // 
            this.AcceptButton = this.buttonOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.BorderColorActive = System.Drawing.Color.Firebrick;
            this.BorderColorInactive = System.Drawing.Color.WhiteSmoke;
            this.CaptionEnabled = true;
            this.CaptionTextColorInactive = System.Drawing.Color.DarkGray;
            resources.ApplyResources(this, "$this");
            this.ControlBox = true;
            this.Controls.Add(this.textBoxError);
            this.Controls.Add(this.pictureBoxTitle);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.buttonOk);
            this.Name = "MessageDialog";
            this.Controls.SetChildIndex(this.buttonOk, 0);
            this.Controls.SetChildIndex(this.labelTitle, 0);
            this.Controls.SetChildIndex(this.pictureBoxTitle, 0);
            this.Controls.SetChildIndex(this.textBoxError, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label textBoxError;
        private System.Windows.Forms.PictureBox pictureBoxTitle;
    }
}