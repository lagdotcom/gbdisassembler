namespace GBDisassembler
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TopMenu = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenRomDialog = new System.Windows.Forms.OpenFileDialog();
            this.OpenProjectDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveProjectDialog = new System.Windows.Forms.SaveFileDialog();
            this.ToolPanel = new System.Windows.Forms.Panel();
            this.FwdBtn = new System.Windows.Forms.Button();
            this.BackBtn = new System.Windows.Forms.Button();
            this.LabelsBox = new System.Windows.Forms.ListBox();
            this.Code = new GBDisassembler.CodeDisplay();
            this.TopMenu.SuspendLayout();
            this.ToolPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopMenu
            // 
            this.TopMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem});
            this.TopMenu.Location = new System.Drawing.Point(0, 0);
            this.TopMenu.Name = "TopMenu";
            this.TopMenu.Size = new System.Drawing.Size(800, 24);
            this.TopMenu.TabIndex = 0;
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewToolStripMenuItem,
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.CloseToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "&File";
            // 
            // NewToolStripMenuItem
            // 
            this.NewToolStripMenuItem.Name = "NewToolStripMenuItem";
            this.NewToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.NewToolStripMenuItem.Text = "&New...";
            this.NewToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.OpenToolStripMenuItem.Text = "&Open...";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Enabled = false;
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.SaveToolStripMenuItem.Text = "&Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // CloseToolStripMenuItem
            // 
            this.CloseToolStripMenuItem.Enabled = false;
            this.CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            this.CloseToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.CloseToolStripMenuItem.Text = "&Close";
            this.CloseToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.ExitToolStripMenuItem.Text = "E&xit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // OpenRomDialog
            // 
            this.OpenRomDialog.Filter = "GameBoy ROMs|*.gb;*.gbc";
            // 
            // OpenProjectDialog
            // 
            this.OpenProjectDialog.Filter = "Project Files|*.gbdprj";
            // 
            // SaveProjectDialog
            // 
            this.SaveProjectDialog.Filter = "Project Files|*.gbdprj";
            // 
            // ToolPanel
            // 
            this.ToolPanel.Controls.Add(this.FwdBtn);
            this.ToolPanel.Controls.Add(this.BackBtn);
            this.ToolPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ToolPanel.Location = new System.Drawing.Point(0, 24);
            this.ToolPanel.Name = "ToolPanel";
            this.ToolPanel.Padding = new System.Windows.Forms.Padding(3);
            this.ToolPanel.Size = new System.Drawing.Size(800, 36);
            this.ToolPanel.TabIndex = 2;
            // 
            // FwdBtn
            // 
            this.FwdBtn.Dock = System.Windows.Forms.DockStyle.Left;
            this.FwdBtn.Enabled = false;
            this.FwdBtn.Location = new System.Drawing.Point(33, 3);
            this.FwdBtn.Margin = new System.Windows.Forms.Padding(5);
            this.FwdBtn.Name = "FwdBtn";
            this.FwdBtn.Size = new System.Drawing.Size(30, 30);
            this.FwdBtn.TabIndex = 1;
            this.FwdBtn.Text = ">";
            this.FwdBtn.UseVisualStyleBackColor = true;
            // 
            // BackBtn
            // 
            this.BackBtn.Dock = System.Windows.Forms.DockStyle.Left;
            this.BackBtn.Enabled = false;
            this.BackBtn.Location = new System.Drawing.Point(3, 3);
            this.BackBtn.Margin = new System.Windows.Forms.Padding(5);
            this.BackBtn.Name = "BackBtn";
            this.BackBtn.Size = new System.Drawing.Size(30, 30);
            this.BackBtn.TabIndex = 0;
            this.BackBtn.Text = "<";
            this.BackBtn.UseVisualStyleBackColor = true;
            // 
            // LabelsBox
            // 
            this.LabelsBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.LabelsBox.FormattingEnabled = true;
            this.LabelsBox.Location = new System.Drawing.Point(680, 60);
            this.LabelsBox.Name = "LabelsBox";
            this.LabelsBox.Size = new System.Drawing.Size(120, 390);
            this.LabelsBox.TabIndex = 4;
            this.LabelsBox.SelectedIndexChanged += new System.EventHandler(this.LabelsBox_SelectedIndexChanged);
            // 
            // Code
            // 
            this.Code.CurrentLine = ((uint)(0u));
            this.Code.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Code.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Code.Location = new System.Drawing.Point(0, 60);
            this.Code.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Code.Name = "Code";
            this.Code.Offset = ((uint)(0u));
            this.Code.Project = null;
            this.Code.Size = new System.Drawing.Size(680, 390);
            this.Code.TabIndex = 5;
            this.Code.Data += new System.EventHandler<GBDisassembler.DataEventArgs>(this.Code_Data);
            this.Code.Goto += new System.EventHandler<GBDisassembler.GotoEventArgs>(this.Code_Goto);
            this.Code.Replace += new System.EventHandler<GBDisassembler.ReplaceEventArgs>(this.Code_Replace);
            this.Code.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Code);
            this.Controls.Add(this.LabelsBox);
            this.Controls.Add(this.ToolPanel);
            this.Controls.Add(this.TopMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.TopMenu;
            this.Name = "MainForm";
            this.Text = "GBDisassembler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.TopMenu.ResumeLayout(false);
            this.TopMenu.PerformLayout();
            this.ToolPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip TopMenu;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenRomDialog;
        private System.Windows.Forms.OpenFileDialog OpenProjectDialog;
        private System.Windows.Forms.SaveFileDialog SaveProjectDialog;
        private System.Windows.Forms.Panel ToolPanel;
        private System.Windows.Forms.Button FwdBtn;
        private System.Windows.Forms.Button BackBtn;
        private System.Windows.Forms.ListBox LabelsBox;
        private CodeDisplay Code;
    }
}

