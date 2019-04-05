using GBLib;
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
    public partial class ReferencesDialog : Form
    {
        private Instruction[] references;

        public ReferencesDialog()
        {
            InitializeComponent();
        }

        public IEnumerable<Instruction> References
        {
            get => references;
            set
            {
                references = value.ToArray();

                ReferenceList.Items.Clear();
                foreach (Instruction inst in value)
                    ReferenceList.Items.Add(inst.ToString());
            }
        }

        public uint Address { get; private set; }

        private void ReferenceList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ReferenceList.SelectedIndex >= 0)
            {
                Address = references[ReferenceList.SelectedIndex].Location;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
