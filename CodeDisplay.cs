using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GBDisassembler
{
    using GBLib;

    public partial class CodeDisplay : UserControl
    {
        const int Row = 12;
        const int Main = 100;
        private uint offset = 0;
        private uint current = 0;
        private Disassembler project;

        public CodeDisplay()
        {
            InitializeComponent();

            MouseWheel += CodeDisplay_MouseWheel;
        }

        public uint Current
        {
            get => current;
            set
            {
                current = value;
                Invalidate();
            }
        }

        public uint Offset
        {
            get => offset;
            set
            {
                offset = value;
                Scrolly.Value = (int)offset;
                Invalidate();
            }
        }

        public Disassembler Project
        {
            get => project;
            set
            {
                project = value;
                Scrolly.Value = 0;

                if (project == null)
                    Scrolly.Visible = false;
                else
                {
                    Scrolly.Maximum = project.ROM.Length;
                    Scrolly.Visible = true;
                }

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (project == null)
            {
                e.Graphics.FillRectangle(Brushes.Gray, e.ClipRectangle);
                return;
            }

            e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);
            int x = Padding.Left;
            int y = e.ClipRectangle.Top + Padding.Top;
            uint o = offset;

            while (y < e.ClipRectangle.Bottom && o < project.ROM.Length)
            {
                if (project.Labeller.Labels.ContainsKey(o))
                {
                    y += Row;

                    string label = project.Labeller.Identify(o);
                    e.Graphics.DrawString($"{label}:", Font, Brushes.DarkGreen, x + Main, y);

                    y += Row;
                }

                if (o == current)
                    e.Graphics.FillRectangle(Brushes.Yellow, x, y, e.ClipRectangle.Width, Font.Height);

                e.Graphics.DrawString($"{o / 0x4000:X2}:{o % 0x4000:X4}", Font, Brushes.Gray, x, y);

                uint move = 1;
                string data;
                if (project.Instructions.ContainsKey(o))
                {
                    Instruction inst = project.Instructions[o];
                    data = $"{inst.OpType} {string.Join(", ", inst.OperandStrings)}";
                    move = inst.TotalSize;
                }
                else
                    data = $"db {project.ROM[o]:X2}";
                e.Graphics.DrawString(data, Font, Brushes.Black, x + Main, y);

                o += move;
                y += Row;
            }
        }

        private void CodeDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            int change = e.Delta > 0 ? Scrolly.LargeChange : -Scrolly.LargeChange;
            Scrolly.Value = (Scrolly.Value - change).Clamp(Scrolly.Minimum, Scrolly.Maximum);
        }

        private void Scrolly_ValueChanged(object sender, EventArgs e)
        {
            Offset = (uint)Scrolly.Value;
        }
    }
}
