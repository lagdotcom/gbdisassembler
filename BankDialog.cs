using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lag.Disassembler
{
    public partial class BankDialog : Form
    {
        private IEnumerable<Segment> segs;

        public BankDialog()
        {
            segs = Array.Empty<Segment>();
            InitializeComponent();
        }

        public Segment Seg
        {
            get => (Segment)SegBox.SelectedItem;
            set => SegBox.SelectedItem = value;
        }

        public IEnumerable<Segment> Segments
        {
            get => segs;
            set
            {
                segs = value;
                SegBox.Items.Clear();
                foreach (Segment seg in segs)
                    SegBox.Items.Add(seg);
            }
        }

        private void BankDialog_Shown(object sender, EventArgs e)
        {
            SegBox.Focus();
        }
    }
}
