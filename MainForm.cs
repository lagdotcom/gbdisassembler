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
    using GBLib.Operand;
    using System.IO;

    public partial class MainForm : Form
    {
        private History history;

        public MainForm()
        {
            InitializeComponent();

            history = new History(20, BackBtn, FwdBtn);
            history.Goto += History_Goto;

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
            UpdateLabels();

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

            Serializer.SaveProject(FileName, Project);

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
            BackBtn.Enabled = false;
            FwdBtn.Enabled = false;
            Code.Project = null;
            UpdateTitleBar();
            LabelsBox.Items.Clear();

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
                NamingDialog dlg = new NamingDialog
                {
                    Text = $"Label ROM{new BankedAddress(offset).ToString()}..."
                };
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

            UpdateLabels();
            ChangeMade?.Invoke(this, null);
        }

        public void CommentOffset(uint offset, string label = null)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                NamingDialog dlg = new NamingDialog
                {
                    Text = $"Comment ROM{new BankedAddress(offset).ToString()}..."
                };
                if (Project.Comments.ContainsKey(offset))
                    dlg.NameString = Project.Comments[offset];

                if (dlg.ShowDialog() == DialogResult.Cancel)
                    return;

                label = dlg.NameString;
            }

            if (string.IsNullOrWhiteSpace(label))
                Project.Comments.Remove(offset);
            else
                Project.Comments[offset] = label;

            ChangeMade?.Invoke(this, null);
        }

        public void NameAddress(uint address, string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                NamingDialog dlg = new NamingDialog
                {
                    Text = $"Label RAM:{address:X4}..."
                };
                if (Project.Namer.Names.ContainsKey(address))
                    dlg.NameString = Project.Namer.Names[address];

                if (dlg.ShowDialog() == DialogResult.Cancel)
                    return;

                name = dlg.NameString;
            }

            if (string.IsNullOrWhiteSpace(name))
                Project.Namer.Names.Remove(address);
            else
                Project.Namer.Names[address] = name;

            ChangeMade?.Invoke(this, null);
        }

        public void Analyse(uint offset)
        {
            Project.Analyse(offset, true);
            ChangeMade?.Invoke(this, null);
        }

        public void ForceData(uint offset)
        {
            if (Project.Instructions.ContainsKey(offset))
            {
                Project.Instructions.Remove(offset);
                ChangeMade?.Invoke(this, null);
            }
        }

        public void FindReferences(uint offset)
        {
            var references = Project.Instructions.Where(pair => pair.Value.Operands != null
                && pair.Value.Operands.Any(o => o.AbsoluteAddress.HasValue && o.AbsoluteAddress.Value == offset));

            if (references.Count() > 0)
            {
                ReferencesDialog dlg = new ReferencesDialog
                {
                    References = references.Select(pair => pair.Value).OrderBy(inst => inst.Location)
                };

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    history.Remember(Code.CurrentLine);
                    history.Move(dlg.Address);
                }
            }
            else MessageBox.Show("No references found.", "Warning", MessageBoxButtons.OK);
        }

        public void ShowGoto(uint offset)
        {
            BankedAddress ba = new BankedAddress(offset);
            GotoDialog dlg = new GotoDialog
            {
                Address = ba
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                history.Remember(Code.CurrentLine);
                history.Move(dlg.Address.AbsoluteAddress.Value);
            }
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

        protected void UpdateLabels()
        {
            LabelsBox.Items.Clear();

            foreach (var pair in Project.Labeller.Labels.OrderBy(pair => pair.Key))
                LabelsBox.Items.Add(new OffsetLabel(pair.Key, pair.Value));
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
                Disassembler project = Serializer.LoadProject(FileName);
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

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (Project == null) return;

            switch (e.KeyCode)
            {
                case Keys.N:
                    e.Handled = true;
                    LabelOffset(Code.CurrentLine);
                    return;

                case Keys.OemSemicolon:
                    e.Handled = true;
                    CommentOffset(Code.CurrentLine);
                    return;

                case Keys.C:
                    e.Handled = true;
                    Analyse(Code.CurrentLine);
                    return;

                case Keys.D:
                    e.Handled = true;
                    ForceData(Code.CurrentLine);
                    return;

                case Keys.L:
                    if (Code.CurrentOp != null && Code.CurrentOp.AbsoluteAddress.HasValue)
                    {
                        e.Handled = true;
                        NameAddress(Code.CurrentOp.AbsoluteAddress.Value);
                    }
                    return;

                case Keys.X:
                    e.Handled = true;
                    FindReferences(Code.CurrentLine);
                    return;

                case Keys.G:
                    e.Handled = true;
                    ShowGoto(Code.CurrentLine);
                    return;

                case Keys.PageDown:
                    e.Handled = true;
                    Code.Offset += CodeDisplay.BigJump;
                    return;

                case Keys.PageUp:
                    e.Handled = true;
                    Code.Offset -= CodeDisplay.BigJump;
                    return;

                case Keys.Down:
                    e.Handled = true;
                    Code.MoveDown();
                    return;

                case Keys.Up:
                    e.Handled = true;
                    Code.MoveUp();
                    return;
            }
        }
        
        private void History_Goto(object sender, GotoEventArgs e)
        {
            Code.CurrentLine = e.Location;
            Code.Offset = e.Location;
        }

        private void Code_Goto(object sender, GotoEventArgs e)
        {
            history.Remember(Code.Offset);
            history.Move(e.Location);
        }

        private void Code_Replace(object sender, ReplaceEventArgs e)
        {
            Project.AddCustomOperand(e.Location, e.Index, e.Operand);
            ChangeMade?.Invoke(this, null);
        }

        private void Code_Data(object sender, DataEventArgs e)
        {
            Project.DataTypes[e.Location] = e.Type;
            ChangeMade?.Invoke(this, null);
        }

        private void LabelsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LabelsBox.SelectedItem is OffsetLabel ol)
                Code_Goto(this, new GotoEventArgs(ol.Offset));
        }

        private class OffsetLabel
        {
            public OffsetLabel(uint offset, string label)
            {
                Offset = offset;
                Label = label;
            }

            public uint Offset;
            public string Label;

            public override string ToString() => Label;
        }
    }
}
