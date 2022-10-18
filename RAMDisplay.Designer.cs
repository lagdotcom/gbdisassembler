namespace Lag.Disassembler
{
    partial class RAMDisplay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Scrolly = new System.Windows.Forms.VScrollBar();
            this.SuspendLayout();
            // 
            // Scrolly
            // 
            this.Scrolly.Dock = System.Windows.Forms.DockStyle.Right;
            this.Scrolly.Location = new System.Drawing.Point(183, 0);
            this.Scrolly.Name = "Scrolly";
            this.Scrolly.Size = new System.Drawing.Size(17, 151);
            this.Scrolly.TabIndex = 0;
            this.Scrolly.Visible = false;
            this.Scrolly.ValueChanged += new System.EventHandler(this.Scrolly_ValueChanged);
            // 
            // RAMDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Scrolly);
            this.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.Name = "RAMDisplay";
            this.Size = new System.Drawing.Size(200, 151);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar Scrolly;
    }
}
