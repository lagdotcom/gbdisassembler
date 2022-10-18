namespace Lag.Disassembler
{
    partial class GotoDialog
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
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.SegBox = new System.Windows.Forms.ComboBox();
            this.OffsetBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(197, 37);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OKBtn
            // 
            this.OKBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKBtn.Location = new System.Drawing.Point(12, 37);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            // 
            // SegBox
            // 
            this.SegBox.FormattingEnabled = true;
            this.SegBox.Location = new System.Drawing.Point(12, 10);
            this.SegBox.Name = "SegBox";
            this.SegBox.Size = new System.Drawing.Size(127, 21);
            this.SegBox.TabIndex = 6;
            // 
            // OffsetBox
            // 
            this.OffsetBox.Location = new System.Drawing.Point(145, 10);
            this.OffsetBox.Name = "OffsetBox";
            this.OffsetBox.Size = new System.Drawing.Size(127, 20);
            this.OffsetBox.TabIndex = 7;
            // 
            // GotoDialog
            // 
            this.AcceptButton = this.OKBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelBtn;
            this.ClientSize = new System.Drawing.Size(284, 70);
            this.Controls.Add(this.OffsetBox);
            this.Controls.Add(this.SegBox);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GotoDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Go to...";
            this.Shown += new System.EventHandler(this.GotoDialog_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.ComboBox SegBox;
        private System.Windows.Forms.TextBox OffsetBox;
    }
}