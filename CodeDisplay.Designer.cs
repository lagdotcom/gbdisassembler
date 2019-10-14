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
            this.components = new System.ComponentModel.Container();
            this.Scrolly = new System.Windows.Forms.VScrollBar();
            this.OperandTypeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ramAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.romAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OperandTypeMenu.SuspendLayout();
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
            // OperandTypeMenu
            // 
            this.OperandTypeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexToolStripMenuItem,
            this.decimalToolStripMenuItem,
            this.ramAddressToolStripMenuItem,
            this.romAddressToolStripMenuItem});
            this.OperandTypeMenu.Name = "OperandContextMenu";
            this.OperandTypeMenu.Size = new System.Drawing.Size(181, 114);
            // 
            // hexToolStripMenuItem
            // 
            this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
            this.hexToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.hexToolStripMenuItem.Text = "&Hex";
            this.hexToolStripMenuItem.Click += new System.EventHandler(this.HexToolStripMenuItem_Click);
            // 
            // decimalToolStripMenuItem
            // 
            this.decimalToolStripMenuItem.Name = "decimalToolStripMenuItem";
            this.decimalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.decimalToolStripMenuItem.Text = "&Decimal";
            this.decimalToolStripMenuItem.Click += new System.EventHandler(this.DecimalToolStripMenuItem_Click);
            // 
            // ramAddressToolStripMenuItem
            // 
            this.ramAddressToolStripMenuItem.Name = "ramAddressToolStripMenuItem";
            this.ramAddressToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.ramAddressToolStripMenuItem.Text = "R&AM Address";
            this.ramAddressToolStripMenuItem.Click += new System.EventHandler(this.RAMAddressToolStripMenuItem_Click);
            // 
            // romAddressToolStripMenuItem
            // 
            this.romAddressToolStripMenuItem.Name = "romAddressToolStripMenuItem";
            this.romAddressToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.romAddressToolStripMenuItem.Text = "R&OM Address";
            this.romAddressToolStripMenuItem.Click += new System.EventHandler(this.ROMAddressToolStripMenuItem_Click);
            // 
            // CodeDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Scrolly);
            this.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CodeDisplay";
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CodeDisplay_MouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CodeDisplay_MouseMove);
            this.OperandTypeMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar Scrolly;
        private System.Windows.Forms.ContextMenuStrip OperandTypeMenu;
        private System.Windows.Forms.ToolStripMenuItem hexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decimalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ramAddressToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem romAddressToolStripMenuItem;
    }
}
