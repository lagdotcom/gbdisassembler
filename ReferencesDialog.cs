using Lag.DisassemblerLib;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Lag.Disassembler
{
    public partial class ReferencesDialog : Form
    {
        private IInstruction[] references;

        public ReferencesDialog()
        {
            InitializeComponent();
        }

        public IEnumerable<IInstruction> References
        {
            get => references;
            set
            {
                references = value.ToArray();

                ReferenceList.Items.Clear();
                foreach (IInstruction inst in value)
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
