using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBDisassembler
{
    using GBLib;
    using System.IO;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            ChangeMade += MainForm_ChangeMade;
        }

        public Disassembler Project { get; private set; }
        public string FileName { get; private set; }
        public bool UnsavedChanges { get; private set; }

        public event EventHandler ChangeMade;
        public event EventHandler ProjectClosed;
        public event EventHandler ProjectLoaded;
        public event EventHandler ProjectSaved;

        public void LoadProject(Disassembler project, bool isNew = false)
        {
            Project = project;
            CloseToolStripMenuItem.Enabled = true;
            SaveToolStripMenuItem.Enabled = true;
            Code.Project = project;
            UpdateTitleBar();

            ProjectLoaded?.Invoke(this, null);
            if (isNew) ChangeMade?.Invoke(this, null);
        }

        public bool SaveProject()
        {
            if (Project == null) return true;

            if (string.IsNullOrEmpty(FileName))
            {
                if (SaveProjectDialog.ShowDialog() != DialogResult.OK)
                    return false;

                FileName = SaveProjectDialog.FileName;
            }

            Project.Save(FileName);
            UnsavedChanges = false;
            UpdateTitleBar();

            ProjectSaved?.Invoke(this, null);
            return true;
        }

        public void CloseProject()
        {
            Project = null;
            CloseToolStripMenuItem.Enabled = false;
            SaveToolStripMenuItem.Enabled = false;
            Code.Project = null;
            UpdateTitleBar();

            ProjectClosed?.Invoke(this, null);
        }

        public bool SaveBeforeClosing()
        {
            if (Project != null && UnsavedChanges)
            {
                switch (MessageBox.Show("Save changes before closing?", "Warning", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        if (!SaveProject()) return false;
                        break;

                    case DialogResult.No:
                        CloseProject();
                        break;

                    case DialogResult.Cancel:
                        return false;
                }
            }

            return true;
        }

        public void LabelOffset(uint offset, string label = null)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                NamingDialog dlg = new NamingDialog();
                if (Project.Labeller.Labels.ContainsKey(offset))
                    dlg.NameString = Project.Labeller.Labels[offset];

                if (dlg.ShowDialog() == DialogResult.Cancel)
                    return;

                label = dlg.NameString;
            }

            if (string.IsNullOrWhiteSpace(label))
                Project.Labeller.Labels.Remove(offset);
            else
                Project.Labeller.Labels[offset] = label;

            ChangeMade?.Invoke(this, null);
        }

        public void Analyse(uint offset)
        {
            Project.Analyse(offset);
            ChangeMade?.Invoke(this, null);
        }

        protected void UpdateTitleBar()
        {
            if (Project == null)
            {
                Text = "GBDisassembler";
                return;
            }

            if (string.IsNullOrEmpty(FileName))
            {
                Text = "GBDisassembler - New Project*";
                return;
            }

            Text = $"GBDisassembler - {Path.GetFileName(FileName)}{(UnsavedChanges ? "*" : "")}";
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveBeforeClosing()) return;

            if (OpenRomDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = string.Empty;
                Disassembler project = new Disassembler(OpenRomDialog.FileName);
                project.StandardAnalysis();
                LoadProject(project, true);
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveBeforeClosing()) return;

            if (OpenProjectDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = OpenProjectDialog.FileName;
                Disassembler project = Disassembler.Load(FileName);
                LoadProject(project);
            }
        }

        private void MainForm_ChangeMade(object sender, EventArgs e)
        {
            UnsavedChanges = true;
            UpdateTitleBar();
            Code.Invalidate();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SaveBeforeClosing()) return;

            CloseProject();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
            {
                if (!string.IsNullOrEmpty(FileName))
                    SaveProject();

                return;
            }

            e.Cancel = !SaveBeforeClosing();
        }
        
        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Project == null) return;

            switch (e.KeyChar)
            {
                case 'n':
                case 'N':
                    e.Handled = true;
                    LabelOffset(Code.CurrentLine);
                    return;

                case 'c':
                case 'C':
                    e.Handled = true;
                    Analyse(Code.CurrentLine);
                    return;
            }
        }
    }
}
