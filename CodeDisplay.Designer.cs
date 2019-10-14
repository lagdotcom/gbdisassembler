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
            this.SetOperandHex = new System.Windows.Forms.ToolStripMenuItem();
            this.SetOperandDecimal = new System.Windows.Forms.ToolStripMenuItem();
            this.SetOperandRAM = new System.Windows.Forms.ToolStripMenuItem();
            this.SetOperandROM = new System.Windows.Forms.ToolStripMenuItem();
            this.ForceOperandBank = new System.Windows.Forms.ToolStripMenuItem();
            this.DataTypeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SetDataByte = new System.Windows.Forms.ToolStripMenuItem();
            this.SetDataWord = new System.Windows.Forms.ToolStripMenuItem();
            this.SetDataROM = new System.Windows.Forms.ToolStripMenuItem();
            this.SetDataRAM = new System.Windows.Forms.ToolStripMenuItem();
            this.OperandTypeMenu.SuspendLayout();
            this.DataTypeMenu.SuspendLayout();
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
            this.SetOperandHex,
            this.SetOperandDecimal,
            this.SetOperandRAM,
            this.SetOperandROM,
            this.ForceOperandBank});
            this.OperandTypeMenu.Name = "OperandContextMenu";
            this.OperandTypeMenu.Size = new System.Drawing.Size(172, 114);
            // 
            // SetOperandHex
            // 
            this.SetOperandHex.Name = "SetOperandHex";
            this.SetOperandHex.Size = new System.Drawing.Size(171, 22);
            this.SetOperandHex.Text = "&Hex";
            this.SetOperandHex.Click += new System.EventHandler(this.SetOperandHex_Click);
            // 
            // SetOperandDecimal
            // 
            this.SetOperandDecimal.Name = "SetOperandDecimal";
            this.SetOperandDecimal.Size = new System.Drawing.Size(171, 22);
            this.SetOperandDecimal.Text = "&Decimal";
            this.SetOperandDecimal.Click += new System.EventHandler(this.SetOperandDecimal_Click);
            // 
            // SetOperandRAM
            // 
            this.SetOperandRAM.Name = "SetOperandRAM";
            this.SetOperandRAM.Size = new System.Drawing.Size(171, 22);
            this.SetOperandRAM.Text = "R&AM Address";
            this.SetOperandRAM.Click += new System.EventHandler(this.SetOperandRAM_Click);
            // 
            // SetOperandROM
            // 
            this.SetOperandROM.Name = "SetOperandROM";
            this.SetOperandROM.Size = new System.Drawing.Size(171, 22);
            this.SetOperandROM.Text = "R&OM Address";
            this.SetOperandROM.Click += new System.EventHandler(this.SetOperandROM_Click);
            // 
            // ForceOperandBank
            // 
            this.ForceOperandBank.Name = "ForceOperandBank";
            this.ForceOperandBank.Size = new System.Drawing.Size(171, 22);
            this.ForceOperandBank.Text = "&Force ROM bank...";
            this.ForceOperandBank.Click += new System.EventHandler(this.ForceOperandBank_Click);
            // 
            // DataTypeMenu
            // 
            this.DataTypeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetDataByte,
            this.SetDataWord,
            this.SetDataROM,
            this.SetDataRAM});
            this.DataTypeMenu.Name = "DataTypeMenu";
            this.DataTypeMenu.Size = new System.Drawing.Size(181, 114);
            // 
            // SetDataByte
            // 
            this.SetDataByte.Name = "SetDataByte";
            this.SetDataByte.Size = new System.Drawing.Size(180, 22);
            this.SetDataByte.Text = "&Byte";
            this.SetDataByte.Click += new System.EventHandler(this.SetDataByte_Click);
            // 
            // SetDataWord
            // 
            this.SetDataWord.Name = "SetDataWord";
            this.SetDataWord.Size = new System.Drawing.Size(180, 22);
            this.SetDataWord.Text = "&Word";
            this.SetDataWord.Click += new System.EventHandler(this.SetDataWord_Click);
            // 
            // SetDataROM
            // 
            this.SetDataROM.Name = "SetDataROM";
            this.SetDataROM.Size = new System.Drawing.Size(180, 22);
            this.SetDataROM.Text = "R&OM Address";
            this.SetDataROM.Click += new System.EventHandler(this.SetDataROM_Click);
            // 
            // SetDataRAM
            // 
            this.SetDataRAM.Name = "SetDataRAM";
            this.SetDataRAM.Size = new System.Drawing.Size(180, 22);
            this.SetDataRAM.Text = "R&AM Address";
            this.SetDataRAM.Click += new System.EventHandler(this.SetDataRAM_Click);
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
            this.DataTypeMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar Scrolly;
        private System.Windows.Forms.ContextMenuStrip OperandTypeMenu;
        private System.Windows.Forms.ToolStripMenuItem SetOperandHex;
        private System.Windows.Forms.ToolStripMenuItem SetOperandDecimal;
        private System.Windows.Forms.ToolStripMenuItem SetOperandRAM;
        private System.Windows.Forms.ToolStripMenuItem SetOperandROM;
        private System.Windows.Forms.ToolStripMenuItem ForceOperandBank;
        private System.Windows.Forms.ContextMenuStrip DataTypeMenu;
        private System.Windows.Forms.ToolStripMenuItem SetDataByte;
        private System.Windows.Forms.ToolStripMenuItem SetDataWord;
        private System.Windows.Forms.ToolStripMenuItem SetDataROM;
        private System.Windows.Forms.ToolStripMenuItem SetDataRAM;
    }
}
