namespace Lag.Disassembler
{
    partial class ReferencesDialog
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
            this.ReferenceList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ReferenceList
            // 
            this.ReferenceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReferenceList.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReferenceList.FormattingEnabled = true;
            this.ReferenceList.ItemHeight = 16;
            this.ReferenceList.Location = new System.Drawing.Point(0, 0);
            this.ReferenceList.Name = "ReferenceList";
            this.ReferenceList.Size = new System.Drawing.Size(384, 361);
            this.ReferenceList.TabIndex = 0;
            this.ReferenceList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ReferenceList_MouseDoubleClick);
            // 
            // ReferencesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.ReferenceList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ReferencesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "References...";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ReferenceList;
    }
}