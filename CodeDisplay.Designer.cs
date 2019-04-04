namespace GBDisassembler
{
    partial class CodeDisplay
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
            this.Scrolly.Location = new System.Drawing.Point(133, 0);
            this.Scrolly.Name = "Scrolly";
            this.Scrolly.Size = new System.Drawing.Size(17, 150);
            this.Scrolly.TabIndex = 0;
            this.Scrolly.Visible = false;
            this.Scrolly.ValueChanged += new System.EventHandler(this.Scrolly_ValueChanged);
            // 
            // CodeDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Scrolly);
            this.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CodeDisplay";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar Scrolly;
    }
}
