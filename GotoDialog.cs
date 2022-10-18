using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Lag.Disassembler
{
    public partial class GotoDialog : Form
    {
        private IEnumerable<Segment> segs;

        public GotoDialog()
        {
            segs = Array.Empty<Segment>();
            InitializeComponent();
        }

        public Segment Seg
        {
            get => (Segment)SegBox.SelectedItem;
            set => SegBox.SelectedItem = value;
        }

        public uint Offset
        {
            get => uint.Parse(OffsetBox.Text, NumberStyles.HexNumber);
            set => OffsetBox.Text = value.ToString("X4");
        }

        public IEnumerable<Segment> Segments
        {
            get => segs;
            set
            {
                Segment sel = Seg;

                segs = value;
                SegBox.Items.Clear();
                foreach (Segment seg in segs)
                    SegBox.Items.Add(seg);

                Seg = sel;
            }
        }

        public Word Address
        {
            get => new Word(Seg, Offset);
            set
            {
                Seg = value.Seg;
                Offset = value.Offset;
            }
        }

        private void GotoDialog_Shown(object sender, EventArgs e)
        {
            OffsetBox.Focus();
        }
    }
}
